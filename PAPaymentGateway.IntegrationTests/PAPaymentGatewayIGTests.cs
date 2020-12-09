using PAPaymentGateway.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace PAPaymentGateway.IntegrationTests
{
    public class PAPaymentGatewayIGTests : IClassFixture<IntegrationTestsWebApplicationFactory<PAPaymentGateway.API.Startup>>
    {
        private readonly IntegrationTestsWebApplicationFactory<PAPaymentGateway.API.Startup> _factory;

        private const string AdminUsername = "PAPaymentGatewayAdmin";
        private const string AdminUserpassword = "PAPaymentGateway1@";

        public PAPaymentGatewayIGTests(IntegrationTestsWebApplicationFactory<PAPaymentGateway.API.Startup> factory )
        {
            _factory = factory;
        }

        #region Authentication

        [Theory]
        [InlineData(AdminUsername, AdminUserpassword)]
        public async void Get_User_Authenticate_Success(string username,string password)
        {
            // Arrange
            var client = _factory.CreateClient();

            HttpContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string,string>("username",username),
                new KeyValuePair<string, string>("password",password)
            });

            // Act
            var response = await client.PostAsync("users/authenticate",content);

            // Assert
            Assert.Equal((int)response.StatusCode, (int) System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(AdminUsername, "IncorrectPassword@")]
        public async void Get_Users_Authenticate_Fail(string username, string password)
        {
            // Arrange
            var client = _factory.CreateClient();

            HttpContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string,string>("username",username),
                new KeyValuePair<string, string>("password",password)
            });

            // Act
            var response = await client.PostAsync("users/authenticate", content);

            // Assert
            Assert.Equal((int)response.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);

        }
   
        #endregion

        #region Payment
        [Fact]
        public async void Post_Payment_All_Fields_Valid_Success() 
        {
            // Arrange
            var client = await SetupAdminClient();

            // Setup Payment
            Payment payment = new Payment();
            payment.Amount = 5.00f;
            payment.Card = new Card
            {
                CardNumber = "4929890681341628",
                Cvv = "569",
                ExpiryDate = DateTime.Now.AddYears(2),                
            };
            payment.CurrencyCode = "EUR";

            var jsonObject = JsonSerializer.Serialize(payment);

            HttpContent httpContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("payments", httpContent);

            // Assert
            Assert.Equal((int)response.StatusCode, (int)System.Net.HttpStatusCode.Created);

        }


        [Fact]
        public async void Post_Payment_Negative_Number()
        {
            // Arrange
            var client = await SetupAdminClient();

            // Setup Payment
            Payment payment = new Payment();
            payment.Amount = -1.00f;
            payment.Card = new Card
            {
                CardNumber = "4929890681341628",
                Cvv = "123",
                ExpiryDate = DateTime.Now.AddYears(2),
            };
            payment.CurrencyCode = "EUR";

            var jsonObject = JsonSerializer.Serialize(payment);

            HttpContent httpContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("payments", httpContent);

            // Assert
            Assert.Equal((int)response.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);

        }

        [Fact]
        public async void Post_Payment_Invalid_Cvv_Length()
        {
            // Arrange
            var client = await SetupAdminClient();


            // Setup Payment
            Payment payment = new Payment();
            payment.Amount = 5.00f;
            payment.Card = new Card
            {
                CardNumber = "4929890681341628",
                Cvv = "123xffd",
                ExpiryDate = DateTime.Now.AddYears(2),
            };
            payment.CurrencyCode = "EUR";

            var jsonObject = JsonSerializer.Serialize(payment);

            HttpContent httpContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("payments", httpContent);

            // Assert
            Assert.Equal((int)response.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);

        }
        #endregion

        #region Merchants
        [Fact]
        public async void Post_Merchant_Success()
        {
            // Arrange
            var client = await SetupAdminClient();

            // Setup Merchants
            Merchant merchant = new Merchant();
            merchant.Name = "Paul's Merchant 001";

            var jsonObject = JsonSerializer.Serialize(merchant);

            HttpContent httpContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("merchants", httpContent);

            // Assert
            Assert.Equal((int)response.StatusCode, (int)System.Net.HttpStatusCode.Created);

        }


        [Fact]
        public async void Get_Merchant_Success()
        {
            // Arrange
            var client =  await SetupAdminClient();

            // Act
            var response = await client.GetAsync("merchants/3");

            // Assert
            Assert.Equal((int)response.StatusCode, (int)System.Net.HttpStatusCode.OK);

        }
        #endregion

        /// <summary>
        /// This will setup the Client and the required headers
        /// for the Admin Test Account.
        /// </summary>
        /// <returns>Returns a HTTP Client</returns>
        private async Task<HttpClient> SetupAdminClient() 
        {
            var client = _factory.CreateClient();
            var token = await _factory.GetAuthenticationToken(AdminUsername);

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

            return client;
        }
    }
}

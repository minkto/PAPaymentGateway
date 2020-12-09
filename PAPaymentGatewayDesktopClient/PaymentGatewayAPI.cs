using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PAPaymentGatewayClient
{
    public class PaymentGatewayAPI
    {
        public async Task<int> Authenticate(string username, string password)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5000/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //NOTE : For Development purposes, keep this for now to ignore certificate errors.
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            HttpContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string,string>("username",username),
                new KeyValuePair<string, string>("password",password)
            });


            try 
            {
                using (HttpResponseMessage hr = await httpClient.PostAsync("/users/authenticate", content))
                {
                    if (hr.IsSuccessStatusCode)
                    {
                        var result = await hr.Content.ReadAsAsync<LoggedInUser>();
                        
                        LoggedInUser loggedInUser = new LoggedInUser();
                        loggedInUser.Username = result.Username;
                        loggedInUser.Token = result.Token;                        
                    }
                    else
                    {
                        throw new Exception(hr.ReasonPhrase);
                    }
                }
            }             
            catch(Exception ex)             
            { 
                
            }
            return -1;
        }




    }
}

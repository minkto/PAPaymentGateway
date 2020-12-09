using PAPaymentGateway.Core.Interfaces;
using PAPaymentGateway.Core.Models;
using System;
using System.Threading.Tasks;

namespace PAPaymentGateway.API.Services
{
    public class TestBankProcessingService : IBankProcessingService
    {
        private enum BankStatusCodes
        {
            Accepted = 0,
            Rejected = 1,
            Failed = 2
        }

        protected readonly IPaymentService _paymentService;

        public TestBankProcessingService()
        {            
        }

        public TestBankProcessingService(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<Payment> ProcessPaymentRequest(Payment payment)
        {
            Payment newPayment = null;

            // Validate Payment Details Request.
            BankPaymentProcessingResponse bankResponseObject = await SendPaymentDetails();

            // If this is successful, then carry out the Implemmentation of saving the Payment Details
            if(bankResponseObject.StatusCode == (int) BankStatusCodes.Accepted) // Success
            {
               payment.BankPaymentUID = bankResponseObject.PaymentUID;
               newPayment = await _paymentService.SavePaymentDetails(payment);

            }
            else 
            { 
                // Log the response from the bank.
            }

            return newPayment;
        }

        public async Task<BankPaymentProcessingResponse> SendPaymentDetails() 
        {
            // Call the API which will hook into an acquiring bank, and obtain the response.
            BankPaymentProcessingResponse responseObject = new BankPaymentProcessingResponse();
            responseObject.StatusCode = 0;
            responseObject.PaymentUID = Guid.NewGuid();
            responseObject.ResponseMessage = "Payment Success";

            // NOTE: With a real bank, there is likely to be a some other bit's for this.

            return responseObject;
        }
    }
}

using System;

namespace PAPaymentGateway.Core.Models
{
    public class BankPaymentProcessingResponse
    {
        /// <summary>
        /// This will represent a unique Payment ID value from the Bank.
        /// </summary>
        public Guid PaymentUID { get; set; }

        /// <summary>
        /// The code that corresponds to the response given from the
        /// bank.
        /// </summary>
        public int StatusCode { get; set; }
        
        /// <summary>
        /// This represents a response message that can obtained
        /// from any bank.
        /// </summary>
        public string ResponseMessage { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PAPaymentGateway.API
{
    public class PAPaymentGatewayAPIException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public PAPaymentGatewayAPIException() : base()
        {                
        }

        public PAPaymentGatewayAPIException(string message) : base(message)
        {
        }

        public PAPaymentGatewayAPIException(string message,Exception innerException) : base(message,innerException)
        {
        }

        public PAPaymentGatewayAPIException(HttpStatusCode httpStatusCode) : base()
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}

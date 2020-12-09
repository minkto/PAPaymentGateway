using System;
using System.ComponentModel.DataAnnotations;

namespace PAPaymentGateway.Core.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public Guid PaymentUID { get; set; }

        /// <summary>
        /// The UID in which the Payment Request
        /// was fulfilled by the Bank.
        /// </summary>
        public Guid BankPaymentUID { get; set; }

        /// <summary>
        /// The amount being paid.
        /// </summary>
        [Required]
        [Range(0,double.MaxValue)]
        public double? Amount { get; set; }

        /// <summary>
        /// The currency code that represents this payment.
        /// </summary>
        [Required]
        [MinLength(3)]
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = "Card Information is required.")]
        public Card Card { get; set; }

        public int CardId { get; set; }
                
        [Required]
        public int MerchantID { get; set; }
        public int Status { get; set; }
        public DateTime SubmittedDate { get; private set; }

        public Payment()
        {
            SubmittedDate = DateTime.Now;
            PaymentUID = Guid.NewGuid();
            Status = 0;
        }
    }
}

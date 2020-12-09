using System;
using System.ComponentModel.DataAnnotations;

namespace PAPaymentGateway.Core.Models
{
    public class Card
    {
        /// <summary>
        /// The ID of the card.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The full card number.
        /// </summary>
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        /// <summary>
        /// The dtae of which the card expires.
        /// </summary>
        [Required]
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// The CVV number for the card.
        /// </summary>
        [Required]
        [StringLength(4)]
        public string Cvv { get; set; }
    }
}

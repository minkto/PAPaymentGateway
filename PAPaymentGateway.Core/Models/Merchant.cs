using System.ComponentModel.DataAnnotations;

namespace PAPaymentGateway.Core.Models
{
    /// <summary>
    /// This will represent the selected Merchant.
    /// </summary>
    public class Merchant
    {
        /// <summary>
        /// ID for the Merchant.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the Merchant.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The User ID which represents which user has
        /// the merchant.
        /// </summary>
        public string UserID { get; set; }
    }
}

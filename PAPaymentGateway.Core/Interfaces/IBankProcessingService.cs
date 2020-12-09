using PAPaymentGateway.Core.Models;
using System.Threading.Tasks;

namespace PAPaymentGateway.Core.Interfaces
{
    public interface IBankProcessingService
    {
        Task<Payment> ProcessPaymentRequest(Payment payment);
    }
}

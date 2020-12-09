using PAPaymentGateway.Core.Models;
using System.Threading.Tasks;

namespace PAPaymentGateway.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> SavePaymentDetails(Payment payment);

        Payment GetPaymentById(int id,string userID);

    }
}

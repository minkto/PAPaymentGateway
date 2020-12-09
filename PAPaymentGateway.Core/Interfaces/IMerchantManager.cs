using PAPaymentGateway.Core.Models;
using System.Threading.Tasks;

namespace PAPaymentGateway.Core.Interfaces
{
    public interface IMerchantManager
    {
        bool CheckUserHasMerchant(string userID);

        Task<Merchant> CreateMerchant(Merchant merchant);

        Task<Merchant> GetMerchant(int merchantID, string userID);

    }
}

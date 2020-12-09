using System.Threading.Tasks;

namespace PAPaymentGateway.Core.Interfaces
{
    public interface ITokenManagerService
    {
        public Task<string> CreateToken(string username);
    }
}

using Microsoft.EntityFrameworkCore;
using PAPaymentGateway.Core.Interfaces;
using PAPaymentGateway.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PAPaymentGateway.API.Services
{
    /// <summary>
    /// Service for handling the interactions of Merchants.
    /// </summary>
    public class MerchantService : IMerchantManager
    {
        protected ApplicationDbContext _context { get; set; }


        public MerchantService(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Merchant> CreateMerchant(Merchant merchant) 
        {
            try 
            { 
                _context.Add(merchant);
                await _context.SaveChangesAsync();
            }

            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }

            return merchant;
        }

        public async Task<Merchant> GetMerchant(int merchantID,string userID) 
        {
            try 
            {
                var merchant = await _context.Merchants.Where(m => m.Id == merchantID && m.UserID == userID).FirstAsync();
                return merchant;
            }

            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }


        public bool CheckUserHasMerchant(string userID)
        {
           bool result = false;

           var merchant = _context.Merchants.Where(m=> m.UserID == userID);
            if (merchant != null)
                result = true;

            return result;

        }
    }
}

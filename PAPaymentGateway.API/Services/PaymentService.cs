using Microsoft.EntityFrameworkCore;
using PAPaymentGateway.Core.Interfaces;
using PAPaymentGateway.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PAPaymentGateway.API.Services
{
    public class PaymentService : IPaymentService
    {
        protected ApplicationDbContext _context { get; set; }  
        
        protected ILoggingService _loggingService { get; set; }


        protected IMerchantManager _merchantManager { get; set; }

        public PaymentService(ApplicationDbContext context,
            ILoggingService loggingService,
            IMerchantManager merchantManager
            ) 
        {
            _context = context;
            _loggingService = loggingService;
            _merchantManager = merchantManager;
        }

        public Payment GetPaymentById(int id,string userID)
        {
            Payment previousPayment = null;

            if (_merchantManager.CheckUserHasMerchant(userID)) 
            {
                previousPayment = _context.Payments.SingleOrDefault(p => p.Id == id);
            }

            return previousPayment;
        } 
        
        public string MaskCardNumber(string cardNumber) 
        {
            // 1. Get the last 4 digits.
            
            int endOfMaskingIndex = cardNumber.Length - 5;
            char[] maskedNumber = new char[cardNumber.Length];

            for (int i = 0; i < cardNumber.Length; i++) 
            {
                if (i <= endOfMaskingIndex)
                    maskedNumber[i] = 'X';
                else
                    maskedNumber[i] = cardNumber[i];
            }

            return new string(maskedNumber);            
        }


        public async Task<Payment> SavePaymentDetails(Payment payment)
        {
            if (payment == null) 
            {
                throw new Exception("Payment must have a value.");
            }

            if (payment.Card == null) 
            {
                throw new Exception("Card Information is invalid.");
            }

            payment.Card.CardNumber = MaskCardNumber(payment.Card.CardNumber);

            try
            {
                _context.Add(payment);
                var saved = await _context.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return payment;
        }
    }
}

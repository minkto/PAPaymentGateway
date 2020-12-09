using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PAPaymentGateway.Core.Interfaces;
using PAPaymentGateway.Core.Models;
using System;
using System.Threading.Tasks;

namespace PAPaymentGateway.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentsController : ControllerBase
    {
        protected readonly IPaymentService _paymentService;

        protected readonly IBankProcessingService _bankProcessingService;

        protected readonly ILoggingService _loggingService;
        protected UserManager<IdentityUser> _userManager { get; set; }

        public PaymentsController(IPaymentService paymentService,
            IBankProcessingService bankProcessingService,
            ILoggingService loggingService,
            UserManager<IdentityUser> userManager) 
        {
            _paymentService = paymentService;
            _loggingService = loggingService;
            _bankProcessingService = bankProcessingService;
            _userManager = userManager;

        }

        [HttpGet("{id}")]        
        public ActionResult<Payment> Get(int id) 
        {
            string userID = _userManager.GetUserId(User);
            var payment = _paymentService.GetPaymentById(id, userID);
            
            if (payment == null)
                return NotFound();

            return payment;
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> Post([FromBody]Payment payment)
        {
            try 
            {
                if (!ModelState.IsValid)                
                    return BadRequest(ModelState);
              
                var processedPayment = await _bankProcessingService.ProcessPaymentRequest(payment);

                if (processedPayment == null)                  
                    return BadRequest();                
                else                 
                    return Created("/payments/", new { identifier = payment.BankPaymentUID, status = "success" });
                
            }
            catch (Exception ex) 
            {
                return BadRequest("An Error has occured with the payment.");
            }            
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PAPaymentGateway.Core.Interfaces;
using PAPaymentGateway.Core.Models;
using System.Threading.Tasks;

namespace PAPaymentGateway.Controllers
{
    /// <summary>
    /// This controller is for managing the merchants.
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class MerchantsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly IMerchantManager _merchantManager;

        public MerchantsController(UserManager<IdentityUser> userManager,
            IMerchantManager merchantManager)
        {
            _userManager = userManager;
            _merchantManager = merchantManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Merchant>> Get(int id) 
        {
            var merchant = await _merchantManager.GetMerchant(id, _userManager.GetUserId(User));
            
            if (merchant == null)
                return NotFound();

            return merchant;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Merchant merchant) 
        {
            if (!ModelState.IsValid)
                BadRequest();

            string userID = _userManager.GetUserId(User);
            merchant.UserID = userID;

            var result = await  _merchantManager.CreateMerchant(merchant);

            if (result.Id > 0)
                return Created("/merchants/", "Success");
            else 
                return BadRequest(); 
        }

    }
}

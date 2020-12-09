using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PAPaymentGateway.Core.Interfaces;
using PAPaymentGateway.Core.Models;
using PAPaymentGateway.Core.Models.Identity;
using System.Threading.Tasks;

namespace PAPaymentGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenManagerService _tokenManagerService;

        public UsersController(UserManager<IdentityUser> userManager, ITokenManagerService tokenManagerService)
        {
            _userManager = userManager;
            _tokenManagerService = tokenManagerService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IdentityUser>> Post([FromBody] UserRegister user) 
        {
            // Check if the user already exists.
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if(existingUser != null) 
            {
                this.ModelState.AddModelError(string.Empty, "This user already exists.");
                return BadRequest();
            }

            IdentityUser newUser = new IdentityUser()
            {
                UserName = user.Username,
                Email = user.Email
            };
            
            var userCreationResponse = await _userManager.CreateAsync(newUser, user.Password);
            if (userCreationResponse.Succeeded) 
            { 
                return Created("/users/", "Success");
            }
            else 
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public async Task<ActionResult> Authenticate([FromForm] UserLoginModel userLoginModel)
        {
            if (!await IsUsernameAndPasswordValid(userLoginModel.Username, userLoginModel.Password)) 
            {
                return BadRequest();
            }

            string token = await _tokenManagerService.CreateToken(userLoginModel.Username);

            var tokenResponse = new
            {
                userName = userLoginModel.Username,
                accessToken = token
            };

            return Ok(tokenResponse);
        }

        /// <summary>
        /// Check to see if the User actually exists.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Returns true if the username and password are valid.</returns>
        private async Task<bool> IsUsernameAndPasswordValid(string username, string password) 
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _userManager.CheckPasswordAsync(user, password);        
        }
    }
}

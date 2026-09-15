using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO.Auth;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this._userManager = userManager;
            this._tokenRepository = tokenRepository;
        }

        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            // Creating identity user object
            var identityUser = new IdentityUser
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Username
            };

            // Saving identity user object to db
            var identityResult = await _userManager
                .CreateAsync(identityUser, registerRequest.Password);

            if (identityResult.Succeeded) {
                // Add roles to this User
                if (registerRequest.Roles != null && registerRequest.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequest.Roles);

                    if (identityResult.Succeeded) {
                        return Ok("User was registered! Please login.");
                    }
                }
            }

            return BadRequest("Something went wrong!");
        }

        // POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(loginRequest.Username);

            if (user != null) 
            {
                // Check password
                bool isCorrectPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

                if (isCorrectPassword)
                {
                    // Get roles from user
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        // Create token
                        var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());

                        return Ok(
                            new LoginResponse
                            {
                                JwtToken = jwtToken,
                            }
                        );
                    }

                }
            }

            
            return BadRequest("Username or password incorrect");
        }
    }
}

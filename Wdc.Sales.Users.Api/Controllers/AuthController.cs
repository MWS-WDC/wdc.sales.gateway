using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wdc.Sales.Users.Api.Abstracts;
using Wdc.Sales.Users.Api.Models;

namespace Wdc.Sales.Users.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService
    ) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost("register")]
        public async Task<ActionResult<AuthModel>> Register(RegisterInputModel input)
        {
            if (await _userManager.FindByEmailAsync(input.Email) is not null)
                return Problem("Email is already registered!", statusCode: 409);

            var user = new ApplicationUser
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                UserName = input.FirstName + " " + input.LastName,
                Email = input.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var token = await _tokenService.CreateToken(user);

            return Ok(new AuthModel()
            {
                Token = token,
                Message = "User registered successfully."
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthModel>> Login(LoginInputModel Input)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, false);
            if (!result.Succeeded) return Unauthorized();

            var token = await _tokenService.CreateToken(user);
            return Ok(new AuthModel()
            {
                Token = token,
            });
        }
    }
}
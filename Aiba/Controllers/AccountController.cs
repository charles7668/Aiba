using Aiba.Model;
using Aiba.Model.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Aiba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        private readonly ILogger<AccountController> _logger;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly UserManager<IdentityUser> _userManager;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginInfo loginInfo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(ModelState.ToString());
                return BadRequest(ModelState);
            }

            IdentityUser? user = await _userManager.FindByNameAsync(loginInfo.UserName);
            if (user == null)
            {
                _logger.LogError("User {UserName} not found", loginInfo.UserName);
                return BadRequest($"User {loginInfo.UserName} not found");
            }

            if (_signInManager.IsSignedIn(User))
            {
                return Ok();
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(user, loginInfo.Password, true, false);
            if (result.Succeeded)
            {
                _logger.LogInformation("user {User} login success", loginInfo.UserName);
                var test = _signInManager.IsSignedIn(User);
                return Ok();
            }

            string errorMessage;
            if (result.IsLockedOut)
            {
                errorMessage = $"User {loginInfo.UserName} is locked out";
                _logger.LogError("user {User} is locked out", loginInfo.UserName);
            }
            else if (result.RequiresTwoFactor)
            {
                errorMessage = $"User {loginInfo.UserName} requires two factor";
                _logger.LogError("user {User} requires two factor", loginInfo.UserName);
            }
            else if (result.IsNotAllowed)
            {
                errorMessage = $"User {loginInfo.UserName} is not allowed";
                _logger.LogError("user {User} is not allowed", loginInfo.UserName);
            }
            else
            {
                errorMessage = $"user {loginInfo.UserName} Invalid login attempt";
                _logger.LogError("user {User} invalid login attempt", loginInfo.UserName);
            }

            return Unauthorized(errorMessage);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterInfo registerInfo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(ModelState.ToString());
                return BadRequest(ModelState);
            }

            var user = new IdentityUser
            {
                UserName = registerInfo.UserName,
                Email = registerInfo.Email
            };
            if (_userManager.Users.Any(u => u.UserName == user.UserName))
            {
                _logger.LogError("User name {UserName} already exists", user.UserName);
                return BadRequest($"User name {registerInfo.UserName} already exists");
            }

            if (_userManager.Users.Any(u => u.Email == user.Email))
            {
                _logger.LogError("Email {Email} already exists", user.Email);
                return BadRequest($"Email {registerInfo.Email} already exists");
            }

            IdentityResult result = await _userManager.CreateAsync(user, registerInfo.Password);
            if (result.Succeeded)
            {
                if (await _roleManager.RoleExistsAsync(UserRole.USER))
                {
                    IdentityResult addRoleResult = await _userManager.AddToRoleAsync(user, UserRole.USER);
                    if (!addRoleResult.Succeeded)
                    {
                        _logger.LogError("Failed add {Role} role to user {User}", UserRole.USER, user.UserName);
                        return BadRequest("Register Failed");
                    }
                }
                else
                {
                    _logger.LogError("{Role} Role not found", UserRole.USER);
                    return BadRequest("Register Failed");
                }

                await _signInManager.SignInAsync(user, true);
                return Ok();
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("status")]
        [AllowAnonymous]
        public Task<IActionResult> Status()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return Task.FromResult<IActionResult>(Ok());
            }

            return Task.FromResult<IActionResult>(Unauthorized());
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
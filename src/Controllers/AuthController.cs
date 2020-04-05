using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Collections.Generic;
using SaveOnCloudApi.Models;
using SaveOnCloudApi.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.WebUtilities;
using SaveOnCloudApi.Models.Auth;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Configuration;

namespace SaveOnCloudApi.Controllers
{
    public class AuthController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
       
    public AuthController(UserManager<ApplicationUser> userManager,
                          RoleManager<IdentityRole> roleManager,
                          IOptions<JwtOptions> jwtOptions,
                          ILoggerFactory loggerFactory,
                          IEmailSender emailSender,
                          IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtOptions = jwtOptions.Value;
            _logger = loggerFactory.CreateLogger<AuthController>();
            _emailSender = emailSender;
            Configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("auth/login")]
        [Produces("application/json")]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Ensure the username and password is valid.
            var user = await _userManager.FindByNameAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return BadRequest(new
                {
                    error = "", //OpenIdConnectConstants.Errors.InvalidGrant,
                    error_description = "The username or password is invalid."
                });
            }

            // Ensure the email is confirmed.
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest(new
                {
                    error = "email_not_confirmed",
                    error_description = "You must have a confirmed email to log in."
                });
            }

            _logger.LogInformation($"User logged in (id: {user.Id})");

            var permissions = new List<string>();
            // Generate and issue a JWT token
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            //var roles = await _userManager.GetRolesAsync(user);
            //foreach (var roleName in roles)
            //{
            //    var role = await _roleManager.FindByNameAsync(roleName);
            //    if (role != null)
            //    {
            //        var roleClaims = await _roleManager.GetClaimsAsync(role);
            //        if (roleClaims != null && roleClaims.Any())
            //        {
            //            permissions.AddRange(roleClaims.Select(c => c.Value));
            //        }
            //    }
            //}

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              issuer: _jwtOptions.issuer,
              audience: _jwtOptions.issuer,
              claims: claims,
              expires: DateTime.Now.AddMinutes(540),
              signingCredentials: creds);

            return Ok(
                new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    permissions = permissions.Distinct(),
                });
        }

        [AllowAnonymous]
        [HttpPost("auth/register")]
        public async Task<IActionResult> RegisterAsync(RegistrationModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.First().Code);
            }

            _logger.LogInformation("User created a new account with password.");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = $"{Configuration["webAppBaseUrl"]}/Account/ConfirmEmail?userId={user.Id}&code={code}";

            _emailSender.SendEmail(
                model.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            _logger.LogInformation("Confrim account email was sent to the user.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return BadRequest("User with this email already registered! Need to confirm your email");
            }

            return Created("", user.Email);
        }

        [AllowAnonymous]
        [HttpPost("auth/confirm")]
        public async Task<IActionResult> ConfirmAsync(EmailConfirmModel model)
        {
            var user = new ApplicationUser { Id = model.UserId };
            var result = await _userManager.ConfirmEmailAsync(user, model.Token);

            _logger.LogInformation($"User email confirm request. Email: {model.UserId}. Status {result.Succeeded}");

            return result.Succeeded ?
                (IActionResult)Ok() :
                BadRequest(result.Errors.First().Description);
        }
    }
}

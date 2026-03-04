using System.Text;
using AuthService.Data;
using AuthService.DTOs;
using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace AuthService.Controllers
{
    /// <summary>
    /// This is the authentication section for user manangement.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtService jwtService,
            IConfiguration configuration,
            IEmailService emailService,
            ILogger<AuthController> logger,
            AppDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
                return BadRequest("Email already in use");

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                Phone = dto.Phone,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Bio = dto.Bio,
                Location = dto.Location,
                JoinedDate = DateTime.UtcNow,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,

            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");

            return Ok("Registration Successful");
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);


            var accessToken = _jwtService.GenerateAccessToken(user, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();


            refreshToken.UserId = user.Id;
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToKen = refreshToken.Token
            });
        }
        [HttpPost("create-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return BadRequest("Role already exists");
            await _roleManager.CreateAsync(new IdentityRole(roleName));

            return Ok("Role created successfully");
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(AssignRoleDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound("User not found");
            if (!await _roleManager.RoleExistsAsync(dto.Role))
                return BadRequest("Role does not exist");

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok("Role assigned successfully");
        }
        
        [HttpGet("get-roles/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found");

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles);
        }
        
        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                return Ok(new
                {
                    message = $"A reset link will been sent to the email {model.Email}"
                });
            }
            try
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodingToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var frontendBaseUrl = _configuration["Frontend:BaseUrl"] ?? "http://cameatwell.vercel.app"; //This is just a temporary link that doesn't exist yet
                var resetUrl = $"{frontendBaseUrl}/reset-password?email={Uri.EscapeDataString(user.Email!)}&token={encodingToken}";

                await _emailService.SendPasswordResetEmailAsync(user.Email!, resetUrl);

                return Ok(new
                {
                    message = $"We have sent a reset email to {model.Email}"
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to send reset email to {model.Email}");
                return Ok(new
                {
                    message = "If you have an account, we will send you a reset link"
                });
            }
        }
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogWarning($"Reset password attempt failed for the email \"{model.Email}\"");
                return BadRequest(new { message = "Invalid reset attempt" });
            }
            try
            {
                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
                var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);
                if(result.Succeeded)
                {
                    _logger.LogInformation($"Password reset successful for: {model.Email}");

                    await _userManager.UpdateSecurityStampAsync(user);
                    return Ok(new { message = "Password has been reset successfully" });
                }
                foreach (var error in result.Errors)
                {
                    if(error.Code == "InvalidToken")
                    {
                        _logger.LogWarning($"Invalid token used for password reset by: {model.Email}");
                        return BadRequest(new { message = "Invalid or expired reset token" });
                    }
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                _logger.LogWarning($"Password reset failed for {model.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");

                return BadRequest(new
                {
                    message = "Failed to reset password",
                    errors = result.Errors.Select(e => e.Description)
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error during password reset for: {model.Email}");
                return BadRequest(new { message = "An error occurred during password reset" });
            }
        }
    }
}

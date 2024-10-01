using BlackBox.Application.Common.Interfaces;
using BlackBox.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BlackBox.Domain.Exceptions;
using System.Data;
using BlackBox.Application.Common.Models;
using BlackBox.Application.Configurations;

namespace BlackBox.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AuthorizationConfig _authorizationConfig;
        private readonly IBlackBoxContext _context; 

        public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
             AuthorizationConfig authorizationConfig, IBlackBoxContext context,  RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager; 
            _authorizationConfig = authorizationConfig;
            _roleManager = roleManager;
            _context = context; 
        }
         
        public async Task<Result<bool>> SignInWithCodeAsync(string userName, string password)
        {

            _ = await GetUserRolesAsync(userName.Trim());

            var result = await _signInManager.PasswordSignInAsync(userName.Trim(), password, false, false);

            return result.Succeeded ? Result<bool>.Success(true) : Result<bool>.Failure(result.ToString(), false);
        }

        public async Task<List<string>> GetUserRolesAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName.Trim());
            if (user == null)
                throw new BusinessRuleException("Credentials", "Invalid login details");

            var roles = ((await _userManager.GetRolesAsync(user)) ?? new List<string>()).ToList();

            return roles ?? new();
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

    
        private string GenerateJwtToken(ApplicationUser user, List<string> roles, out DateTime expires)
        {
            expires = DateTime.UtcNow.AddMinutes(_authorizationConfig.ExpirationInMinutes);

            if (_authorizationConfig == null || user == null) return string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authorizationConfig.Secret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Issuer = _authorizationConfig.Issuer,
                Audience = _authorizationConfig.Issuer,
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<ApplicationUserDTO?> GetApplicationUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            return new ApplicationUserDTO
            {
                EUID = user.EUID,
                Email = user.Email!,
                UserId = user.Id 
            };
        }

        public async Task<ApplicationUserDTO?> GetApplicationUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            return new ApplicationUserDTO
            {
                EUID = user.EUID,
                Email = user.Email!,
                UserId = user.Id,
            };
        }
 
    }
}

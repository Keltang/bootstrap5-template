
using BlackBox.Application.Common.Models;
using BlackBox.Domain.Common;
using Microsoft.AspNetCore.Identity; 

namespace BlackBox.Application.Common.Interfaces
{
    public interface IIdentityService
    {
       Task<Result<bool>> SignInWithCodeAsync(string userName, string password);
        Task LogOutAsync(); 
        Task<ApplicationUserDTO?> GetApplicationUserAsync(string userId);
        Task<ApplicationUserDTO?> GetApplicationUserByEmailAsync(string email);
        Task<List<string>> GetUserRolesAsync(string userName); 
    }
}

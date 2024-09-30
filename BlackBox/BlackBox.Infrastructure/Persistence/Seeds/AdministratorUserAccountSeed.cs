using BlackBox.Application.Common.Interfaces;
using BlackBox.Domain.Enums;
using BlackBox.Domain.Models.Seeds; 

namespace BlackBox.Infrastructure.Persistence.Seeds
{
    public class AdministratorUserAccountSeed
    {
        public static async Task ExecuteAsync(IIdentityService identity)
        {
            try
            {                
                var adminUser = identity.GetApplicationUserAsync(AdministratorSeedData.UserId).Result;
                if (adminUser == null || string.IsNullOrEmpty(adminUser.UserId))
                {
                    identity.CreateUserAsync(AdministratorSeedData.UserId, AdministratorSeedData.Email, AdministratorSeedData.Name, string.Empty, Domain.Enums.AuthType.Sso, string.Empty).Wait();
                }
                var roles = new string[] { UserRole.Teacher };
                await identity.CreateRoleWithUserIfNotExistAsync(roles.ToList(), AdministratorSeedData.Email);
            }
            catch (Exception) { }
        }
    }
}

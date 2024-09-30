using System.Text;
using BlackBox.Application.Common.Interfaces;
using BlackBox.Application.Configurations;
using BlackBox.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BlackBox.Infrastructure
{
    public static class DependencyInjection
    {
        private const string ConnectionStringIdentifier = "DefaultConnection";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<BlackBoxContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(ConnectionStringIdentifier), 
                    b => b.MigrationsAssembly(typeof(BlackBoxContext).Assembly.FullName)));

            services.AddSingleton(new DbConnectionConfig(configuration.GetConnectionString(ConnectionStringIdentifier)!));
             
            services.AddScoped<IBlackBoxContext>(provider => provider.GetRequiredService<BlackBoxContext>());
             
            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<BlackBoxContext>();
             
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IBlackBoxReadOnlyContext, BlackBoxReadOnlyContext>(); 
              
            var authConfig = new AuthorizationConfig();
            configuration.GetSection(AuthorizationConfig.ConfigurationName).Bind(authConfig);
            
            var key = Encoding.ASCII.GetBytes(authConfig.Secret);

            //services.AddSingleton<AuthorizationConfig>(_ => authConfig);
            //services.AddSingleton<FrontendConfig>(_ => frontendConfig);
           

            //services.AddTransient<IClassRepository, ClassRepository>(); 

            services.AddAuthentication(config =>
                {
                    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddAuthorization(options => { });

            return services;
        }
    }
}

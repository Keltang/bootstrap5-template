using BlackBox.Application.Common.Interfaces;
using BlackBox.Application;
using BlackBox.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore.Internal;
using BlackBox.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
RegisterServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//await SeedDatabaseDataAsync(app);

app.Run();

//static async Task SeedDatabaseDataAsync(WebApplication app)
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var identity = scope.ServiceProvider.GetRequiredService<IIdentityService>();
//        await AdministratorUserAccountSeed.ExecuteAsync(identity);
//    }
//}

static void RegisterServices(WebApplicationBuilder builder)
{
    var services = builder.Services;

    services.AddApplication();
    services.AddInfrastructure(builder.Configuration);

    services.AddScoped<ICurrentUserService, CurrentUserService>();

    services.AddHttpContextAccessor();
 
    services.Configure<KestrelServerOptions>(options => options.Limits.MaxRequestBodySize = 100_000_000);
    services.Configure<FormOptions>(options =>
    {
        options.MultipartBodyLengthLimit = 100_000_000;
    });
}
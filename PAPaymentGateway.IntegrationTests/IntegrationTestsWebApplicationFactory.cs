using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PAPaymentGateway.Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PAPaymentGateway.IntegrationTests
{
    public class IntegrationTestsWebApplicationFactory<TStartup> : 
        WebApplicationFactory<TStartup> where TStartup : class 
    {        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>));
                
                
                //services.Remove(descriptor);

                //services.AddDbContext<ApplicationDbContext>(options =>
                //{
                //    options.UseInMemoryDatabase("InMemoryDbForTesting");
                //});


                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    //var logger = scopedServices
                    //    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Identity Services to assist in seeding required Users.
                    var userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();
                    var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                    try
                    {
                        db.Database.EnsureCreated();
                        DatabaseUtilities.Seed(db, userManager, roleManager);
                    }
                    catch (Exception ex)
                    {
                        //logger.LogError(ex, "An error occurred seeding the " +
                        //    "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }

        public async Task<string> GetAuthenticationToken(string username) 
        {
            using (var scope = Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var tokenManager = scopedServices.GetRequiredService<ITokenManagerService>();
                return await tokenManager.CreateToken(username);
            }
        }
    }
}

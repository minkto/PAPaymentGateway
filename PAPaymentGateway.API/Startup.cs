using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PAPaymentGateway.API.Services;
using PAPaymentGateway.Core.Interfaces;
using PAPaymentGateway.Core.Models.Identity;
using System.Text;

namespace PAPaymentGateway.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(setup => setup.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PA Payment Gateway",
                Version = "v1"
            })); 

            var identitySettingsSection = Configuration.GetSection("IdentitySettings");
            services.Configure<IdentitySettings>(identitySettingsSection);


            // JWT Authentication
            var identitySettings = identitySettingsSection.Get<IdentitySettings>();
            var key = Encoding.UTF8.GetBytes(identitySettings.SecretKey);

            services.AddAuthentication(au => {
                au.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                au.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                
            }).AddJwtBearer(jwt => { 
                jwt.RequireHttpsMetadata = false; // Off for testing purposes.
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // Off for testing purposes.
                    ValidateAudience = false                
                };
            
            }); 

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ProdDB")));
            services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IBankProcessingService, TestBankProcessingService>();
            services.AddTransient<ILoggingService, LoggingService>();
            services.AddTransient<IMerchantManager, MerchantService>();
            services.AddTransient<ITokenManagerService, TokenManagerService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "PA Payment Gateway v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

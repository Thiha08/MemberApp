using MemberApp.Data;
using MemberApp.Data.Abstract;
using MemberApp.Data.Infrastructure;
using MemberApp.Data.Infrastructure.Services;
using MemberApp.Data.Infrastructure.Services.Abstract;
using MemberApp.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace MemberApp.Web
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
            string connectionString = Configuration.GetConnectionString("MemberAppDatabase");

            services.AddDbContextPool<MemberAppContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), builder => builder.MigrationsAssembly("MemberApp.Web")));

            // Repositories
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IMemberRoleRepository, MemberRoleRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ILoggingRepository, LoggingRepository>();

            // Services
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IEncryptionService, EncryptionService>();

            services.AddAuthentication();

            // Polices
            services.AddAuthorization(options =>
            {
                // inline policies
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Admin");
                });
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            DbInitializer.Initialize(app.ApplicationServices);
        }
    }
}

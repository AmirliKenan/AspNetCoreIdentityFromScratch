using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreIdentity.CustomValidation;
using CoreIdentity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreIdentity
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));



           










            services.AddIdentity<AppUser, AppRole>(options=> 
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                
             
            }).AddPasswordValidator<CustomPasswordValidator>().AddUserValidator<CustomUserValidation>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>

            {

                // Cookie settings

                options.Cookie.HttpOnly = true;

                options.LoginPath = new PathString("/Home/Login");
                options.LogoutPath = new PathString("/Member/LogOut");

                options.SlidingExpiration = true;

                options.ExpireTimeSpan = TimeSpan.FromDays(60);

                options.Cookie.SameSite = SameSiteMode.Lax;

                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                options.Cookie.Name = "myblogCooke";
                options.AccessDeniedPath = new PathString("/Member/AccessDenied");

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
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default",
                    pattern:"{Controller=Home}/{action=Index}/{id?}"

                    );
            });
        }
    }
}

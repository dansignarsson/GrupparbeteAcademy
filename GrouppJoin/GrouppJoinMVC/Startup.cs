using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupJoinMVC.Models;
using GroupJoinMVC.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GrouppJoinMVC
{
    public class Startup
    {

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MyIdentityDbContext>(o => o.UseSqlServer(connString));
            services.AddDbContext<GroupJoinDBContext>(o => o.UseSqlServer(connString));

            services.AddIdentity<MyIdentityUser, IdentityRole>(o =>
            {
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
                o.Password.RequireUppercase = false;
                o.Password.RequireLowercase = false;
                
                
            })
                .AddEntityFrameworkStores<MyIdentityDbContext>()
                .AddDefaultTokenProviders();

            // Behövs bara om man vill använda en annan URL
            // till login-sidan än account/login:
            services.ConfigureApplicationCookie(
                o => o.LoginPath = "/LogIn");

            services.AddTransient<AccountService>();
            services.AddTransient<ProductService>();
            services.AddTransient<RatingService>();
            services.AddTransient<StoresService>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
            app.UseStaticFiles();

        }
    }
}

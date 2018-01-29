using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PizzeriaASP.Configuration;
using PizzeriaASP.Models;

namespace PizzeriaASP
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Dependency Injection
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddTransient<IIngredientRepository, EFIngredientRepository>();
            services.AddTransient<IIdentityRepository, EFIdentityRepository>();
            services.AddTransient<ICustomerRepository, EFCustomerRepository>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();

            services.AddMvc();

            // Same object for requests
            //services.AddScoped(SessionCart.GetCart);

            // Same object should always be used
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
                {
                    //opts.User.RequireUniqueEmail = true;
                    opts.Password.RequiredLength = 6;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<TomasosContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Sets upp the in-memory data store
            //services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            // Reggisters the services used to access session data
            services.AddSession();

            // Add Google sign-in
            services.AddAuthentication().AddGoogle(opts =>
            {
                opts.ClientId = "884616106260-8gh90apq8rglan0jksqt09v0gj9mofgm.apps.googleusercontent.com";
                opts.ClientSecret = "52VxjtQcMxEneKUI-xL3m5md";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }


            // Enable static files as css and js
            app.UseStaticFiles();

            // Allows the session system to auto associate requests with sessions when arriving from the client
            app.UseSession();

            app.UseAuthentication(); // app.UseIdentity() will be removed

            app.UseMvc(routes =>
            {
                //Improve routing for pagination
                //routes.MapRoute(
                //    name: null,
                //    template: "{category}/Page{productPage:int}",
                //    defaults: new { Controller = "Product", action = "List" });

                //routes.MapRoute(
                //    name: null,
                //    template: "Page{productPage:int}",
                //    defaults: new { Controller = "Product", action = "List", productPage = 1 });

                //routes.MapRoute(
                //    name: null,
                //    template: "{category}",
                //    defaults: new { Controller = "Product", action = "List", productPage = 1 });

                //routes.MapRoute(
                //    name: null,
                //    template: "",
                //    defaults: new { Controller = "Product", action = "List", productPage = 1 });

                //routes.MapRoute(
                //    name: null,
                //    template: "{controller}/{action}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Product}/{action=List}/{id?}");
            });



            // Populate with product/ingredient 
            SeedData.EnsurePopulated(app);

            new UserRoleSeed(app.ApplicationServices.GetService<RoleManager<IdentityRole>>()).Seed();

            // Ensure admin user
            IdentitySeed.EnsurePopulated(app);
        }
    }
}

// System
using System;

// H9
using H9ShoesShopApp.Models;
using H9ShoesShopApp.Models.Repository;
using H9ShoesShopApp.Repository;

// .NET Core
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace H9ShoesShopApp
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices( IServiceCollection services )
        {
            services.AddControllersWithViews();
            services.AddMvc( option =>
            {
                option.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                option.Filters.Add( new AuthorizeFilter( policy ) );
            } );
            services.AddDbContext<AppDbContext>( options =>
            options.UseSqlServer( Configuration.GetConnectionString( "H9ShoesDbString" ) ) );
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddDistributedMemoryCache();
            services.AddSession( cfg => {
                cfg.Cookie.Name = "Huyhieu";
                cfg.IdleTimeout = new TimeSpan( 0, 60, 0 );
            } );
        }

        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if ( env.IsDevelopment() )
            { 
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler( "/Home/Error" );
                app.UseHsts();
                app.UseExceptionHandler( "/Error" );
                app.UseStatusCodePagesWithRedirects( "/Error/{0}" );
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();
            
            app.UseEndpoints( endpoints =>
            {
                endpoints.MapControllerRoute( 
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}" );
            } );
        }
    }
}
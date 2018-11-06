using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelFlix.App.HttpClients;
using TelFlix.App.Infrastructure.Extensions;
using TelFlix.App.Infrastructure.Providers;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services;
using TelFlix.Services.Contracts;

namespace TelFlix.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            this.Environment = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.RegisterData(services);
            this.RegisterAuthentication(services);
            this.RegisterServices(services);
            this.RegisterInfrastructure(services);

            services.AddAuthorization(options => 
            {
                options.AddPolicy("RequireAdministratorRole", policy =>
                {
                    policy.RequireRole("Administrator");
                });
            });

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
        }

        private void RegisterData(IServiceCollection services)
        {
            services.AddDbContext<TFContext>(options =>
            {
                var connectionString = this.Configuration.GetConnectionString("DevLocalDb");
                options.UseSqlServer(connectionString);
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddHttpClient<TheMovieDbClient>();
            services.AddSingleton<IJsonProvider, JsonProvider>();

            services.AddScoped<UserManager<User>>();
            //services.AddScoped<RoleManager<IdentityRole>>();
            //services.AddScoped<SignInManager<User>>();

            services.AddTransient<IMovieServices, MovieServices>();
            services.AddTransient<IAddMovieService, AddMovieService>();
            services.AddTransient<IGenreServices, GenreServices>();
            services.AddTransient<IActorServices, ActorServices>();

            //services.AddScoped<IModifyMovieServices, ModifyMovieServices>();
            //services.AddScoped<ISeedDatabaseService, SeedDatabaseService>();
        }

        private void RegisterInfrastructure(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private void RegisterAuthentication(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<TFContext>()
                .AddDefaultTokenProviders();                ;

            if (this.Environment.IsDevelopment())
            {
                services.Configure<IdentityOptions>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 0;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
                    options.Lockout.MaxFailedAccessAttempts = 999;
                });
            }
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDatabaseMigration();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "identity",
                  template: "{area:exists}/Account/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

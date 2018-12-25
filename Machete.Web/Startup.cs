using System;
using System.Globalization;
using System.IO;
using Machete.Data;
using Machete.Service;
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Machete.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = Configuration.GetConnectionString("DefaultConnection");

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)//;
                .AddCookie(options =>
                    options.LoginPath = "/Account/Login"
                );

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddDbContext<MacheteContext>(builder => {
                if (connString == null || connString == "Data Source=machete.db")
                    builder.UseSqlite("Data Source=machete.db", with =>
                        with.MigrationsAssembly("Machete.Data"));
                else
                    builder.UseSqlServer(connString, with =>
                        with.MigrationsAssembly("Machete.Data"));
            });
            
            services.AddIdentity<MacheteUser, IdentityRole>()
                .AddEntityFrameworkStores<MacheteContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
//                options.Password.RequireDigit = true;
//                options.Password.RequiredLength = 8;
//                options.Password.RequireNonAlphanumeric = false;
//                options.Password.RequireUppercase = true;
//                options.Password.RequireLowercase = false;
//                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                // these paths are the default, declared explicitly
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            
            services.AddMvc(/*config => { config.Filters.Add(new AuthorizeFilter()); }*/)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<IEmailConfig, EmailConfig>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IWorkerSigninRepository, WorkerSigninRepository>();
            services.AddTransient<IWorkerRepository, WorkerRepository>();
            services.AddTransient<IWorkerRequestRepository, WorkerRequestRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IEmployerRepository, EmployerRepository>();
            services.AddTransient<IEmailRepository, EmailRepository>();
            services.AddTransient<IWorkOrderRepository, WorkOrderRepository>();
            services.AddTransient<IWorkAssignmentRepository, WorkAssignmentRepository>();
            services.AddTransient<ILookupRepository, LookupRepository>();
            services.AddTransient<IReportsRepository, ReportsRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IActivityRepository, ActivityRepository>();
            services.AddTransient<IConfigRepository, ConfigRepository>();
            services.AddTransient<IActivitySigninRepository, ActivitySigninRepository>();
            services.AddSingleton<ITransportProvidersRepository, TransportProvidersRepository>();
            services.AddSingleton<ITransportProvidersAvailabilityRepository, TransportProvidersAvailabilityRepository>();
            
            services.AddTransient<IConfigService, ConfigService>();
            services.AddTransient<ILookupService, LookupService>();
            services.AddTransient<IActivitySigninService, ActivitySigninService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IWorkerSigninService, WorkerSigninService>();
            services.AddTransient<IWorkerService, WorkerService>();
            services.AddTransient<IWorkerRequestService, WorkerRequestService>();
            services.AddTransient<IEmployerService, EmployerService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IWorkOrderService, WorkOrderService>();
            services.AddTransient<IWorkAssignmentService, WorkAssignmentService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IReportsV2Service, ReportsV2Service>();
            services.AddSingleton<ITransportProvidersService, TransportProvidersService>();
            services.AddSingleton<ITransportProvidersAvailabilityService, TransportProvidersAvailabilityService>();

            services.AddSingleton<IDefaults, Defaults>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("es"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });
            
            app.UseHttpsRedirection();

            app.UseStaticFiles(); // ?
            app.UseStaticFiles("/Content");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Content")),
                RequestPath = "/Content"
            });

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });            
            
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Content")),
                RequestPath = "/Content"
            });
            
            app.UseMvcWithDefaultRoute();
        }
    }
}

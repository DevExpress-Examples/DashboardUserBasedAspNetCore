using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace AspNetCore31Dashboard {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment) {
            Configuration = configuration;
            FileProvider = hostingEnvironment.ContentRootFileProvider;
        }

        public IConfiguration Configuration { get; }
        public IFileProvider FileProvider { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // Configures services to use the Web Dashboard Control.
            services
                .AddDevExpressControls()
                .AddControllersWithViews()
                .AddDefaultDashboardController((configurator, serviceProvider) => {
                    configurator.SetConnectionStringsProvider(serviceProvider.GetService<CustomConnectionStringProvider>());
                    configurator.SetDataSourceStorage(serviceProvider.GetService<CustomDataSourceStorage>());
                    configurator.SetDashboardStorage(serviceProvider.GetService<CustomDashboardStorage>());
                    configurator.SetDBSchemaProvider(serviceProvider.GetService<CustomDBSchemaProvider>());

                    configurator.VerifyClientTrustLevel += (s, e) => {
                        var contextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
                        var userName = contextAccessor.HttpContext.Session.GetString("CurrentUser");

                        if (string.IsNullOrEmpty(userName) || userName == "Guest")
                            e.ClientTrustLevel = ClientTrustLevel.Restricted;
                    };
                });
            
            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddTransient<CustomConnectionStringProvider>();
            services.AddTransient<CustomDataSourceStorage>();
            services.AddTransient<CustomDashboardStorage>();
            services.AddTransient<CustomDBSchemaProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // Registers the DevExpress middleware.
            app.UseDevExpressControls();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                // Maps the dashboard route.
                EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/dashboards");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

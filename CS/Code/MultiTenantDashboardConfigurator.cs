using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AspNetCore31Dashboard {
    public class MultiTenantDashboardConfigurator : DashboardConfigurator {
        private readonly IHttpContextAccessor contextAccessor;
        
        public MultiTenantDashboardConfigurator(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor) {
            this.contextAccessor = contextAccessor;
            
            SetConnectionStringsProvider(new CustomConnectionStringProvider(contextAccessor));
            SetDataSourceStorage(new CustomDataSourceStorage(contextAccessor));
            SetDashboardStorage(new CustomDashboardStorage(hostingEnvironment, contextAccessor));
            SetDBSchemaProvider(new CustomDBSchemaProvider(contextAccessor));

            VerifyClientTrustLevel += MultiTenantDashboardConfigurator_VerifyClientTrustLevel;
        }

        private void MultiTenantDashboardConfigurator_VerifyClientTrustLevel(object sender, VerifyClientTrustLevelEventArgs e) {
            var userName = contextAccessor.HttpContext.Session.GetString("CurrentUser");

            if (string.IsNullOrEmpty(userName) || userName == "Guest")
                e.ClientTrustLevel = ClientTrustLevel.Restricted;
        }
    }
}
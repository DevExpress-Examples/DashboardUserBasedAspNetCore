using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public class CustomDashboardStorage : IEditableDashboardStorage {
    private readonly IHttpContextAccessor сontextAccessor;
    private string dashboardStorageFolder;

    public CustomDashboardStorage(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor) {
        this.сontextAccessor = contextAccessor;
        this.dashboardStorageFolder = hostingEnvironment.ContentRootFileProvider.GetFileInfo("App_Data/Dashboards").PhysicalPath;
    }

    public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo() {
        var dashboardInfos = new List<DashboardInfo>();

        var files = Directory.GetFiles(dashboardStorageFolder, "*.xml");

        foreach (var item in files) {
            var name = Path.GetFileNameWithoutExtension(item);
            var userName = сontextAccessor.HttpContext.Session.GetString("CurrentUser");

            if (!string.IsNullOrEmpty(userName) && name.EndsWith(userName, System.StringComparison.InvariantCultureIgnoreCase))
                dashboardInfos.Add(new DashboardInfo() { ID = name, Name = name });
        }

        return dashboardInfos;
    }

    public XDocument LoadDashboard(string dashboardID) {
        if (GetAvailableDashboardsInfo().Any(di => di.Name == dashboardID)) {
            var path = Path.Combine(dashboardStorageFolder, dashboardID + ".xml");
            var content = File.ReadAllText(path);
            return XDocument.Parse(content);
        }
        else {
            throw new System.ApplicationException("You are not authorized to view this dashboard.");
        }
    }

    public string AddDashboard(XDocument dashboard, string dashboardName) {
        var userName = сontextAccessor.HttpContext.Session.GetString("CurrentUser");

        if (string.IsNullOrEmpty(userName) || userName != "Admin")
            throw new System.ApplicationException("You are not authorized to add dashboards.");

        var path = Path.Combine(dashboardStorageFolder, dashboardName + "_" + userName + ".xml");

        File.WriteAllText(path, dashboard.ToString());

        return Path.GetFileNameWithoutExtension(path);
    }

    public void SaveDashboard(string dashboardID, XDocument dashboard) {
        var userName = сontextAccessor.HttpContext.Session.GetString("CurrentUser");

        if (string.IsNullOrEmpty(userName) || userName != "Admin")
            throw new System.ApplicationException("You are not authorized to save dashboards.");

        var path = Path.Combine(dashboardStorageFolder, dashboardID + ".xml");

        File.WriteAllText(path, dashboard.ToString());
    }
}
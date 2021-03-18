using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Web;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

public class CustomConnectionStringProvider : IDataSourceWizardConnectionStringsProvider {
    private readonly IHttpContextAccessor сontextAccessor;
    private Dictionary<string, string> connectionStrings = new Dictionary<string, string>();

    public CustomConnectionStringProvider(IHttpContextAccessor contextAccessor) {
        this.сontextAccessor = contextAccessor;
        connectionStrings.Add("NorthwindConnectionString", @"XpoProvider=SQLite; Data Source=App_Data/nwind.db;");
        connectionStrings.Add("CarsXtraSchedulingConnectionString", @"XpoProvider=SQLite;Data Source=App_Data/CarsDB.db;");
    }

    public Dictionary<string, string> GetConnectionDescriptions() {
        var connections = new Dictionary<string, string>();
        var userName = сontextAccessor.HttpContext.Session.GetString("CurrentUser");

        if (userName == "Admin") {
            connections.Add("NorthwindConnectionString", "Northwind Connection");
            connections.Add("CarsXtraSchedulingConnectionString", "CarsXtraScheduling Connection");
        }
        else if (userName == "User") {
            connections.Add("CarsXtraSchedulingConnectionString", "CarsXtraScheduling Connection");
        }

        return connections;
    }

    public DataConnectionParametersBase GetDataConnectionParameters(string name) {
        if (GetConnectionDescriptions().ContainsKey(name)) {
            return new CustomStringConnectionParameters(connectionStrings[name]);
        }
        else {
            throw new System.ApplicationException("You are not authorized to use this connection.");
        }
    }
}
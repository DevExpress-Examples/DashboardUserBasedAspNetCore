# ASP.NET Core Dashboard - How to implement multi-tenant Dashboard architecture

This example shows how to configure the Dashboard control so that it works in the multi-user environment. 

You can identify a user in the current session and return the following user-specific content:

### Dashboards

Custom dashboard storage allows you to specify which dashboards the user can access, edit, and save. 

**API**: [IEditableDashboardStorage Interface](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.IEditableDashboardStorage) 

**Files to look at**: [CustomDashboardStorage.cs](./CS/Code/CustomDashboardStorage.cs)

### Data Sources

Custom data source storage allows you to specify which data sources are available to the user. 

**API**: [IDataSourceStorage Interface](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.IDataSourceStorage) 

**Files to look at**: [CustomDataSourceStorage.cs](./CS/Code/CustomDataSourceStorage.cs)

### Data Source Schema

A custom data source schema provider allows you to filter the data source for different users to show only a part of the data source.

**API**: [DBSchemaProviderEx Class](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Sql.DBSchemaProviderEx)

**Files to look at**: [CustomDBSchemaProvider.cs](./CS/Code/CustomDBSchemaProvider.cs)

### Connection Strings

A custom connection string provider allows you to specify connection strings depending on the user's access rights.

**API**: [IDataSourceWizardConnectionStringsProvider Interface](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Web.IDataSourceWizardConnectionStringsProvider) 

**Files to look at**: [CustomConnectionStringProvider.cs](./CS/Code/CustomConnectionStringProvider.cs)


### Working Mode

The Web Dashboard control can operate in `ViewerOnly` mode for unauthorized users. To do this, handle the [DashboardConfigurator.VerifyClientTrustLevel](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.DashboardConfigurator.VerifyClientTrustLevel) event and set the `e.ClientTrustLevel` property to `Restricted`. This setting prevents inadvertent or unauthorized modifications of dashboards stored on a server. You can find more information in the following help section: [Security Considerations - Designer and Viewer Modes](https://docs.devexpress.com/Dashboard/118651/web-dashboard/general-information/security-considerations#designer-and-viewer-modes).

**API**: [DashboardConfigurator.VerifyClientTrustLevel Event](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.DashboardConfigurator.VerifyClientTrustLevel)

**Files to look at**: [Dashboard.cshtml](./CS/Views/Home/Dashboard.cshtml) and [Startup.cs](./CS/Startup.cs)


## Example Structure

You can limit access to sensitive information depending on the current user's ID. Every custom store/provider reads the `IHttpContextAccessor.HttpContext.Session.GetString("CurrentUser")` value from session state. We use the standard [IHttpContextAccessor](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-3.0) with dependency injection to access the HTTP context is custom storages/providers.

When the application starts, you see the [Index](./CS/Views/Home/Index.cshtml) view with a ComboBox in which you can select a user. When you click the **Sign in** button, the ID of the selected user is passed to the `CurrentUser` variable in Session and you are redirected to the [Dashboard](./CS/Views/Home/Dashboard.cshtml) view. In this view, the Web Dashboard control displays the features available to the selected user. Below is a table that illustrates the user IDs and their associated rights in this example:

| Role  | Dashboard Storage | DataSource Storage | ConnectionString Provider | DBSchema Provider | Working Mode | Create/Edit |
| --- | --- | --- | --- | --- | --- | --- |
| Admin | dashboard1_admin, dashboard2_admin | SqlDataSource, JsonDataSource | Northwind, CarsXtraScheduling | All (Categories, Products, Cars,...) | Designer, Viewer | Yes |
| User | dashboard1_user | SqlDataSource | CarsXtraScheduling | Cars | Designer, Viewer | No |
| Guest | dashboard1_guest | - | - | - | ViewerOnly | - |
| Unauthorized| - | - | - | - | ViewerOnly | - |

## Documentation

- [Manage Multi-Tenancy](https://docs.devexpress.com/Dashboard/402924/web-dashboard/dashboard-backend/manage-multi-tenancy)

## More Examples

- [ASP.NET Core Dashboard - How to load different data based on the current user](https://github.com/DevExpress-Examples/DashboardDifferentUserDataAspNetCore)
- [ASP.NET Core Dashboard - How to implement authentication](https://github.com/DevExpress-Examples/ASPNET-Core-Dashboard-Authentication)
- [ASP.NET MVC Dashboard - How to implement multi-tenant Dashboard architecture](https://github.com/DevExpress-Examples/DashboardUserBasedMVC)
- [ASP.NET MVC Dashboard - How to load and save dashboards from/to a database](https://github.com/DevExpress-Examples/mvc-dashboard-how-to-load-and-save-dashboards-from-to-a-database-t400693)
- [ASP.NET MVC Dashboard - How to load different data based on the current user](https://github.com/DevExpress-Examples/DashboardDifferentUserDataMVC)

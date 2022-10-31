<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/349063210/21.2.2%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T983227)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# Dashboard for ASP.NET Core - How to implement multi-tenant Dashboard architecture

This example shows how to configure the Dashboard control so that it works in the multi-user environment. 

You can identify a user in the current session and return the following user-specific content:

### Dashboards

Custom dashboard storage allows you to specify which dashboards the user can access, edit, and save. 

**API**: [IEditableDashboardStorage Interface](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.IEditableDashboardStorage) 

**Files to review**: [CustomDashboardStorage.cs](./CS/Code/CustomDashboardStorage.cs)

### Data Sources

Custom data source storage allows you to specify which data sources are available to the user. 

**API**: [IDataSourceStorage Interface](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.IDataSourceStorage) 

**Files to review**: [CustomDataSourceStorage.cs](./CS/Code/CustomDataSourceStorage.cs)

### Data Source Schema

A custom data source schema provider allows you to filter the data source for different users to show only a part of the data source.

**API**: [DBSchemaProviderEx Class](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Sql.DBSchemaProviderEx)

**Files to review**: [CustomDBSchemaProvider.cs](./CS/Code/CustomDBSchemaProvider.cs)

### Connection Strings

A custom connection string provider allows you to specify connection strings depending on the user's access rights.

**API**: [IDataSourceWizardConnectionStringsProvider Interface](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Web.IDataSourceWizardConnectionStringsProvider) 

**Files to review**: [CustomConnectionStringProvider.cs](./CS/Code/CustomConnectionStringProvider.cs)


### Working Mode

The Web Dashboard control can operate in `ViewerOnly` mode for unauthorized users. To do this, handle the [DashboardConfigurator.VerifyClientTrustLevel](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.DashboardConfigurator.VerifyClientTrustLevel) event and set the `e.ClientTrustLevel` property to `Restricted`. This setting prevents inadvertent or unauthorized modifications of dashboards stored on a server. You can find more information in the following help section: [Security Considerations - Working Mode Access Rights](https://docs.devexpress.com/Dashboard/118651/web-dashboard/general-information/security-considerations#working-mode-access-rights).

**API**: [DashboardConfigurator.VerifyClientTrustLevel Event](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.DashboardConfigurator.VerifyClientTrustLevel)

**Files to review**: [Dashboard.cshtml](./CS/Views/Home/Dashboard.cshtml) and [Startup.cs](./CS/Startup.cs)


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

- [Dashboard for ASP.NET Core - How to load different data based on the current user](https://github.com/DevExpress-Examples/DashboardDifferentUserDataAspNetCore)
- [Dashboard for ASP.NET Core - How to implement authentication](https://github.com/DevExpress-Examples/ASPNET-Core-Dashboard-Authentication)
- [Dashboard for MVC - How to implement multi-tenant Dashboard architecture](https://github.com/DevExpress-Examples/DashboardUserBasedMVC)
- [Dashboardfor MVC - How to load and save dashboards from/to a database](https://github.com/DevExpress-Examples/mvc-dashboard-how-to-load-and-save-dashboards-from-to-a-database-t400693)
- [Dashboardfor MVC - How to load different data based on the current user](https://github.com/DevExpress-Examples/DashboardDifferentUserDataMVC)

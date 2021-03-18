using DevExpress.DataAccess.Sql;
using DevExpress.Xpo.DB;
using Microsoft.AspNetCore.Http;
using System.Linq;

public class CustomDBSchemaProvider : DBSchemaProviderEx {
    private readonly IHttpContextAccessor сontextAccessor;

    public CustomDBSchemaProvider(IHttpContextAccessor contextAccessor) : base() {
        this.сontextAccessor = contextAccessor;
    }
    
    public override DBTable[] GetTables(SqlDataConnection connection, params string[] tableList) {
        var result = base.GetTables(connection, tableList);

        var userName = this.сontextAccessor.HttpContext.Session.GetString("CurrentUser");

        if (userName == "Admin") {
            return result;
        }
        else if (userName == "User") {
            return result.Where(t => t.Name == "Cars").ToArray();
        }
        else {
            return new DBTable[0];
        }
    }
}
using Azure.Identity;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async () =>
{
    // Podmień na swój serwer i bazę
    var connString = "Server=tcp:umbraco123.database.windows.net,1433;Database=umbracoDb123;";

    var credential = new DefaultAzureCredential();
    var token = credential.GetToken(
        new Azure.Core.TokenRequestContext(
            new[] { "https://database.windows.net/.default" }
        )
    ).Token;

    using var conn = new SqlConnection(connString);
    conn.AccessToken = token;
    await conn.OpenAsync();

    using var cmd = new SqlCommand("SELECT DB_NAME()", conn);
    var dbName = await cmd.ExecuteScalarAsync();

    return $"Połączono z bazą: {dbName}";
    
    //return $"test ";
});

app.Run();

var builder = DistributedApplication.CreateBuilder(args);

// Add SQL Server
var sqlPassword = builder.AddParameter("sql-password", secret: true);
var sqlServer = builder.AddSqlServer("sql", sqlPassword, port: 1433)
    .WithDataVolume();

var database = sqlServer.AddDatabase("zionauditing");

// Add API project with SQL Server reference
var api = builder.AddProject<Projects.ZionAuditing_Api>("api")
    .WithReference(database)
    .WithExternalHttpEndpoints();

builder.Build().Run();

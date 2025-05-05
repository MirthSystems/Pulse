using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("mirthsystems-pulse-cache")
    .WithDataBindMount(
        source: @"..\..\resources\cache",
        isReadOnly: false
    )
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRedisInsight();

var dbMigrations = builder.AddProject<Projects.MirthSystems_Pulse_Services_DatabaseMigrations>("mirthsystems-pulse-migrations");

var apiService = builder.AddProject<Projects.MirthSystems_Pulse_Services_API>("mirthsystems-pulse-api")
    .WaitFor(dbMigrations)
    .WithReference(cache)
    .WaitFor(cache);

builder.AddNpmApp("client", "../MirthSystems.Pulse.Client", "dev")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();

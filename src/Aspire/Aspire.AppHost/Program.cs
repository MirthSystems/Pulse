using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Pulse_API>("apiservice");

builder.AddProject<Projects.Pulse_Web>("webfrontend")
    .WithReference(apiService);

builder.Build().Run();

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("mirthsystems")
                      .WithDataBindMount(source: "../../../Data/PostgreSQL", isReadOnly: false)
                      .WithPgAdmin()
                      .WithPgWeb();

var postgresdb = postgres.AddDatabase("pulse_db");

builder.Build().Run();

using GrpcServer;

await Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder
        .UseStartup<Startup>())
    .Build()
    .RunAsync();
using Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);

var stateStore = builder.AddDaprStateStore("statestore");
var pubSub = builder.AddDaprPubSub("pubsub");

var storage = builder.AddAzureStorage("storage");

if (!builder.ExecutionContext.IsPublishMode)
{
    storage.RunAsEmulator();
}

var tables = storage.AddTables("tables");

builder.AddProject<Projects.AspireDaprDemo_AliceService>("alice")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        // Loads the resiliency policy for service invocation from alice to bob
        ResourcesPaths = [Path.Combine("..", "resources")],
    })
    .WithReference(stateStore);

builder.AddProject<Projects.AspireDaprDemo_BobService>("bob")
    .WithDaprSidecar()
    .WithReference(pubSub);

builder.AddProject<Projects.AspireDaprDemo_BrookService>("brook")
    .WithDaprSidecar()
    .WithReference(pubSub)
    .WithReference(tables)
    .WaitFor(tables);

builder.AddProject<Projects.AspireDaprDemo_Frontend>("aspiredaprdemo-frontend")
    .WithDaprSidecar()
    .WithReference(tables)
    .WithExternalHttpEndpoints()
    .WaitFor(tables);

builder.Build().Run();

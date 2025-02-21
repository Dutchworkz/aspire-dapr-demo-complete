using AspireDaprDemo.BrookService.Apis;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddAzureTableClient("tables");

var app = builder.Build();

app.UseCloudEvents();
app.MapSubscribeHandler();
app.MapSwaggerEndpoints();

app.MapEndpoints();

app.Run();


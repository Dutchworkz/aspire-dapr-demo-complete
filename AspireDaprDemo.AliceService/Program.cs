using AspireDaprDemo.AliceService.Apis;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var app = builder.Build();

app.MapSwaggerEndpoints();
app.MapEndpoints();

app.MapDefaultEndpoints();

app.Run();

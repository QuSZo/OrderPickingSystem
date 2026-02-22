using Api.Mqtt;
using Api.Policies;
using Api.RobotService;
using Api.WebSockets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddWebSocketServices();
builder.Services.AddMqtt();
builder.Services.AddControllers();
builder.Services.AddCorsPolicy();

builder.Services.AddRobotService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseWebSockets();
app.UseCors();

app.MapControllers();
app.MapHubs();

app.Run();
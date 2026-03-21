using Api.Mqtt;
using Api.Orders;
using Api.Policies;
using Api.Products;
using Api.PythonNet;
using Api.RobotService;
using Api.Services;
using Api.TravelingSalesmanAlgorithms;
using Api.WebSockets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddWebSocketServices();
builder.Services.AddMqtt();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter(
                System.Text.Json.JsonNamingPolicy.CamelCase
            )
        );
    });
builder.Services.AddCorsPolicy();

builder.Services.AddRobotService();
builder.Services.AddProducts();
builder.Services.AddOrders();
builder.Services.AddTspAlgorithm();
builder.Services.AddStatisticsService();

builder.Services.AddHostedService<InitializePythonEngine>();

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
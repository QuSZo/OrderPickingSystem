namespace Api.Mqtt;

public static class Extensions
{
    public static void AddMqtt(this IServiceCollection services)
    {
        services.AddHostedService<MqttConsumer>();
    }
}
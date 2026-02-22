using MQTTnet;

namespace Api.Mqtt;

public static class Extensions
{
    public static void AddMqtt(this IServiceCollection services)
    {
        services.AddSingleton<MqttClientFactory>();
        services.AddSingleton<MqttConfiguration>();
        services.AddSingleton<MqttConnection>();
        services.AddSingleton<MqttConsumer>();
        services.AddSingleton<MqttProducer>();
    }
}
// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text;

public class MQTTSubscriber
{
    public static async Task Main(String[] args)
    {
        var mqttFactory = new MqttFactory();
        IMqttClient client = mqttFactory.CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
            .WithClientId(Guid.NewGuid().ToString())
            .WithTcpServer("test.mosquitto.org", 1883)
            .WithCleanSession()
            .Build();

        client.UseConnectedHandler(async e =>
        {
            Console.WriteLine("Connnected to broker successfullly");
            var topicFilter = new TopicFilterBuilder()
                                     .WithTopic("MyTopic")
                                    .Build();

            await client.SubscribeAsync(topicFilter);
        });


        client.UseDisconnectedHandler(e =>
        {
            Console.WriteLine("Disconnnected to broker successfullly");
        });

        client.UseApplicationMessageReceivedHandler(e =>
        {
            Console.WriteLine($"Received message {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
        });

        await client.ConnectAsync(options);

        Console.WriteLine("Press key to exit...");
        Console.ReadLine();
        await client.DisconnectAsync();
       
    }
}
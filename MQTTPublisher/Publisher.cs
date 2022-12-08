// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;

namespace MQTTPublisher {
    class Publisher{
        public static async Task Main(String[] args)
        {
            var mqttFactory = new MqttFactory();
            IMqttClient client = mqttFactory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("test.mosquitto.org",1883)
                .WithCleanSession()
                .Build();

            client.UseConnectedHandler(e =>
            {
                Console.WriteLine("Connnected to broker successfullly");
            });


            client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Disconnnected to broker successfullly");
            });

            await client.ConnectAsync(options);

            Console.WriteLine("Pree any key to publish a message...");
            Console.ReadLine();

            await publishMessageAsyncClient(client);

            // as soon as message is published, disconnect the client

            await client.DisconnectAsync();
           
        }

        private static async Task publishMessageAsyncClient(IMqttClient client)
        {
            string messagePayload = "I said Helllo";
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("MyTopic")           //Name of topic where message is published
                .WithPayload(messagePayload)
                .WithQualityOfServiceLevel(0)   //Qos 0 1 2 3
                .Build();

            if (client.IsConnected) {
                await client.PublishAsync(message);
            }

        }
    }
}



using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;

class LogEmitter
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "direct_logs",
                                    type: "direct");

            string[] severityType = {"info", "warning", "error", "critical"};
            Random rnd = new Random();
            int sIndex = rnd.Next(severityType.Length);

            var severity = severityType[sIndex];
            var message =  "Here is a "+severityType[sIndex]+" message!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "direct_logs",
                                 routingKey: severity,
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine(" [x] Sent '{0}':'{1}'", severity, message);
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}
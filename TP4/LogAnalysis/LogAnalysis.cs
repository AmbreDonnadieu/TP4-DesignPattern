using System;
using System.IO;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class LogAnalysis
{

    private static int NbInfo, NbError, NbWarning, NbCritical = 0;

    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "direct_logs",
                                    type: "direct");
            var queueName = channel.QueueDeclare().QueueName;

            string[] severities = { "error", "info", "warning", "critical" };
            foreach (var severity in severities)
            {
                channel.QueueBind(queue: queueName,
                                  exchange: "direct_logs",
                                  routingKey: severity);
            }

            Console.WriteLine(" [*] Waiting for messages.");

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(10);

            var timer = new System.Threading.Timer((e) =>
            {
                Console.WriteLine();
                Console.WriteLine("Info : {0} infos", NbInfo);
                Console.WriteLine("Warning : {0} warnings", NbWarning);
                Console.WriteLine("Error : {0} errors", NbError);
                Console.WriteLine("Critical : {0} criticals", NbCritical);
                Console.WriteLine();
            }, null, startTimeSpan, periodTimeSpan);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                var routingKey = ea.RoutingKey;
                switch (routingKey)
                {
                    case "error":
                        NbError++;
                        break;

                    case "info":
                        NbInfo++;
                        break;

                    case "warning":
                        NbWarning++;
                        break;

                    case "critical":
                        NbCritical++;
                        break;

                }
                //Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);
                using (StreamWriter w = File.AppendText("AllLogs.log"))
                {
                    Log(routingKey, message, w);
                }

            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }

    public static void Log(string level, string msg, TextWriter w)
    {
        w.WriteLine("[" + DateTime.Now + "]"
                        + " [" + level + "]"
                        + " : [" + msg + "]");
    }

    // Console.WriteLine("Info : '{0}' infos", NbInfo);
    // Console.WriteLine("Warning : '{0}' warnings", NbWarning);
    // Console.WriteLine("Error : '{0}' errors", NbError);
    // Console.WriteLine("Critical : '{0}' criticals", NbCritical);

}
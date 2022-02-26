// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("#####  Beşiktaş Skorbord  ######");

var factory = new ConnectionFactory() { HostName = "localhost" };
using (IConnection connection = factory.CreateConnection())
using (IModel channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "Besiktas",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Goal? goal = JsonConvert.DeserializeObject<Goal>(message);
        Console.WriteLine($"GOL ATAN {goal?.PlayerName}-{goal?.AssistName}");
        Console.WriteLine("Gücüne Güç Katmaya geldik formanda ter olmaya geldik...");
        Console.WriteLine("........................................................");
    };


    channel.BasicConsume(queue: "Besiktas", autoAck: true, consumer: consumer);


    Console.ReadLine();
}
public record Goal(string PlayerName, string AssistName, DateTime GoalDate);
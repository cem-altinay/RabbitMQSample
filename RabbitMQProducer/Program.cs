// See https://aka.ms/new-console-template for more information


using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("BEŞİKTAŞ gol atıyor.........");

STARTPOINT:

Console.Write("Asist Yapan Oyuncu: ");
var asssitPlayer = Console.ReadLine();

Console.Write("Gol Atan Oyuncu: ");
var goalPlayer = Console.ReadLine();

var goal1 = new Goal(goalPlayer??"Bilinmiyor", asssitPlayer??"Bilinmiyor", DateTime.UtcNow);

var factory = new ConnectionFactory() { HostName = "localhost" };
using (IConnection connection = factory.CreateConnection())
using (IModel channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "Besiktas",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    string message = JsonConvert.SerializeObject(goal1);
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "",
                         routingKey: "Besiktas",
                         basicProperties: null,
                         body: body);
    Console.WriteLine($"Gole katkı sağlayanlar: {goal1.PlayerName}-{goal1.AssistName}");
}

Console.WriteLine("Top Kaleye Gönderildi...");
Console.WriteLine("--------------------------------------------------------------------");
goto STARTPOINT;



public record Goal(string PlayerName, string AssistName, DateTime GoalDate);
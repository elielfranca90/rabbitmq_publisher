using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace rabbitMQ.publisher
{
    /// <summary>
    /// Classe representando uma conexão para vários canais
    /// </summary>
    public class MultiChannel
    {

        public void Publish(Product product, AcessHistory acessHistory)
        {
            RMQConnection rMQConnection = new RMQConnection();

            try
            {
                var factory = rMQConnection.RMQ_NewConnection();

                using (var connection = factory.CreateConnection())
                {
                    var queueName = "saneamento";

                    var prodClient_Channel = CreateChannel(connection);
                    var acessHist_Channel = CreateChannel(connection);

                    BuildPublishers(prodClient_Channel, queueName, "prodClient", product, null);
                    BuildPublishers(acessHist_Channel, queueName, "acessHistory", null, acessHistory);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} | {ex.StackTrace}");
            }

        }

        public static IModel CreateChannel(IConnection connection)
        {
            var channel = connection.CreateModel();

            channel.ConfirmSelect();

            //channel.BasicAcks += Confirmation_Event; //Reconhece uma ou mais mensagens
            //channel.BasicNacks += NonConfirmation_Event; //Rejeita uma ou mais mensagens

            return channel;
        }

        public static void BuildPublishers(IModel channel, string queueName, string publisherName, 
            Product product, 
            AcessHistory acessHistory)
        {
            try
            {
                Task.Run(() =>
                    {
                        channel.QueueDeclare(queueName, true, false, false, null);

                        switch(publisherName)
                        {
                            case "prodClient":
                                var jsonA = Newtonsoft.Json.JsonConvert.SerializeObject(product);
                                var bodyA = Encoding.UTF8.GetBytes(jsonA);
                                channel.BasicPublish("", queueName, null, bodyA);
                                break;

                            case "acessHistory":
                                var jsonB = Newtonsoft.Json.JsonConvert.SerializeObject(acessHistory);
                                var bodyB = Encoding.UTF8.GetBytes(jsonB);
                                channel.BasicPublish("", queueName, null, bodyB);
                                break;
                        } 
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        //private static void NonConfirmation_Event(object sender, BasicNackEventArgs e)
        //{
        //    Console.WriteLine("Evento não confirmado(Nack)...");
        //}

        //private static void Confirmation_Event(object sender, BasicAckEventArgs e)
        //{
        //    Console.WriteLine("Evento confirmado(Ack)...");
        //}
    }
}

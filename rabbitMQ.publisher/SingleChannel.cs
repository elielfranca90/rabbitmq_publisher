using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace rabbitMQ.publisher
{
    public class SingleChannel
    {

        /// <summary>
        /// Faz a publicação das mensagens para o rbMQ
        /// </summary>
        public void Publish_ProdClient(Product product)
        {
            RMQConnection rMQConnection = new RMQConnection();

            var factory = rMQConnection.RMQ_NewConnection();

            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel()) //Cria um modelo e um canal novo para o envio
                {
                    channel.ConfirmSelect(); //Habilita o reconhecimento publisher

                    channel.BasicAcks += Confirmation_Event; //Reconhece uma ou mais mensagens
                    channel.BasicNacks += NonConfirmation_Event; //Rejeita uma ou mais mensagens

                    channel.QueueDeclare("prodClient", true, false, false, null);

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(product);

                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "",
                        routingKey: "prodClient",
                        basicProperties: null,
                        body: body);

                    Console.WriteLine("Mensagem enviada para o rbMQ...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
            }
        }

        public void Publish_AcessHistory(AcessHistory acessHistory)
        {

            try
            {
                RMQConnection rMQConnection = new RMQConnection();

                var factory = rMQConnection.RMQ_NewConnection();

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel()) //Cria um modelo e um canal novo para o envio
                {
                    channel.ConfirmSelect(); //Habilita o reconhecimento publisher
                    channel.BasicAcks += Confirmation_Event; //Reconhece uma ou mais mensagens
                    channel.BasicNacks += NonConfirmation_Event; //Rejeita uma ou mais mensagens

                    // Create an exchange
                    channel.ExchangeDeclare("my-exchange", ExchangeType.Direct, true, false);

                    // Create an queue
                    channel.QueueDeclare("histAcess", true, false, false);

                    // Bind the exchange to the queue
                    channel.QueueBind("histAcess", "my-exchange", "histAcess");

                    // declare message
                    var jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(acessHistory);
                    var body = Encoding.UTF8.GetBytes(jsonMessage);

                    channel.BasicPublish(exchange: "my-exchange",
                                         routingKey: "histAcess",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine("Mensagem enviada para o rbMQ...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }

        private void NonConfirmation_Event(object sender, BasicNackEventArgs e)
        {
            Console.WriteLine("Evento não confirmado(Nack)...");
        }

        private void Confirmation_Event(object sender, BasicAckEventArgs e)
        {
            Console.WriteLine("Evento confirmado(Ack)...");
        }
    }
}

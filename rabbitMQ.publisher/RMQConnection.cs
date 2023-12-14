using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rabbitMQ.publisher
{
    public class RMQConnection
    {
        /// <summary>
        /// Cria uma nova conexão com determinado servidor do rbMQ
        /// </summary>
        /// <returns></returns>
        public ConnectionFactory RMQ_NewConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = AmqpTcpEndpoint.UseDefaultPort,
                UserName = "guest",
                Password = "guest"
            };

            return factory;
        }
    }
}

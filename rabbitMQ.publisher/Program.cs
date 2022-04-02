using System;
using System.Threading;

namespace rabbitMQ.publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string USER = "39104545850";

            SingleChannel rbMQ = new SingleChannel();

            //MultiChannel rbMQ = new MultiChannel();

            int i = 1;

            while(true)
            {
                var product = new Product();

                product.Id = i;
                product.EAN = "";
                product.ProdDescription = $"PRODUTO_{i}";
                product.InternalCode = (i + 1).ToString();
                product.UserCNPJ = USER;

                Thread.Sleep(500);

                var acessHist = new AcessHistory();

                acessHist.CNPJ = "";
                acessHist.AcessHistDate = DateTime.Now.ToShortDateString();
                acessHist.AcessHistHour = DateTime.Now.ToShortTimeString();
                acessHist.AcessHistTotalProductsSent = 0;
                acessHist.AcessHistTotalProductsReceived = 0;

                //rbMQ.Publish(product, acessHist);
                rbMQ.Publish_ProdClient(product);
                rbMQ.Publish_AcessHistory(acessHist);

                i++;
            }

            //for (int i = 1; i < 10; i++)
            //{
                
            //}
        }
    }
}

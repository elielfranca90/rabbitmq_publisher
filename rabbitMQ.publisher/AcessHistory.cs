using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rabbitMQ.publisher
{
    public class AcessHistory
    {
        public string CNPJ { get; set; }

        public string AcessHistDate { get; set; }

        public string AcessHistHour { get; internal set; }

        public int AcessHistTotalProductsSent { get; set; }

        public int AcessHistTotalProductsReceived { get; set; }

    }
}

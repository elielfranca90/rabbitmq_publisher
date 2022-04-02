using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rabbitMQ.publisher
{
    public class Product
    {
        public int Id { get; set; }

        public string InternalCode { get; set; }

        public string EAN { get; set; }

        public string ProdDescription { get; set; }

        public string UserCNPJ { get; set; }

    }
}

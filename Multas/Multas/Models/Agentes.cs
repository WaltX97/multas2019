using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Agentes {
        public Agentes()
        {
            ListaDasMultas = new HashSet<Multas>();
        }

        public int ID { get; set; }

        public string Nome { get; set; }

        public string Esquadra { get; set; }

        public string Fotografia { get; set; }

        // identifica as multas passadas pelo Agente

        public virtual ICollection<Multas> ListaDasMultas {get; set;}


    }
}
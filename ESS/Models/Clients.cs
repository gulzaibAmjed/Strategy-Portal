using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESS.Models
{
    public class Clients
    {
        public int Clients_ID { get; set; }
        public string Clients_Name { get; set; }
        public string client_ID { get; set; }
        public string client_Name { get; set; }

        public IEnumerable<SelectListItem> Items { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS.Models
{
    public class TemplateMaster
    {
        public TemplateMaster()
       {
           HTMLTEMP = new List<HTMLTemplate>();
       }
       public string Name { get; set; }
       public List<HTMLTemplate> HTMLTEMP { get; set; }
    }
}
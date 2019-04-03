using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogWebApi.Models
{
    public class CompanyModel
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public long Contact { get; set; }
        public string Address { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogWebApi.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Level { get; set; }
        public int? EmployeeID { get; set; }
        public EmployeeModel Employee { get; set; }
    }
}
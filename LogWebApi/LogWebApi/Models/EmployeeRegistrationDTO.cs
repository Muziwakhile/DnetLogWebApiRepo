using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogWebApi.Models
{
    public class EmployeeRegistrationDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public long Contact { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Level { get; set; }
    }
}
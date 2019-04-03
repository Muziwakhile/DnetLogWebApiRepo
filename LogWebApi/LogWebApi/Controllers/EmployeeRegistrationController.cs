using LogWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LogWebApi.Controllers
{
    public class EmployeeRegistrationController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage RegisterEmployee(EmployeeRegistrationDTO registrationDTO)
        {
            using (DbEntities entities = new DbEntities())
            {

                var checkresults = (from e in entities.Employees
                                    where e.Emp_Name == registrationDTO.Name || e.Emp_Surname == registrationDTO.Surname || e.Emp_Email == registrationDTO.Email
                                    select e).FirstOrDefault();

                if (checkresults != null)
                {

                    EmployeeModel employeeModel = new EmployeeModel
                    {
                        ID = checkresults.Emp_No,
                        Name = checkresults.Emp_Name,
                        Surname = checkresults.Emp_Surname,
                        Email = checkresults.Emp_Email,
                        Contact = checkresults.Emp_Contact,
                        Position = checkresults.Emp_Position,
                        Status = checkresults.Emp_Status
                    };

                    return Request.CreateResponse(HttpStatusCode.Conflict, employeeModel);


                }
                else
                {
                    Employee emp = new Employee
                    {
                        Emp_Name = registrationDTO.Name,
                        Emp_Surname = registrationDTO.Surname,
                        Emp_Email = registrationDTO.Email,
                        Emp_Contact = registrationDTO.Contact,
                        Emp_Position = registrationDTO.Position,
                        Emp_Status = "Active"
                    };

                    User u = new User
                    {
                        users_name = registrationDTO.Username,
                        users_password = registrationDTO.Password,
                        Employee = emp,
                        users_level = "Technical"
                    };

                    entities.Users.Add(u);
                    entities.SaveChanges();


                    EmployeeModel employeeModel = new EmployeeModel
                    {
                        ID = emp.Emp_No,
                        Name = emp.Emp_Name,
                        Surname = emp.Emp_Surname,
                        Email = emp.Emp_Email,
                        Contact = emp.Emp_Contact,
                        Position = emp.Emp_Position,
                        Status = emp.Emp_Status
                    };

                    UserModel userModel = new UserModel
                    {
                        ID = u.users_no,
                        Username = u.users_name,
                        Password = u.users_password,
                        Level = u.users_level,
                        EmployeeID = u.Emp_no,
                        Employee = employeeModel
                    };

                    var message = Request.CreateResponse(HttpStatusCode.Created, userModel);
                    message.Headers.Location = new Uri(Request.RequestUri + "/" + userModel.EmployeeID.ToString());

                    return message;
                }
            }
        }

    }
}

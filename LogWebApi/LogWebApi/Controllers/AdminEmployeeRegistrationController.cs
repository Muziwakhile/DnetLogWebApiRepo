using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogWebApi.Models;

namespace LogWebApi.Controllers
{
    public class AdminEmployeeRegistrationController : ApiController
    {
        [HttpGet]
        public List<EmployeeModel> AdminGetAllEmployees()
        {
            List<EmployeeModel> emplist = new List<EmployeeModel>();
            using (DbEntities entities = new DbEntities())
            {
                var employees = (from e in entities.Employees
                                 select e).ToList<Employee>();

                foreach (var item in employees)
                {
                    emplist.Add( new EmployeeModel
                    {
                        ID =item.Emp_No,
                        Name = item.Emp_Name,
                        Surname = item.Emp_Surname,
                        Email = item.Emp_Email,
                        Contact = item.Emp_Contact,
                        Position = item.Emp_Position,
                        Status = item.Emp_Status
                    });
                }

                return emplist;
            }
        }
        [HttpGet]
        public HttpResponseMessage GetAllEmployees(int id)
        {
            using (DbEntities entities = new DbEntities())
            {
                var employee = (from e in entities.Employees
                                where e.Emp_No == id
                                 select e).FirstOrDefault();

                if (employee != null)
                {
                    EmployeeModel employeeModel = new EmployeeModel
                    {
                        ID = employee.Emp_No,
                        Name = employee.Emp_Name,
                        Surname = employee.Emp_Surname,
                        Email = employee.Emp_Email,
                        Contact = employee.Emp_Contact,
                        Position = employee.Emp_Position,
                        Status = employee.Emp_Status
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, employeeModel);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,$"Employee with ID {id} was not found");
                }   
            }
        }


        [HttpPost]
        public HttpResponseMessage AdminRegisterEmployee(EmployeeRegistrationDTO registrationDTO)
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
                        Emp_Status = registrationDTO.Status
                    };

                    User u = new User
                    {
                        users_name = registrationDTO.Username,
                        users_password = registrationDTO.Password,
                        users_level = registrationDTO.Level,
                        Employee = emp
                       
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

        [HttpPut]
        public HttpResponseMessage AdminUpdateEmployee(EmployeeModel employee)
        {
            using (DbEntities entities = new DbEntities())
            {
                var results = (from e in entities.Employees
                               where e.Emp_No == employee.ID
                               select e).FirstOrDefault();

                if (results != null)
                {
                    results.Emp_Name = employee.Name;
                    results.Emp_Surname = employee.Surname;
                    results.Emp_Email = employee.Email;
                    results.Emp_Contact = employee.Contact;
                    results.Emp_Position = employee.Position;
                    results.Emp_Status = employee.Status;

                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, employee);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"The specified Employee was not found");
                }
            }
        }

    }

}


using LogWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LogWebApi.Controllers
{
    public class EmployeeLoginController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage EmployeeLogin([FromUri]string username,[FromUri]string password)
        {
            using (DbEntities entities = new DbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = true;
                var userResult = (from u in entities.Users
                                  where u.users_name == username && u.users_password == password
                                  select u).FirstOrDefault();

                if (userResult != null)
                {
                    if (userResult.Employee.Emp_Status != "In-Active")
                    {
                        EmployeeModel employee = new EmployeeModel
                        {
                            ID = userResult.Emp_no,
                            Name = userResult.Employee.Emp_Name,
                            Surname = userResult.Employee.Emp_Surname,
                            Email = userResult.Employee.Emp_Email,
                            Contact = userResult.Employee.Emp_Contact,
                            Position = userResult.Employee.Emp_Position,
                            Status = userResult.Employee.Emp_Status
                        };
                        UserModel userModel = new UserModel
                        {
                            ID = userResult.users_no,
                            Username = userResult.users_name,
                            Password = userResult.users_password,
                            Level = userResult.users_level,
                            EmployeeID = userResult.Emp_no,
                            Employee = employee

                        };

                        return Request.CreateResponse(HttpStatusCode.OK, userModel);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Is Forbiden. Please Contact Administrator");
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No User Was Found With Matching Credentials");
                }
            }
        }


        [HttpPut]
        public HttpResponseMessage UpdateEmployeeLoginDetails(UserModel user)
        {
            using (DbEntities entities = new DbEntities())
            {
                var userResult = (from u in entities.Users
                                  where u.users_no == user.ID
                                  select u).FirstOrDefault();
                if (userResult != null)
                {
                    userResult.users_password = user.Password;
                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, user);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotModified,"Could Not Update Password. Please Contact Administrator");
                }
            }
        }

    }
}

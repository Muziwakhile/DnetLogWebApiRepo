using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogWebApi.Models;

namespace LogWebApi.Controllers
{
    public class AdminEmployeeLoginController : ApiController
    {
        [HttpGet]
        public List<UserModel> GetAllEmployeeUers()
        {
            List<UserModel> userList = new List<UserModel>();
            using (DbEntities entities = new DbEntities())
            {
                var users = (from u in entities.Users
                             select u).ToList<User>();

                foreach (var item in users)
                {
                    userList.Add(
                        new UserModel
                        {
                            ID = item.users_no,
                            Username = item.users_name,
                            Password = item.users_password,
                            Level = item.users_level,
                            EmployeeID = item.Emp_no
                        }
                        );
                }

                return userList;
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllEmployeeUers(int id)
        {
            using (DbEntities entities = new DbEntities())
            {
                var user = (from u in entities.Users
                            where u.users_no == id
                            select u).FirstOrDefault<User>();

                if (user != null)
                {
                    UserModel userModel = new UserModel
                    {
                        ID = user.users_no,
                        Username = user.users_name,
                        Password = user.users_password,
                        Level = user.users_level,
                        EmployeeID = user.Emp_no
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, userModel);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"No User with ID {id} Was Found");
                }

            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateEmployeeLogin(UserModel user)
        {
            using (DbEntities entities = new DbEntities())
            {
                var userResult = (from u in entities.Users
                                  where u.users_no == user.ID
                                  select u).FirstOrDefault<User>();

                if (userResult != null)
                {
                    userResult.users_name = user.Username;
                    userResult.users_password = user.Password;
                    userResult.users_level = user.Level;
                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, user);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"No User with ID {user.ID} Was Found");
                }

            }
        }
    }
}

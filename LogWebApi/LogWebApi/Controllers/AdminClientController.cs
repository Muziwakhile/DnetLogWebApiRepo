using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogWebApi.Models; 
namespace LogWebApi.Controllers
{
    public class AdminClientController : ApiController
    {
        [HttpGet]
        public List<ClientModel> GetAllClients()
        {
            List<ClientModel> clientList = new List<ClientModel>();
            using (DbEntities entities = new DbEntities())
            {
                var clients = (from cu in entities.Client_User
                               select cu).ToList<Client_User>();

                foreach (var item in clients)
                {
                    clientList.Add(new ClientModel
                    {
                        ID = item.client_user_no,
                        Name = item.cu_name,
                        Surname = item.cu_surname,
                        Email = item.cu_email,
                        Contact = item.cu_contact,
                        Username = item.cu_password,
                        CompanyID = item.client_no
                    });

                }

                return clientList;
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllClients(int id)
        {
            using (DbEntities entities = new DbEntities())
            {
                var clients = (from cu in entities.Client_User
                               where cu.client_user_no == id
                               select cu).FirstOrDefault<Client_User>();

                if (clients != null)
                {
                    ClientModel model = new ClientModel
                    {
                        ID = clients.client_user_no,
                        Name = clients.cu_name,
                        Surname = clients.cu_surname,
                        Email = clients.cu_email,
                        Contact = clients.cu_contact,
                        Username = clients.cu_password,
                        CompanyID = clients.client_no
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,$"Client With ID {id} Was Not Found");
                }   
            }
        }


        [HttpPut]
        public HttpResponseMessage EditClient(ClientModel client)
        {
            using (DbEntities entities = new DbEntities())
            {
                var result = (from u in entities.Client_User
                              where u.client_user_no == client.ID
                              select u).FirstOrDefault<Client_User>();

                if (result != null)
                {
                    result.cu_name = client.Name;
                    result.cu_surname = client.Surname;
                    result.cu_email = client.Email;
                    result.cu_contact = client.Contact;
                    result.cu_username = client.Username;
                    result.cu_password = client.Password;

                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK,client);
                    
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"Client With ID {client.ID} Was Not Found");
                }
            }
        }

    }
}

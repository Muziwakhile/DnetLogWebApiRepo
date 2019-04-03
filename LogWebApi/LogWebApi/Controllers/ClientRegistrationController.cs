using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogWebApi.Models;

namespace LogWebApi.Controllers
{
    public class ClientRegistrationController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage RegisterClient(ClientModel client)
        {
            using (DbEntities entities = new DbEntities())
            {
                var clientExists = (from c in entities.Client_User
                                    where c.cu_name == client.Name || c.cu_surname == client.Surname || c.cu_email == client.Email
                                    select  c).FirstOrDefault();

                if (clientExists != null)
                {
                    if (client.Username != null && client.Password != null)
                    {
                         ClientModel clientModel = new ClientModel
                        {
                            ID = clientExists.client_user_no,
                            Name = clientExists.cu_name,
                            Surname = clientExists.cu_surname,
                            Email = clientExists.cu_email,
                            Contact = clientExists.cu_contact,
                            CompanyID = clientExists.client_no,
                            Username = clientExists.cu_username,
                            Password = clientExists.cu_password
                        };
                        return Request.CreateResponse(HttpStatusCode.Conflict, clientModel);
                    }
                    else
                    {
                        ClientModel clientModel = new ClientModel
                        {
                            ID = clientExists.client_user_no,
                            Name = clientExists.cu_name,
                            Surname = clientExists.cu_surname,
                            Email = clientExists.cu_email,
                            Contact = clientExists.cu_contact,
                            CompanyID = clientExists.client_no
                        };
                        return Request.CreateResponse(HttpStatusCode.Found, clientModel);
                    }
                   
                }
                else
                {
                    Client_User client_User = new Client_User
                    {
                        client_user_no = client.ID,
                        cu_name = client.Name,
                        cu_surname = client.Surname,
                        cu_email = client.Email,
                        cu_contact = client.Contact,
                        cu_username = client.Username,
                        cu_password = client.Password,
                        client_no = client.CompanyID
                    };

                    entities.Client_User.Add(client_User);
                    entities.SaveChanges();

                    client.ID = client_User.client_no;

                    var message = Request.CreateResponse(HttpStatusCode.Created,client);
                    message.Headers.Location = new Uri(Request.RequestUri +"/"+ client.ID.ToString());

                    return message;
                }
            }
        }


        [HttpPut]
        public HttpResponseMessage UpdateClientDetails(ClientModel clientModel)
        {
            using (DbEntities entities = new DbEntities())
            {
                var update = (from c in entities.Client_User
                              where c.client_user_no == clientModel.ID
                              select c).FirstOrDefault<Client_User>();
                if (update != null)
                {
                    update.cu_contact = clientModel.Contact;
                    update.cu_email = clientModel.Email;
                    update.cu_username = clientModel.Username;
                    update.cu_password = clientModel.Password;

                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK,clientModel);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,$"User With ID {clientModel.ID} Was not Found");
                }
            }
        }
    }
}

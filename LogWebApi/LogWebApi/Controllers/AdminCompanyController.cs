using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogWebApi.Models;

namespace LogWebApi.Controllers
{
    public class AdminCompanyController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage AddCompanies(CompanyModel companyModel)
        {
            using (DbEntities entities = new DbEntities())
            {
                Client comp = new Client
                {
                    client_name = companyModel.CompanyName,
                    client_contact = companyModel.Contact,
                    Client_address = companyModel.Address
                };

                entities.Clients.Add(comp);
                entities.SaveChanges();

                companyModel.ID = comp.client_no;

                var message = Request.CreateResponse(HttpStatusCode.Created, companyModel);
                message.Headers.Location = new Uri(Request.RequestUri + "/" + companyModel.ID.ToString());

                return message;
            }
        }


        [HttpGet]
        public List<CompanyModel> GeAllCompanies()
        {
            List<CompanyModel> companyList = new List<CompanyModel>();
            using (DbEntities entities = new DbEntities())
            {
                var companies = (from c in entities.Clients
                                 select c).ToList<Client>();

                foreach (var item in companies)
                {
                    companyList.Add(new CompanyModel
                    {
                        ID = item.client_no,
                        CompanyName = item.client_name,
                        Contact = item.client_contact,
                        Address = item.Client_address
                    });
                }

                return companyList;
            }
        }


        [HttpGet]
        public HttpResponseMessage GeAllCompanies(int id)
        {
            using (DbEntities entities = new DbEntities())
            {
                var company = (from c in entities.Clients
                               where c.client_no == id
                               select c).FirstOrDefault<Client>();
                if (company != null)
                {
                    CompanyModel companyModel = new CompanyModel
                    {
                        ID = company.client_no,
                        CompanyName = company.client_name,
                        Contact = company.client_contact,
                        Address = company.Client_address
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, companyModel);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Company With ID {id} Was Not Fount");
                }

            }
        }


        [HttpPut]
        public HttpResponseMessage UpdateCompanies(CompanyModel companyModel)
        {
            using (DbEntities entities = new DbEntities())
            {
                var company = (from c in entities.Clients
                               where c.client_no == companyModel.ID
                               select c).FirstOrDefault<Client>();
                if (company != null)
                {
                    company.client_name = companyModel.CompanyName;
                    company.client_contact = companyModel.Contact;
                    company.Client_address = companyModel.Address;

                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, companyModel);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Company With ID {companyModel.ID} Was Not Fount");
                }

            }
        }
    }
}

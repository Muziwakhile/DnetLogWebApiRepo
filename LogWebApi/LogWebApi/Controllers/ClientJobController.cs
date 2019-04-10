using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogWebApi.Models;
namespace LogWebApi.Controllers
{
    public class ClientJobController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage CreateLog(JobModel jobModel)
        {
            using (DbEntities entities = new DbEntities())
            {
                Job job = new Job
                {
                    job_title = jobModel.Title,
                    job_description = jobModel.Description,
                    job_attachement = jobModel.Attachment,
                    job_date = jobModel.Date,
                    client_user_no = jobModel.ClientID,
                    job_status = "Registered"
                };

                entities.Jobs.Add(job);
                entities.SaveChanges();

                jobModel.ID = job.job_No;

                var message = Request.CreateResponse(HttpStatusCode.Created, jobModel);
                message.Headers.Location = new Uri(Request.RequestUri + "/" + jobModel.ID);

                return message;
            }
        }
    }
}

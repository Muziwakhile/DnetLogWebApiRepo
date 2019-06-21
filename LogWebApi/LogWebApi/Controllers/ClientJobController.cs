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

        [HttpGet]
        public List<JobModel> GetAllJobs(int Id)
        {
            List<JobModel> jobList = new List<JobModel>();
            using (DbEntities entities = new DbEntities())
            {
                var result = (from j in entities.Jobs
                              where j.client_user_no == Id

                              select j).ToList<Job>();



                if (result != null)
                {
                    foreach (var item in result)
                    {
                        jobList.Add(new JobModel
                        {
                            ID = item.job_No,
                            Title = item.job_title,
                            Description = item.job_description,
                            Date = item.job_date,
                            Attachment = item.job_attachement,
                            ClientID = item.client_user_no,
                            Status = item.job_status
                        });
                    }
                    return jobList;

                }
                else
                {
                    return null;
                }
            }
        }
    }
}

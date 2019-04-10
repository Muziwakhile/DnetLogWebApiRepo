using LogWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LogWebApi.Controllers
{
    public class JobController : ApiController
    {

        [HttpGet]
        public List<JobModel> GetJobs()
        {
            List<JobModel> jobList = new List<JobModel>();
            using (DbEntities entities = new DbEntities())
            {
                var jobs = (from j in entities.Jobs
                            select j).ToList<Job>();

                foreach (var item in jobs)
                {
                    jobList.Add(new JobModel
                    {
                        ID = item.job_No,
                        Title = item.job_title,
                        Description = item.job_description,
                        Attachment = item.job_attachement,
                        Date = item.job_date,
                        Status = item.job_status,
                        ClientID = item.client_user_no
                    });
                }

                return jobList;
            }
        }



        [HttpGet]
        public HttpResponseMessage GetJobs(int id)
        {
            using (DbEntities entities = new DbEntities())
            {
                var job = (from j in entities.Jobs
                            where j.job_No == id
                            select j).FirstOrDefault<Job>();

                if (job != null)
                {
                    JobModel jobModel = new JobModel
                    {
                        ID = job.job_No,
                        Title = job.job_title,
                        Description = job.job_description,
                        Attachment = job.job_attachement,
                        Date = job.job_date,
                        Status = job.job_status,
                        ClientID = job.client_user_no
                    };


                    return Request.CreateResponse(HttpStatusCode.OK,jobModel);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"The Specified Job With ID {id} Was Not Found");
                }     
            }
        }



        [HttpPost]
        public HttpResponseMessage CreateNewJob(JobDTO jobDTO)
        {
            using (DbEntities entities = new DbEntities())
            {
                Client_User _User = new Client_User
                {
                    cu_name = jobDTO.Name,
                    cu_surname = jobDTO.Surname,
                    cu_email = jobDTO.Email,
                    cu_contact = jobDTO.Contact,
                    client_no = jobDTO.CompanyID
                };

                Job job = new Job
                {
                    job_title = jobDTO.Title,
                    job_description = jobDTO.Description,
                    job_attachement = jobDTO.Attachment,
                    client_user_no = jobDTO.ClientID,
                    job_date = jobDTO.Date,
                    job_status = "Registered",
                    Client_User = _User
                };

                entities.Jobs.Add(job);
                entities.SaveChanges();

                JobModel jobModel = new JobModel
                {
                    ID = job.job_No,
                    Title = job.job_title,
                    Description = job.job_description,
                    Attachment = job.job_attachement,
                    ClientID = job.client_user_no,
                    Date = job.job_date,
                    Status = job.job_status
                };

                var message = Request.CreateResponse(HttpStatusCode.Created, jobModel);
                message.Headers.Location = new Uri(Request.RequestUri + "/" + jobModel.ID);

                return message;
            }
        }
    }
}

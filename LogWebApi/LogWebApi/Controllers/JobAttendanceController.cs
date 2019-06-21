using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogWebApi.Models;

namespace LogWebApi.Controllers
{
    public class JobAttendanceController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage AttendJob(JobAttendanceModel jobAttendance)
        {
            using (DbEntities entities = new DbEntities())
            {
                Emp_Job _Job = new Emp_Job
                {
                    job_no = jobAttendance.JobID,
                    emp_no = jobAttendance.EmployeeID,
                    emp_job_start = jobAttendance.StartDate
                };

                var result = (from s in entities.Jobs
                              where s.job_No == jobAttendance.JobID
                              select s).FirstOrDefault();

                result.job_status = "Pending";

                entities.Emp_Job.Add(_Job);
                entities.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }



        [HttpPut]
        public HttpResponseMessage JobCompletion(JobAttendanceDTO jobDone)
        {

            if (jobDone.Comments != null)
            {
                using (DbEntities entities = new DbEntities())
                {
                    Comment cm = new Comment()
                    {
                        com_date = DateTime.Now,
                        com_description = jobDone.Comments,
                        job_no = jobDone.JobID,
                        emp_no = jobDone.EmployeeID
                    };
                    var results = (from s in entities.Jobs
                                   where s.job_No == jobDone.JobID
                                   select s).FirstOrDefault();
                    results.job_status = "Done";


                    var r = (from f in entities.Emp_Job
                             where f.job_no == jobDone.JobID & f.emp_no == jobDone.EmployeeID
                             select f).FirstOrDefault();
                    r.emp_job_end = jobDone.EndDate;

                    entities.Comments.Add(cm);
                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, results);

                }
            }
            else
            {
                using (DbEntities entities = new DbEntities())
                {
                    var results = (from s in entities.Jobs
                                   where s.job_No == jobDone.JobID
                                   select s).FirstOrDefault();
                    results.job_status = "Done";

                    var r = (from f in entities.Emp_Job
                             where f.job_no == jobDone.JobID & f.emp_no == jobDone.EmployeeID
                             select f).FirstOrDefault();
                    r.emp_job_end = jobDone.EndDate;

                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, results);

                }

            }


            //public HttpResponseMessage AdminAssignJob(IEnumerable<JobAttendanceModel> jobAttendance)
            //{
            //    List<Emp_Job> jobAssignment = new List<Emp_Job>();
            //    using (DbEntities entities = new DbEntities())
            //    {
            //        foreach (var item in jobAttendance)
            //        {
            //            jobAssignment.Add(new Emp_Job
            //            {
            //                job_no = item.JobID,
            //                emp_no = item.EmployeeID,
            //                emp_job_start = item.StartDate
            //            });
            //        }

            //        entities.Emp_Job.AddRange(jobAssignment);
            //        entities.SaveChanges();
            //        return Request.CreateResponse(HttpStatusCode.OK);
            //    }
            //}


        }


    }

}


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

                entities.Emp_Job.Add(_Job);
                entities.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        public HttpResponseMessage AdminAssignJob(IEnumerable<JobAttendanceModel> jobAttendance)
        {
            List<Emp_Job> jobAssignment = new List<Emp_Job>(); 
            using (DbEntities entities = new DbEntities())
            {
                foreach (var item in jobAttendance)
                {
                    jobAssignment.Add(new Emp_Job
                    {
                        job_no = item.JobID,
                        emp_no = item.EmployeeID,
                        emp_job_start = item.StartDate
                    });
                }

                entities.Emp_Job.AddRange(jobAssignment);
                entities.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
    }
}

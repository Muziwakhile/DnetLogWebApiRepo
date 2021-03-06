﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogWebApi.Models
{
    public class JobModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Attachment { get; set; }
        public DateTime Date { get; set; }
        public int ClientID { get; set; }
        public string Status { get; set; }
    }
}
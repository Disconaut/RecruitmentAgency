using System;
using System.Collections.Generic;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class Interview
    {
        public int InterviewId { get; set; }
        public int ApplicationId { get; set; }
        public DateTime InterviewDate { get; set; }
        public string Feedback { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Application Application { get; set; }
    }
}

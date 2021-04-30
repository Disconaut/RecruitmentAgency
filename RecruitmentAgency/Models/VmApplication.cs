using System;
using System.Collections.Generic;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class VmApplication
    {
        public int ApplicationId { get; set; }
        public int VacancyId { get; set; }
        public int CandidateId { get; set; }
        public int RecruiterId { get; set; }
        public string Candidate { get; set; }
        public string Status { get; set; }
        public string JobTitle { get; set; }
        public string JobCategory { get; set; }
        public string Recruiter { get; set; }
        public string Customer { get; set; }
        public string CustomerCompany { get; set; }
        public string City { get; set; }
    }
}

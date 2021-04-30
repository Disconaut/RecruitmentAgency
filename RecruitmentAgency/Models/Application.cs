using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class Application
    {
        public Application()
        {
            Interviews = new HashSet<Interview>();
        }

        public int ApplicationId { get; set; }
        [Required]
        [Display(Name="Candidate")]
        public int CandidateId { get; set; }
        public int VacancyId { get; set; }
        public int ApplicationStatusId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ApplicationStatus ApplicationStatus { get; set; }
        public virtual Candidate Candidate { get; set; }
        public virtual Vacancy Vacancy { get; set; }
        public virtual ICollection<Interview> Interviews { get; set; }
    }
}

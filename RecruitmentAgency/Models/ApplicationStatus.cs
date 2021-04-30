using System;
using System.Collections.Generic;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class ApplicationStatus
    {
        public ApplicationStatus()
        {
            Applications = new HashSet<Application>();
        }

        public int ApplicationStatusId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
    }
}

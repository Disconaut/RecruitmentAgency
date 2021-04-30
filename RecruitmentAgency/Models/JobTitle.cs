using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class JobTitle
    {
        public JobTitle()
        {
            Vacancies = new HashSet<Vacancy>();
        }

        public int JobTitleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name="Job category")]
        public int JobCategoryId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        
        [Display(Name = "Job category")]
        public virtual JobCategory JobCategory { get; set; }
        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class Department
    {
        public Department()
        {
            Candidates = new HashSet<Candidate>();
            Employees = new HashSet<Employee>();
        }

        public int DepartmentId { get; set; }
        public int CityId { get; set; }
        [Display(Name = "Dptmt. address", Description = "Department address", Prompt = "Department address")]
        public string Address { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Display(Name = "City")]
        public virtual City City { get; set; }
        public virtual ICollection<Candidate> Candidates { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}

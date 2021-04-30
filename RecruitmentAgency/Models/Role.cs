using System;
using System.Collections.Generic;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class Role
    {
        public Role()
        {
            Employees = new HashSet<Employee>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public decimal PercentForVacancy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}

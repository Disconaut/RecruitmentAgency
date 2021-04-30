using System;
using System.Collections.Generic;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class City
    {
        public City()
        {
            CompanyOffices = new HashSet<CompanyOffice>();
            Departments = new HashSet<Department>();
        }

        public int CityId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<CompanyOffice> CompanyOffices { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }
}

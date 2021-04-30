using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class CompanyOffice
    {
        public CompanyOffice()
        {
            Vacancies = new HashSet<Vacancy>();
        }

        public int CompanyOfficeId { get; set; }
        [Display(Name="Company")]
        public int CompanyId { get; set; }
        public int CityId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Address { get; set; }

        public virtual City City { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}

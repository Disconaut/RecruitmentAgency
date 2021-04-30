using System;
using System.Collections.Generic;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class Company
    {
        public Company()
        {
            CompanyOffices = new HashSet<CompanyOffice>();
            Customers = new HashSet<Customer>();
        }

        public int CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<CompanyOffice> CompanyOffices { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}

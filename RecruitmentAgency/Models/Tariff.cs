using System;
using System.Collections.Generic;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class Tariff
    {
        public Tariff()
        {
            Vacancies = new HashSet<Vacancy>();
        }

        public int TariffId { get; set; }
        public string Name { get; set; }
        public int ManagerId { get; set; }
        public decimal PriceForCandidate { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Employee Manager { get; set; }
        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}

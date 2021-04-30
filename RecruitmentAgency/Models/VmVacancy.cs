using System;
using System.Collections.Generic;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class VmVacancy
    {
        public int VacancyId { get; set; }
        public int RecruiterId { get; set; }
        public int CustomerId { get; set; }
        public int CompanyId { get; set; }
        public int JobTitleId { get; set; }
        public int JobCategoryId { get; set; }
        public int CityId { get; set; }
        public string JobTitle { get; set; }
        public string JobCategory { get; set; }
        public int PositionsCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Tariff { get; set; }
        public string Recruiter { get; set; }
        public string Customer { get; set; }
        public string CustomerCompany { get; set; }
        public string City { get; set; }
    }
}

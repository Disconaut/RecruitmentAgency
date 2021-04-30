using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using RecruitmentAgency.Models;

namespace RecruitmentAgency.ViewModels
{
    public class VacancyApplicationsResponse
    {
        public int? VacancyId { get; set; }

        public string VacancyJobTitle { get; set; }

        public string VacancyCustomer { get; set; }

        public string VacancyCompany { get; set; }

        public string VacancyCompanyOffice { get; set; }

        public bool VacancyClosed { get; set; }

        public IEnumerable<Application> Applications { get; set; }
    }
}

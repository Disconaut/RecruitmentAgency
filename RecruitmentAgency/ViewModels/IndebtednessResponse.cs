using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RecruitmentAgency.Models;

namespace RecruitmentAgency.ViewModels
{
    public class IndebtednessResponse
    {
        public int CustomerId { get; set; }

        public string CustomerFullName { get; set; }

        public string CustomerCompany { get; set; }

        public IEnumerable<Vacancy> Vacancies { get; set; }
    }
}

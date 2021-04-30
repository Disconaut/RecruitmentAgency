using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RecruitmentAgency.ViewModels
{
    public class PaymentFilter
    {
        public int? Year { get; set; } = -1;
        public int? Month { get; set; } = -1;
        public int? Day { get; set; } = -1;
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; } = -1;
        [Display(Name = "Recruiter")]
        public int? RecruiterId { get; set; } = -1;
        [Display(Name = "Customer")]
        public int? CustomerId { get; set; } = -1;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class Vacancy
    {
        public Vacancy()
        {
            Applications = new HashSet<Application>();
            Payments = new HashSet<Payment>();
        }

        public int VacancyId { get; set; }
        [Display(Name="Job title")]
        public int JobTitleId { get; set; }

        [Display(Name = "Requirements")]
        [MaxLength(4000)]
        public string Requirements { get; set; }
        [Display(Name = "Positions count", Prompt = "Positions count")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Positions count cannot be negative")]
        public int PositionsCount { get; set; }
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End date")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        [Display(Name = "Tariff")]
        public int TariffId { get; set; }
        [Display(Name = "Recruiter")]
        public int RecruiterId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        [Display(Name = "Company office")]
        public int CompanyOfficeId { get; set; }

        [Display(Name = "Company office")]
        public virtual CompanyOffice CompanyOffice { get; set; }

        [Display(Name = "Customer")]
        public virtual Customer Customer { get; set; }
        [Display(Name = "Job title")]
        public virtual JobTitle JobTitle { get; set; }
        [Display(Name = "Recruiter")]
        public virtual Employee Recruiter { get; set; }
        [Display(Name = "Tariff")]
        public virtual Tariff Tariff { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}

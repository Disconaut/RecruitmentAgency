using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int VacancyId { get; set; }
        [Display(Name ="Amount", Prompt = "0.00")]
        [Required]
        [Range(0.01, Double.MaxValue)]
        public decimal Sum { get; set; }
        [Required]
        [Display(Name="Transaction date")]
        public DateTime TransactionDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Vacancy Vacancy { get; set; }
    }
}

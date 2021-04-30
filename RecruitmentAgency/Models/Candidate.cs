using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class Candidate
    {
        public Candidate()
        {
            Applications = new HashSet<Application>();
        }

        public int CandidateId { get; set; }
        [Display(Name = "First name", Prompt = "First name")]
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression("^[A-Z][a-z]+$")]
        public string FirstName { get; set; }
        [Display(Name = "Last name", Prompt = "Last name")]
        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression("^[A-Z][a-z]+$")]
        public string LastName { get; set; }
        [Display(Name = "Department", Prompt = "Department")]
        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }
        [Display(Name = "Email", Prompt = "Email")]
        [EmailAddress(ErrorMessage = "Invalid format of email")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "--")]
        public string Email { get; set; }
        [Display(Name = "Phone number", Prompt = "Phone number")]
        [Phone(ErrorMessage = "Invalid format of phone number")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "--")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Resume file")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "--")]
        public string ResumeUrl { get; set; }

        [Display(Name = "Rating", Prompt = "Rating")]
        [Range(0, 5, ErrorMessage = "Rating must be in range [0; 5]")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "--")]
        public decimal? Rating { get; set; }

        [NotMapped]
        [Display(Name = "Resume")]
        [DataType(DataType.Upload)]
        public IFormFile Resume { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Display(Name="Department")]
        public virtual Department Department { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
    }
}

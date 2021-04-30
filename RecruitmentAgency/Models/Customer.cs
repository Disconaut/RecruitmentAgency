using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Vacancies = new HashSet<Vacancy>();
        }

        public int CustomerId { get; set; }
        [Display(Name = "First name", Prompt = "First name")]
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression("^[A-Z][a-z]+$", ErrorMessage = "Invalid format of first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last name", Prompt = "Last name")]
        [RegularExpression("^[A-Z][a-z]+$", ErrorMessage = "Invalid format of last name")]
        public string LastName { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        [Display(Name = "Email", Prompt = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "--")]
        public string Email { get; set; }

        [Display(Name = "Phone number", Prompt = "Phone number")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "--")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Rating", Prompt = "Rating")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "--")]
        [Range(0, 5, ErrorMessage = "Rating must be in range [0; 5]")]
        public decimal? Rating { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Display(Name="Company")]
        public virtual Company Company { get; set; }
        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}

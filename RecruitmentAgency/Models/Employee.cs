using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class Employee
    {
        public Employee()
        {
            SalaryTransactions = new HashSet<SalaryTransaction>();
            Tariffs = new HashSet<Tariff>();
            Vacancies = new HashSet<Vacancy>();
        }

        public int EmployeeId { get; set; }
        [Display(Name = "First name", Prompt = "First name")]
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression("^[A-Z][a-z]+$")]
        public string FirstName { get; set; }
        [Display(Name = "Last name", Prompt = "Last name")]
        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression("^[A-Z][a-z]+$")]
        public string LastName { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [Display(Name = "Role")]
        public int RoleId { get; set; }
        [Display(Name="Rating", Prompt = "Rating")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "--")]
        [Range(0, 5, ErrorMessage = "Rating must be in range [0; 5]")]
        public decimal? Rating { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Department Department { get; set; }
        [Display(Name = "Role")]
        public virtual Role Role { get; set; }
        public virtual PersonalAccount PersonalAccount { get; set; }
        public virtual ICollection<SalaryTransaction> SalaryTransactions { get; set; }
        public virtual ICollection<Tariff> Tariffs { get; set; }
        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}

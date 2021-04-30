using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class PersonalAccount
    {
        public int PersonalAccountId { get; set; }
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}

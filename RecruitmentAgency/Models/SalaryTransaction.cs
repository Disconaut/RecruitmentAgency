using System;
using System.Collections.Generic;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class SalaryTransaction
    {
        public int SalaryTransactionId { get; set; }
        public int EmployeeId { get; set; }
        public int YearNum { get; set; }
        public byte MonthNum { get; set; }
        public decimal Sum { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string TransactionNum { get; set; }

        public virtual Employee Employee { get; set; }
    }
}

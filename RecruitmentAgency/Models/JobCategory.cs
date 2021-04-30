using System;
using System.Collections.Generic;

#nullable disable

namespace RecruitmentAgency.Models
{
    public partial class JobCategory
    {
        public JobCategory()
        {
            JobTitles = new HashSet<JobTitle>();
        }

        public int JobCategoryId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<JobTitle> JobTitles { get; set; }
    }
}

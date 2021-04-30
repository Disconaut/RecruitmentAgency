using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecruitmentAgency.Models
{
    public enum ApplicationStatusEnum
    {
        New = 1,
        SelectedForInterview,
        CustomerValidation,
        Offering,
        Hired,
        Canceled = 7
    }
}

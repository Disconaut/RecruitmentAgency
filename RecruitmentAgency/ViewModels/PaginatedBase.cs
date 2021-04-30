using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecruitmentAgency.ViewModels
{
    public class PaginatedBase
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int CountOfPages { get; set; }

        public string SearchQuery { get; set; }

        public int TotalItems { get; set; }

        public bool HasNextPage => PageIndex < CountOfPages;

        public bool HasPreviousPage => PageIndex > 1;

        public List<SelectListItem> DefaultPageSizes => new List<SelectListItem>
        {
            new SelectListItem("10", "10"),
            new SelectListItem("25", "25"),
            new SelectListItem("50", "50"),
            new SelectListItem("100", "100")
        };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MegaDeskRazor.Data;
using MegaDeskRazor.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MegaDeskRazor.Pages.Desks
{
    public class IndexModel : PageModel
    {
        private readonly MegaDeskRazor.Data.MegaDeskRazorContext _context;

        public IndexModel(MegaDeskRazor.Data.MegaDeskRazorContext context)
        {
            _context = context;
        }

        public IList<Desk> Desk { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList? CustomerName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? DeskCustomerName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }

        public async Task OnGetAsync()
        {
            var desks = from d in _context.Desk
                         select d;

            // Searching
            if (!string.IsNullOrEmpty(SearchString))
            {
                desks = desks.Where(s => s.CustomerName.Contains(SearchString));
            }

            // Sorting by Customer Name
            switch (SortBy)
            {
                case "name":
                    desks = (SortOrder == "desc") ? desks.OrderByDescending(s => s.CustomerName) : desks.OrderBy(s => s.CustomerName);
                    break;
                default:
                    desks = desks.OrderBy(s => s.CustomerName);
                    break;
            }

            Desk = await desks.ToListAsync();
        }
    }
}

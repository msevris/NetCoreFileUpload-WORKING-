using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetCoreFileUpload.Data;
using NetCoreFileUpload.Models;

namespace NetCoreFileUpload.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        private readonly NetCoreFileUpload.Data.CompanyContext _context;

        public DetailsModel(NetCoreFileUpload.Data.CompanyContext context)
        {
            _context = context;
        }

        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);

            if (Employee == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

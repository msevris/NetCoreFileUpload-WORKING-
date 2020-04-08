using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetCoreFileUpload.Data;
using NetCoreFileUpload.Models;
using NetCoreFileUpload.Models.CompanyViewModels;

namespace NetCoreFileUpload.Pages.Employees
{
    public class IndexModel : PageModel
    {
        private readonly CompanyContext _context;

        public IndexModel(CompanyContext context)
        {
            _context = context;
        }

        public EmployeeIndexData EmployeeData { get; set; }
        public int EmployeeID { get; set; }
        public int FileID { get; set; }

        
        public async Task OnGetAsync(int? id,int? fileID)
        {
            EmployeeData = new EmployeeIndexData();
            EmployeeData.Employees = await _context.Employees
                .Include(f => f.AppFiles)
                .OrderBy(f => f.Id)
                .ToListAsync();

            if (id!=null)
            {
                EmployeeID = id.Value;
                Employee employee = EmployeeData.Employees.Where(e => e.Id == id.Value).Single();
            }
            if (fileID!=null)
            {
                FileID = fileID.Value;
                var selectedImage = EmployeeData.Files.Where(f => f.Id == fileID).Single();
            }
        }
        
    }
}

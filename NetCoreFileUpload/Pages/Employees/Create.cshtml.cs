using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using NetCoreFileUpload.Data;
using NetCoreFileUpload.Models;
using NetCoreFileUpload.Utilities;

namespace NetCoreFileUpload.Pages.Employees
{
    public class CreateModel : PageModel
    {
        private readonly CompanyContext _context;
        private readonly long _fileSizeLimit;
        private readonly string[] _permitedExtensions = { ".png", ".jpg" }; 

        public CreateModel(CompanyContext context,IConfiguration config)
        {
            _context = context;
            _fileSizeLimit = config.GetValue<long>("FleSizeLimit");
        }

        [BindProperty]
        public Employee Employee { get; set; }
        [BindProperty]
        public MultipleFileUpload FileUpload { get; set; }

        public string Result { get; private set; }

        public IActionResult OnGet()
        {
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Result = " Please Correct the Form";
                return Page();
            }
            var emp = new Employee()
            {
                Name = Employee.Name
            };
            foreach (var formFile in FileUpload.FormFiles)
            {

                var formFileContent =
                    await FileHelpers.ProcessFormFile<MultipleFileUpload>(
                        formFile, ModelState, _permitedExtensions, _fileSizeLimit);

                var file = new AppFile()
                {
                    Content = formFileContent,
                    UntrustedName = formFile.FileName,
                    Size = formFile.Length,
                    UploadDT = DateTime.Now,
                    Employee = emp
                };
                _context.AppFiles.AddRange(file);
            }
            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public class MultipleFileUpload
        {
            [Required]
            [Display(Name="Files")]
            public List<IFormFile> FormFiles { get; set; }


        }
    }
}

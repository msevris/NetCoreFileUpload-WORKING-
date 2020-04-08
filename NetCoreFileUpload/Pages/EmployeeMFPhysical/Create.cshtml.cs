using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using NetCoreFileUpload.Data;
using NetCoreFileUpload.Models;
using NetCoreFileUpload.Utilities;

namespace NetCoreFileUpload.Pages.EmployeeMFPhysical
{
    public class CreateModel : PageModel
    {
        private readonly CompanyContext _context;
        private readonly long _fileSizeLimit;
        private readonly string[] _permitedExtensions = { ".png", ".jpg" };
        private readonly string _targetFilePath;

        public CreateModel(CompanyContext context,IConfiguration config)
        {
            _context = context;
            _fileSizeLimit = config.GetValue<long>("FleSizeLimit");
            _targetFilePath = config.GetValue<string>("StoredFilesPath");
        }
        [BindProperty]
        public Employee Employee { get; set; }
        [BindProperty]
        public MultipleFileUploadPhysical FileUpload { get; set; }

        public string Result { get; private set; }

        public void OnGet()
        {

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
                    await FileHelpers
                        .ProcessFormFile<MultipleFileUploadPhysical>(
                            formFile, ModelState, _permitedExtensions,
                            _fileSizeLimit);
                var trustedFileNameForFileStorage = Path.GetRandomFileName();
                var filePath = Path.Combine(
                    _targetFilePath, trustedFileNameForFileStorage);

                using (var fileStream = System.IO.File.Create(filePath))
                {
                    await fileStream.WriteAsync(formFileContent);

                    // To work directly with the FormFiles, use the following
                    // instead:
                    //await formFile.CopyToAsync(fileStream);
                }

            }
            return RedirectToPage("./Index");
        }

        public class MultipleFileUploadPhysical
        {
            [Required]
            [Display(Name = "File")]
            public List<IFormFile> FormFiles { get; set; }
        }
    }
}
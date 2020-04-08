using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreFileUpload.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<AppFile> AppFiles { get; set; }
    }
}

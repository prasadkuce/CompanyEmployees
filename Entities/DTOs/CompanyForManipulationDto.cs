using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class CompanyForManipulationDto
    {
        [Required(ErrorMessage = "Company Name is required field")]
        [MaxLength(60, ErrorMessage = "Maximum Length for Company name is 60 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Company Address is required field")]
        [MaxLength(120, ErrorMessage = "Maximum Length for Company Address is 120 characters")]
        public string Address { get; set; }
        public string Country { get; set; }

        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }
    }
}

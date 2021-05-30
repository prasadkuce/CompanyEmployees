using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public abstract class EmployeeForManipulationDto
    {
        [Required(ErrorMessage = "Employee name is required field")]
        [MaxLength(30, ErrorMessage = "Maximum Length for Employee name is 30 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Age is required field")]
        [Range(18, int.MaxValue, ErrorMessage = "Age is required and it can't be lower than 18")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Position is required field")]
        [MaxLength(20, ErrorMessage = "Maximum Length for Position is 20 characters")]
        public string Position { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Employee
    {
        [Column("EmployeeId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Employee name is required field")]
        [MaxLength(30, ErrorMessage = "Maximum Length for Employee name is 30 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Age is required field")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Position is required field")]
        [MaxLength(20, ErrorMessage = "Maximum Length for Position is 20 characters")]
        public string Position { get; set; }
        [ForeignKey(nameof(Company))]
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasData(
                new Employee
                {
                    Id = new Guid("d1060972-7e2e-4400-a909-2b7e1523ddf0"),
                    Name = "Sam Raiden",
                    Age = 26,
                    Position = "Software Developer",
                    CompanyId = new Guid("5620fb5a-a862-4586-85d0-54c71f7c8cfc")
                },
                new Employee
                {
                    Id = new Guid("cc9f8ec5-3b7b-4483-bd9d-b5ee20ac0981"),
                    Name = "Jana McLEaf",
                    Age = 30,
                    Position = "Software Developer",
                    CompanyId = new Guid("5620fb5a-a862-4586-85d0-54c71f7c8cfc")
                },
                new Employee
                {
                    Id = new Guid("e5e1b783-34d9-4e61-8f2f-019e3b0fc032"),
                    Name = "Kane Miller",
                    Age = 35,
                    Position = "Administrator",
                    CompanyId = new Guid("b601718d-9ba3-4752-8088-835a30c0457e")
                }
                );
        }
    }
}

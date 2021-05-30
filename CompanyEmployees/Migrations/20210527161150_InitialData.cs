using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompanyEmployees.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[] { new Guid("5620fb5a-a862-4586-85d0-54c71f7c8cfc"), "583 Wall Dr. Gwynn Oak, MD 21207", "USA", "IT_Solutions Ltd" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[] { new Guid("b601718d-9ba3-4752-8088-835a30c0457e"), "312 Forest Avenue, BF 973", "USA", "Admin_Solutions Ltd" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("d1060972-7e2e-4400-a909-2b7e1523ddf0"), 26, new Guid("5620fb5a-a862-4586-85d0-54c71f7c8cfc"), "Sam Raiden", "Software Developer" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("cc9f8ec5-3b7b-4483-bd9d-b5ee20ac0981"), 30, new Guid("5620fb5a-a862-4586-85d0-54c71f7c8cfc"), "Jana McLEaf", "Software Developer" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("e5e1b783-34d9-4e61-8f2f-019e3b0fc032"), 35, new Guid("b601718d-9ba3-4752-8088-835a30c0457e"), "Kane Miller", "Administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("cc9f8ec5-3b7b-4483-bd9d-b5ee20ac0981"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("d1060972-7e2e-4400-a909-2b7e1523ddf0"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("e5e1b783-34d9-4e61-8f2f-019e3b0fc032"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("5620fb5a-a862-4586-85d0-54c71f7c8cfc"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("b601718d-9ba3-4752-8088-835a30c0457e"));
        }
    }
}

using Contracts;
using Entities;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Extensions;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>,IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext):base(repositoryContext)
        {

        }
        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            //var employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                        //.OrderBy(e => e.Name).ToListAsync();
            //return PagedList<Employee>.ToPagedList(employees, employeeParameters.PageNumber, employeeParameters.PageSize);
            //in case getting all employees from db to here takes time, we can get count via separate API call and return paged list
            var employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                .FilterEmployees(employeeParameters.MinAge,employeeParameters.MaxAge)
                .Search(employeeParameters.SearchTerm)
                .ToListAsync();
            var count = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).CountAsync();
            return new PagedList<Employee>(employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);
        }
        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges) => await FindByCondition(e => e.Id.Equals(employeeId) && e.CompanyId.Equals(companyId),trackChanges).SingleOrDefaultAsync();

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee) => Delete(employee);
    }
}

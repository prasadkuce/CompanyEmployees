using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.ActionFilters
{
    public class ValidateEmployeeForCompanyExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;

        public ValidateEmployeeForCompanyExistsAttribute(IRepositoryManager repositoryManager, ILoggerManager logger)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;
            var companyId = (Guid)context.ActionArguments["companyId"];
            var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
            if (company == null)
            {
                _logger.LogInfo($"Comapny with id: {companyId} doesn't exist");
                context.Result = new NotFoundResult();
            }
            var employeeId = (Guid)context.ActionArguments["employeeId"];
            var employee = await _repositoryManager.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges);
            if(employee == null)
            {
                _logger.LogInfo($"Employee with id: {employeeId} doesn't exist");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("employee", employee);
                await next();
            }
        }
    }
}

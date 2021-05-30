﻿using AutoMapper;
using CompanyEmployees.ActionFilters;
using Contracts;
using Entities;
using Entities.DTOs;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeesController(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            if(!employeeParameters.ValidAgeRange)
            {
                return BadRequest("MAX age can't be less than min age");
            }
            var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);
            if(company == null)
            {
                _logger.LogInfo($"Company with id {companyId} does not exist");
                return NotFound();
            }
            var employees = await _repositoryManager.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(employees.MetaData));
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employeesDto);
        }
        [HttpGet("{employeeId}",Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id {companyId} does not exist");
                return NotFound();
            }
            var employee =await _repositoryManager.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges: false);
            if(employee == null)
            {
                _logger.LogInfo($"Employee with id {employeeId} does not exist in the company with id {companyId}");
                return NotFound();
            }
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDto);
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody]EmployeeForCreationDto employee)
        {
            var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: false);
            if(company == null)
            {
                _logger.LogInfo($"Company with id {companyId} doesn't exist");
                return NotFound();
            }
            var employeeEntity = _mapper.Map<Employee>(employee);
            _repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
           await  _repositoryManager.SaveAsync();
            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, employeeId = employeeToReturn.Id }, employeeToReturn);
        }
        [HttpDelete("{employeeId}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            var employeeForCompany = HttpContext.Items["employee"] as Employee;
            _repositoryManager.Employee.DeleteEmployee(employeeForCompany);
            await _repositoryManager.SaveAsync();
            return NoContent();
        }
        [HttpPut("{employeeId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid employeeId, [FromBody] EmployeeForUpdateDto employee) 
        { 
            if (employee == null) 
            { 
                _logger.LogError("EmployeeForUpdateDto object sent from client is null."); 
                return BadRequest("EmployeeForUpdateDto object is null"); 
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Model State for EmployeeForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }
            var employeeEntity = HttpContext.Items["employee"] as Employee;
             _mapper.Map(employee, employeeEntity); 
            await _repositoryManager.SaveAsync(); 
            return NoContent(); 
        }
        [HttpPatch("{employeeId}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid employeeId, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("PatchDoc object sent from client is null.");
                return BadRequest("PatchDoc object is null");
            }
            var employeeEntity = HttpContext.Items["employee"] as Employee;
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
            patchDoc.ApplyTo(employeeToPatch,ModelState);
            TryValidateModel(employeeToPatch);
            if(!ModelState.IsValid)
            {
                _logger.LogError("Invalid Model State for patcg document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repositoryManager.SaveAsync();
            return NoContent();
        }
    }
}

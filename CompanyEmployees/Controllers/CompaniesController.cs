using AutoMapper;
using CompanyEmployees.ActionFilters;
using Contracts;
using Entities;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompaniesController(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _repositoryManager.Company.GetAllCompaniesAsync(trackChanges: false);
            var companiesDto = _mapper.Map <IEnumerable<CompanyDto>>(companies);
            return Ok(companiesDto);
        }
        [HttpGet("{companyId}",Name = "CompanyById")]
        public async Task<IActionResult> GetCompany(Guid companyId)
        {
            var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges:false);
            if(company == null)
            {
                _logger.LogInfo($"Company with Id {companyId} does not exisit");
                return NotFound();
            }
            else
            {
                var companyDto = _mapper.Map<CompanyDto>(company);
                return Ok(companyDto);
            }
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody]CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _repositoryManager.Company.CreateCompany(companyEntity);
            await _repositoryManager.SaveAsync();
            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return CreatedAtRoute("CompanyById", new { companyId = companyToReturn.Id }, companyToReturn);
        }
        [HttpGet("collection/{companyIds}", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> companyIds)
        {
            if(companyIds == null)
            {
                _logger.LogError("Parameter companyIds is null");
                return BadRequest("Parameter companyIds is null");
            }
            var companyEntities = await _repositoryManager.Company.GetByIdsAsync(companyIds, trackChanges: false);
            if(companyIds.Count() != companyEntities.Count())
            {
                _logger.LogError("Some of the companyIds are not valid in the collection");
                return NotFound();
            }
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return Ok(companiesToReturn);
        }
        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection == null) 
            {   
                _logger.LogError("Company collection sent from client is null."); 
                return BadRequest("Company collection is null"); 
            }
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection); 
            foreach (var company in companyEntities) 
            { 
                _repositoryManager.Company.CreateCompany(company); 
            }
            await _repositoryManager.SaveAsync(); 
            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities); 
            var companyIds = string.Join(",", companyCollectionToReturn.Select(c => c.Id)); 
            return CreatedAtRoute("CompanyCollection", new { companyIds }, companyCollectionToReturn);
        }
        [HttpDelete("{companyId}")]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            var company = HttpContext.Items["company"] as Company;
            _repositoryManager.Company.DeleteCompany(company);
            await _repositoryManager.SaveAsync();
            return NoContent();
        }
        [HttpPut("{companyId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid companyId, [FromBody] CompanyForUpdateDto company) 
        { 
            var companyEntity = HttpContext.Items["company"] as Company;
            _mapper.Map(company, companyEntity); 
            await _repositoryManager.SaveAsync(); 
            return NoContent(); 
        }
    }
}

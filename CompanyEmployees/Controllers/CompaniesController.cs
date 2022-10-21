using AutoMapper;
using Contracts.Interfaces;
using Entities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CompaniesController(IRepositoryManager irep, ILoggerManager log, IMapper mapper)
        {
            this._repositoryManager = irep;
            this._logger = log;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            

            var companies = _repositoryManager.Company.GetAllCompanies(false);
            _logger.LogInfo("Succesfully retrieved all the companies...");
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            
            return Ok(companiesDto);
        }
    }
}

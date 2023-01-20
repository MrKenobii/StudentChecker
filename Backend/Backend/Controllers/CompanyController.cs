using AutoMapper;
using Backend.DataTransferObject;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/[controller]")]
public class CompanyController : Controller
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public CompanyController(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CompanyGetResponse>))]
    public IActionResult GetCompanies()
    {
        //var companies = _mapper.Map<List<CompanyDto>>(_companyRepository.GetCompanies());
        var companies = _companyRepository.GetCompanies();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(companies);
    }
    [HttpGet("{companyId}")]
    [ProducesResponseType(200, Type = typeof(CompanyGetResponse))]
    [ProducesResponseType(400)]
    public IActionResult GetCompanyById(int companyId)
    {
        if (!_companyRepository.CompanyExists(companyId))
            return NotFound();
        var company =  _companyRepository.GetCompanyById(companyId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(company);
    }

    [HttpGet("{companyId}/recruiters")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RecruiterDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetRecruitersByCompany(int companyId)
    {
        if (!_companyRepository.CompanyExists(companyId))
            return NotFound();
        
        var recruiterDtos = _companyRepository.GetRecruitersByCompanyId(companyId);
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(recruiterDtos);
    }
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(CompanyDto))]
    public IActionResult CreateCompany([FromBody] CompanyPostRequest companyDto)
    {
        var company = _mapper.Map<Company>(_companyRepository.CreateCompany(companyDto));
        return Created("HttpStatusCode.Created",company);
    }
    [HttpPut("{companyId}")]
    [ProducesResponseType(200, Type = typeof(CompanyDto))]
    public IActionResult UpdateCompany(int companyId, [FromBody] CompanyPostRequest companyDto)
    {
        Console.WriteLine("ID: " + companyId);
        Console.WriteLine("Company: " + companyDto.Name);
        var company = _companyRepository.UpdateCompany(companyId, companyDto);
        return Ok(company);
    }
    
    [HttpDelete("{companyId}")]
    [ProducesResponseType(200)]
    public void DeleteCompany(int companyId)
    {
        _companyRepository.DeleteCompany(companyId);
    }
    [HttpGet("{companyId}/city")]
    [ProducesResponseType(200, Type=typeof(IEnumerable<CityDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetCityByCompanyId(int companyId)
    {
        Console.WriteLine(companyId);
        if (!_companyRepository.CompanyExists(companyId))
            return NotFound();
        var city = _companyRepository.GetCompanyCity(companyId);
        return Ok(city);
    }
    [HttpPut("{companyId}/add-recruiter")]
    [ProducesResponseType(200, Type = typeof(Company))]
    public IActionResult AddCourseStudent(int companyId, [FromBody] AddRecruiterToCompany addRecruiterToCompany)
    {
        var company = _companyRepository.AddRecruiter(companyId, addRecruiterToCompany);
        return Ok(company);
    }
    
}
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExperianTechTest.Dto;
using ExperianTechTest.Exceptions;
using ExperianTechTest.Infrastructure;
using ExperianTechTest.Services;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExperianTechTest.Controllers;

[ApiController]
[Route("[controller]")]
public class FinancialController : Controller
{
    private readonly IFileHelper _fileHelper;
    private readonly IFinancialService _financialService;
    private readonly IValidator<IFormFile> _fileValidator;
    private readonly ILogger<FinancialController> _logger;
    //private readonly IRepository<PersonItem> _repository;

    public FinancialController(IFileHelper fileHelper, IFinancialService financialService,
        IValidator<IFormFile> fileValidator, ILogger<FinancialController> logger)
        //, IRepository<PersonItem> repository)
    {
        _fileHelper = fileHelper;
        _financialService = financialService;
        _fileValidator = fileValidator;
        _logger = logger;
    }

    [HttpPost]
    [EnableCors("MyPolicy")]
    public async Task<IActionResult> Post([FromForm] FileUploadDto fileUploadDto)
    {
        try
        {
            if (fileUploadDto.File == null)
            {
                return BadRequest("File is not supplied.");
            }

            var fileValidationStatus = _fileValidator.Validate(fileUploadDto.File);

            if (!fileValidationStatus.IsValid)
            {
                var errorList = (from error in fileValidationStatus.Errors
                                 select error.ErrorMessage).ToList();
                return BadRequest(errorList);
            }
        

            var strContents = await _fileHelper.ReadFileContentToStringAsync(fileUploadDto.File);
           
            var serviceResponse = _financialService.ParseAndValidate(strContents);

            //_repository.CreateAsync(PersonItem);

            //foreach (var person in serviceResponse.SucessRecords)
            //{
            //    await _repository.CreateAsync(person.ToItem());
            //}

            return Ok(serviceResponse);
        }
        catch(FileInvalidException ex)
        {
            _logger.LogError(ex.ToString());
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
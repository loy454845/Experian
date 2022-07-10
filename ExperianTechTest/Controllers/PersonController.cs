using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExperianTechTest.Dto;
using ExperianTechTest.Services;
using ExperianTechTest.Infrastructure;
using FluentValidation;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace ExperianTechTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IFileHelper _fileHelper;
        private readonly IValidator<IFormFile> _fileValidator;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonService personService, IFileHelper fileHelper
            , IValidator<IFormFile> fileValidator, ILogger<PersonController> logger)
        {
            _personService = personService;
            _fileHelper = fileHelper;
            _fileValidator = fileValidator;
            _logger = logger;
        }

        [HttpPost]
        // GET api/values
        public async Task<ActionResult> Post([FromForm] FileUploadDto fileUploadDto)
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

                var serviceResponse = _personService.ParseAndValidate(strContents);

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

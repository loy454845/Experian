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
using Microsoft.AspNetCore.Cors;
using Microsoft.Azure.CosmosRepository;
using ExperianTechTest.Items;
using ExperianTechTest.Extensions;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;

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
        private readonly IRepository<PersonItem> _repository;

        
        public PersonController(IPersonService personService, IFileHelper fileHelper
            , IValidator<IFormFile> fileValidator, ILogger<PersonController> logger, IRepository<PersonItem> repository)
        {
            _personService = personService;
            _fileHelper = fileHelper;
            _fileValidator = fileValidator;
            _logger = logger;
            _repository = repository;
        }

        [HttpPost]
        [EnableCors("MyPolicy")]
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


                //_repository.CreateAsync(PersonItem);

                foreach (var person in serviceResponse.SucessRecords)
                {
                    await _repository.CreateAsync(person.ToItem());
                }

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetPersonDto request)
        {
           var response = await _repository.TryGetAsync(request.FirstName + ' ' + request.SurName, request.FirstName + ' ' + request.SurName);
            if (response == null)
            {
                return NotFound();
            }
           return Ok(response);
        }

        [HttpGet("WildCardSearch")] 
        public async Task<ActionResult> WildCardSearch([FromQuery] GetPersonDto request)
        {
            try
            {
                IEnumerable<PersonItem> response; 
                if(string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.SurName))
                {
                    response = await _repository.GetAsync(x => x.FullName.Contains(request.FirstName+ request.SurName, StringComparison.InvariantCultureIgnoreCase));
                    if (response == null || !response.Any())
                    {
                        return NotFound();
                    }
                    return Ok(response);
                }
                 response = await _repository.GetAsync(x => x.FirstName.Contains(request.FirstName, StringComparison.InvariantCultureIgnoreCase) && x.Surname.Contains(request.SurName, StringComparison.InvariantCultureIgnoreCase));
                if (response == null || !response.Any())
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch(CosmosException e) when (e.StatusCode is HttpStatusCode.NotFound)
            {
                return NotFound();
            }
        }
    }
}

using System.Threading.Tasks;
using ExperianTechTest.Dto;
using ExperianTechTest.Exceptions;
using ExperianTechTest.Infrastructure;
using ExperianTechTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ExperianTechTest.Controllers;

[ApiController]
[Route("[controller]")]
public class FinancialController : Controller
{
    private readonly IConfiguration _config;
    private readonly IFileHelper _fileHelper;
    private readonly IFinancialService _financialService;

    public FinancialController(IConfiguration config, IFileHelper fileHelper, IFinancialService financialService)
    {
        _config = config;
        _fileHelper = fileHelper;
        _financialService = financialService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] FileUploadDto fileUploadDto)
    {
        var strContents = await _fileHelper.ReadFileContentToStringAsync(fileUploadDto.File);

        try
        {
            var serviceResponse = _financialService.ParseAndValidate(strContents);

            return Ok(serviceResponse);
        }
        catch(FileInvalidException ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}
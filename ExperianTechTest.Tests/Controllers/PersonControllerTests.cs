namespace ExperianTechTest.Tests;

using System.Net;
using ExperianTechTest.Controllers;
using ExperianTechTest.Dto;
using ExperianTechTest.Infrastructure;
using ExperianTechTest.Services;
using ExperianTechTest.Services.ServiceResponse;
using ExperianTechTest.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.Extensions.Logging;

[TestClass]
public class PersonControllerTests
{

    private Mock<IPersonService> _personServiceMock = new Mock<IPersonService>();
    private Mock<IFileHelper> _fileHelperMock = new Mock<IFileHelper>();
    private IConfiguration? _configuration;
    private Mock<ILogger<PersonController>> _logger = new Mock<ILogger<PersonController>>();

    private IValidator<IFormFile>? _fileValidator;

    [TestInitialize]
    public void Initialize()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            {"FileSizeLimitInKB", "2097152"}
        };

        _configuration = new ConfigurationBuilder()
    .AddInMemoryCollection(inMemorySettings)
    .Build();
        _fileValidator = new FileValidator(_configuration);
    }


    [TestMethod]
    public async Task Post_WhenNoFileIsSupplied_ReturnsBadRequest()
    {
        //Arrange
        FileUploadDto personFileDto = new FileUploadDto();
        var sut = new PersonController( _personServiceMock.Object, _fileHelperMock.Object, _fileValidator, _logger.Object);
        var response = await sut.Post(personFileDto);

        //Act
        var result = response as BadRequestObjectResult;

        //Assert
        Assert.AreEqual((int)HttpStatusCode.BadRequest, actual: result?.StatusCode);
    }

    [TestMethod]
    [DataRow("Test.XML")]
    [DataRow("Test.HTML")]
    [DataRow("Test.CSS")]
    [DataRow("Test.JS")]
    [DataRow("Test.PPT")]
    public async Task Post_WhenFileExtensionIsInvalid_ReturnsBadRequest(string filename)
    {
        //Arrange
        var formFileMock = CreateMockFile(filename);
        FileUploadDto personFileDto = new FileUploadDto
        {
            File = formFileMock.Object
        };
        var sut = new PersonController( _personServiceMock.Object, _fileHelperMock.Object, _fileValidator, _logger.Object);

        //Act
        var response = await sut.Post(personFileDto);
        var result = response as BadRequestObjectResult;

        //Assert
        Assert.AreEqual((int)HttpStatusCode.BadRequest, actual: result?.StatusCode);
    }


    [TestMethod]
    [DataRow(2097153)]
    public async Task Post_WhenFileSizeExccedsLimit_ReturnsBadRequest(long fileSize)
    {
        //Arrange
        var formFileMock = CreateMockFile("Test.csv");

        //Act
        formFileMock.Setup(_ => _.Length).Returns(fileSize);
        FileUploadDto personFileDto = new FileUploadDto
        {
            File = formFileMock.Object
        };
        var sut = new PersonController( _personServiceMock.Object, _fileHelperMock.Object, _fileValidator, _logger.Object);
        var response = await sut.Post(personFileDto);
        var result = response as BadRequestObjectResult;

        //Assert
        Assert.AreEqual((int)HttpStatusCode.BadRequest, actual: result?.StatusCode);

    }

    [TestMethod]
    public async Task Post_WhenAllOk_ReturnsSuccess()
    {
        //Arrange
        var formFileMock = CreateMockFile("Test.csv");
        const int validRecords = 1;
        const int invalidRecords = 5;
        const int totalRecords = 6;

        var presonServiceResult = new PersonValidationResult
        {
            ValidRecords = 1,
            ErrorRecords = new Dictionary<int, List<string>>
            {
                {1,new List<string>
                {
                    "Unable to process as the Dob is invalid."
                } },
                {
                    2, new List<string>
                    {
                      "Unable to process as the SurName is invalid.",
                      "Unable to process as the Dob is invalid.",
                      "Unable to process as the Address is invalid.",
                      "Unable to process as the Postcode is invalid.",
                    }
                },
                { 3, new List<string>
                {      "Unable to process as the Dob is invalid." } },

                { 5, new List<string>{
                        "Unable to process as the Dob is invalid."
                } },
                { 6, new List<string>
                {
                    "Unable to process as the FirstName is invalid.",
                      "Unable to process as the Dob is invalid.",
                      "Unable to process as the Address is invalid."
                } }
            }
        };

        _fileHelperMock.Setup(x => x.ReadFileContentToStringAsync(It.IsAny<IFormFile>())).ReturnsAsync("ASDV");
        _personServiceMock.Setup(x => x.ParseAndValidate(It.IsAny<string>())).Returns(presonServiceResult);

        //Act
        FileUploadDto personFileDto = new FileUploadDto
        {
            File = formFileMock.Object
        };
        var sut = new PersonController(_personServiceMock.Object, _fileHelperMock.Object, _fileValidator, _logger.Object);
        var response = await sut.Post(personFileDto);
        var result = response as OkObjectResult;
        var personValidation = result?.Value as PersonValidationResult;

        //Assert
        Assert.AreEqual((int)HttpStatusCode.OK, actual: result?.StatusCode);
        Assert.AreEqual(validRecords, personValidation?.ValidRecords);
        Assert.AreEqual(invalidRecords, personValidation?.InvalidRecords);
        Assert.AreEqual(totalRecords, personValidation?.TotalRecordCount);

    }

   
    [TestMethod]
    public async Task Post_WhenAnException_ReturnInternalServerError()
    {
        //Arrange
         var formFileMock = CreateMockFile("Test.csv");
        const int expectedStatusCode = (int)HttpStatusCode.InternalServerError;
        FileUploadDto personFileDto = new FileUploadDto
        {
            File = formFileMock.Object
        };
        //Act

        _fileHelperMock.Setup(x => x.ReadFileContentToStringAsync(It.IsAny<IFormFile>())).ThrowsAsync(new Exception());

        //Assert
        var sut = new PersonController(_personServiceMock.Object, _fileHelperMock.Object, _fileValidator, _logger.Object);
        var response = await sut.Post(personFileDto);

         var result = response as StatusCodeResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(expectedStatusCode, result.StatusCode);

        _personServiceMock.Verify(x => x.ParseAndValidate(It.IsAny<string>()), Times.Never);
        
    }

    private Mock<IFormFile> CreateMockFile(string name)
    {
        Mock<IFormFile> fileMock = new Mock<IFormFile>();
        var content = @"Id,FirstName,Surname,Dob,Address,Postcode
                        1,John,Smith,23 / 09 / 1980,'15, Station Road, Cambridge','CB3 5RR'
                        2,John,,23 / 09 / 1980,,
                        3,Jane,Smith,2 / 5 / 1984,'15, Station Road, Cambridge','CB3 5RR'
                        4,Anshika,Mandal,1965 - 03 - 03,'512, London Road, Newark-on-Trent, Nottinghamshire','NG23 5RS'
                        5,Alessandro,Romano,12 / 15 / 1992,'22, Cat Walk, Pentrefoelas, Clwyd','LL24 3ED'
                        6,,Romano,15 / 12 / 1992,,'LL24 3ED'";
        var fileName = name;
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;
        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);

        return (fileMock);
    }

}
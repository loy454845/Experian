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
using Microsoft.Azure.CosmosRepository;
using ExperianTechTest.Items;

namespace ExperianTechTest.Tests.Controllers;

[TestClass]
public class FinanceControllerTests
{
    private Mock<IFinancialService> _financeServiceMock = new Mock<IFinancialService>();
    private Mock<IFileHelper> _fileHelperMock = new Mock<IFileHelper>();
    private IConfiguration? _configuration;
    private Mock<ILogger<FinancialController>> _logger = new Mock<ILogger<FinancialController>>();
    //private Mock<IRepository<FinanceItem>> _repository = new Mock<IRepository<FinanceItem>>();
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
        FileUploadDto financialFileDto = new FileUploadDto();
        var sut = new FinancialController(_fileHelperMock.Object, _financeServiceMock.Object, _fileValidator, _logger.Object);
            //, _repository.Object);
        var response = await sut.Post(financialFileDto);

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
        FileUploadDto financeFileDto = new FileUploadDto
        {
            File = formFileMock.Object
        };
        var sut = new FinancialController(_fileHelperMock.Object, _financeServiceMock.Object, _fileValidator, _logger.Object);

        //Act
        var response = await sut.Post(financeFileDto);
        var result = response as BadRequestObjectResult;

        //Assert
        Assert.AreEqual((int)HttpStatusCode.BadRequest, actual: result?.StatusCode);
    }

    [TestMethod]
    [DataRow(2097153)]
    public async Task Post_WhenFileSizeExccedsLimit_ReturnsBadRequest(long fileSize)
    {
        //Arrange
        var formFileMock = CreateMockFile("TestFinanceFile.csv");

        //Act
        formFileMock.Setup(_ => _.Length).Returns(fileSize);
        FileUploadDto financeFileDto = new FileUploadDto
        {
            File = formFileMock.Object
        };
        var sut = new FinancialController(_fileHelperMock.Object, _financeServiceMock.Object, _fileValidator, _logger.Object);
        var response = await sut.Post(financeFileDto);
        var result = response as BadRequestObjectResult;

        //Assert
        Assert.AreEqual((int)HttpStatusCode.BadRequest, actual: result?.StatusCode);

    }

    [TestMethod]
    public async Task Post_WhenAllOk_ReturnsSuccess()
    {
        //Arrange
        var formFileMock = CreateMockFile("TestFinanceFile.csv");
        const int validRecords = 1;
        const int invalidRecords = 2;
        const int totalRecords = 3;

        var financeServiceResult = new FinancialValidationResult
        {
            ValidRecords = 1,
            ErrorRecords = new Dictionary<int, List<string>>
            {
                {
                    11,new List<string>
                    {
                    "Unable to process as the FirstName is invalid."
                    }
                },
                {
                    12, new List<string>
                    {
                      "Unable to process as the Dob is invalid.",
                    }
                }
            }
        };

        _fileHelperMock.Setup(x => x.ReadFileContentToStringAsync(It.IsAny<IFormFile>())).ReturnsAsync("ASDV");
        _financeServiceMock.Setup(x => x.ParseAndValidate(It.IsAny<string>())).Returns(financeServiceResult);

        //Act
        FileUploadDto financeFileDto = new FileUploadDto
        {
            File = formFileMock.Object
        };
        var sut = new FinancialController(_fileHelperMock.Object, _financeServiceMock.Object, _fileValidator, _logger.Object);
        var response = await sut.Post(financeFileDto);
        var result = response as OkObjectResult;
        var financeValidation = result?.Value as FinancialValidationResult;

        //Assert
        Assert.AreEqual((int)HttpStatusCode.OK, actual: result?.StatusCode);
        Assert.AreEqual(validRecords, financeValidation?.ValidRecords);
        Assert.AreEqual(invalidRecords, financeValidation?.InvalidRecords);
        Assert.AreEqual(totalRecords, financeValidation?.TotalRecordCount);

    }



    private Mock<IFormFile> CreateMockFile(string name)
    {
        Mock<IFormFile> fileMock = new Mock<IFormFile>();
        var content = @"Id,FirstName,Surname,Dob,Postcode,AccountType,InitialAmount,TotalPaymentAmount,RepaymentAmount,RemainingAmount,TransactionDate,MinimumPaymentAmount,InterestRate,InitialTerm,RemainingTerm,Status
                        1,John,Smith,23/09/1980,'CB3 5RR',Mortgage,190000,,,,12/07/2021,960,3.1,240,240,InitialPurchase
                        2,John,Smith,23/09/1980,'CB3 5RR',Mortgage,190000,960,780,189220,12/08/2021,960,3.1,240,239,OK
                        3,John,Smith,23/09/1980,'CB3 5RR',Mortgage,190000,960,780,188440,12/09/2021,960,3.1,240,238,OK
                        4,John,Smith,23/09/1980,'CB3 5RR',Mortgage,190000,960,780,187660,12/10/2021,960,3.1,240,237,OK
                        5,John,Smith,23/09/1980,'CB3 5RR',Mortgage,190000,960,780,186880,12/11/2021,960,3.1,240,236,OK
                        6,John,Smith,23/09/1980,'CB3 5RR',Mortgage,190000,960,780,186100,12/12/2021,960,3.1,240,235,OK
                        7,John,Smith,23/09/1980,'CB3 5RR',Mortgage,190000,960,780,185320,12/01/2022,960,3.1,240,234,OK
                        8,John,Smith,23/09/1980,'CB3 5RR',Mortgage,190000,960,780,184540,12/02/2022,960,3.1,240,233,OK
                        9,John,Smith,23/09/1980,'CB3 5RR',Mortgage,190000,960,780,183760,12/03/2022,960,3.1,240,232,OK
                        10,John,Smith,23/09/1980,'CB3 5RR',Mortgage,190000,960,780,182980,12/04/2022,960,3.1,240,231,OK
                        11,,Smith,23/09/1980,'CB3 5RR',Mortgage,190000,960,780,184540,12/02/2022,960,3.1,240,233,OK
                        12,John,Smith,23/09/80,'CB3 5RR',CreditCard,250,,,,03/03/2022,5.00,28.4,,,InitialPurchase";
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



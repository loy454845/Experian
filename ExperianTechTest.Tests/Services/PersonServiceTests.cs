using ExperianTechTest.Models;
using ExperianTechTest.Services;
using ExperianTechTest.Services.ServiceResponse;
using ExperianTechTest.Validators;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExperianTechTest.Tests.Services
{
    [TestClass]
    public class PersonServiceTests
    {
       // private Mock<IPersonService> _personServiceMock = new Mock<IPersonService>();
        private readonly Mock<ICsvToObject<Person>> _csvToPersonMock;
        private readonly IValidator<Person> _personValidator;
        private readonly Mock<ILogger<PersonService>> _loggerMock;

        public PersonServiceTests()
        {
            _csvToPersonMock = new Mock<ICsvToObject<Person>>();
            _personValidator = new PersonValidator();
            _loggerMock = new Mock<ILogger<PersonService>>();
        }

        [TestMethod]
        public void PersonValidationResult_WhenListIsNotValid_ReturnFailure()
        {

            var newPersonList = CreatePersonList();
            _csvToPersonMock.Setup(x => x.GetRecords(It.IsAny<string>())).Returns(newPersonList);

            var personService = new PersonService(_csvToPersonMock.Object, _personValidator, _loggerMock.Object);
            PersonValidationResult result = personService.ParseAndValidate(content);

            //Expected = 3, Valid Records = 1, Error Records = 2
            Assert.AreEqual(3, result.TotalRecordCount);
            Assert.AreEqual(1,result.ValidRecords);
            Assert.AreEqual(2, result.ErrorRecords.Count);
            _csvToPersonMock.Verify(x => x.GetRecords(It.IsAny<string>()), Times.Once);

        }


        private List<Person> CreatePersonList()
        {
            return new List<Person> { new Person {
                Id=1,
                FirstName = "John",
                Surname= "Smith",
                Dob = "23/09/1980",
                Address = "15, Station Road, Cambridge",
                Postcode= "CB3 5RR"
            },new Person {
                Id=2,
                FirstName = "John",
                Surname= "Smith",
                Dob = "23 / 09 / 1980"
            },new Person {
                Id=3,
                FirstName = "John",
                Surname= "Smith",
                Dob = "23 / 09 / 1980",
                Address = "15, Station Road, Cambridge",
                Postcode= ""
            } };
        }

        private string content = @"Id,FirstName,Surname,Dob,Address,Postcode
                        1,John,Smith,23 / 09 / 1980,'15, Station Road, Cambridge','CB3 5RR'
                        2,John,,23 / 09 / 1980,,
                        3,Jane,Smith,2 / 5 / 1984,'15, Station Road, Cambridge','CB3 5RR'
                        4,Anshika,Mandal,1965 - 03 - 03,'512, London Road, Newark-on-Trent, Nottinghamshire','NG23 5RS'
                        5,Alessandro,Romano,12 / 15 / 1992,'22, Cat Walk, Pentrefoelas, Clwyd','LL24 3ED'
                        6,,Romano,15 / 12 / 1992,,'LL24 3ED'";
    }
}

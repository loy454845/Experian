using ExperianTechTest.Validators;
using ExperianTechTest.Models;

namespace ExperianTechTest.Tests.Validators
{
    [TestClass]
    public class PersonValidatorTest
    {
        public PersonValidatorTest()
        {
        }
        [TestMethod]   
        public void PersonValidator_WhenIdIsEmpty_ReturnsError()
        {
            //Arrange
            var person = new Person{
                //Id = 2,
               Address = "Abcd",
                FirstName="Tom",
                Surname = "David",
                Dob="12/03/2011",
                 Postcode="NF12 5RW"
            };
            const string ExpectedErrorMessage = "Unable to process as the Id is negative or zero.";

            PersonValidator personValidator = new PersonValidator();

            //Act
            var result = personValidator.Validate(person);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1,result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }
    }
}
//("Unable to process as the Id is null");
//RuleFor(x => x.FirstName).NotEmpty().WithMessage("Unable to process as the FirstName is invalid.");
//RuleFor(x => x.Surname).NotEmpty().WithMessage("Unable to process as the SurName is invalid.");
//RuleFor(x => x.Dob).NotEmpty().Must(x => IsCorrectDateFormat(x)).WithMessage("Unable to process as the Dob is invalid.");
//RuleFor(x => x.Address).NotEmpty().WithMessage("Unable to process as the Address is invalid.");
//RuleFor(x => x.Postcode).NotEmpty().WithMessage("Unable to process as the Postcode is invalid.");
//    }
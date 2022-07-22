using ExperianTechTest.Validators;
using ExperianTechTest.Models;

namespace ExperianTechTest.Tests.Validators
{
    [TestClass]
    public class PersonValidatorTest
    {
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

        [TestMethod]
        public void PersonValidator_WhenFirstNameisInvalid_ReturnsError()
        {
            //Arrange
            var person = new Person
            {
                Id = 1,
                Address = "Abcd",
                FirstName = "",
                Surname = "David",
                Dob = "12/03/2011",
                Postcode = "NF12 5RW"
            };
            const string ExpectedErrorMessage = "Unable to process as the FirstName is invalid.";

            PersonValidator personValidator = new PersonValidator();

            //Act
            var result = personValidator.Validate(person);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PersonValidator_WhenSurNameisInvalid_ReturnsError()
        {
            //Arrange
            var person = new Person
            {
                Id = 1,
                Address = "Abcd",
                FirstName = "John",
                Surname = "",
                Dob = "12/03/2011",
                Postcode = "NF12 5RW"
            };
            const string ExpectedErrorMessage = "Unable to process as the SurName is invalid.";

            PersonValidator personValidator = new PersonValidator();

            //Act
            var result = personValidator.Validate(person);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PersonValidator_WhenDobIsInvalid_ReturnsError()
        {
            //Arrange
            var person = new Person
            {
                Id = 1,
                Address = "Abcd",
                FirstName = "John",
                Surname = "David",
                Dob = "23-13-2011",
                Postcode = "NF12 5RW"
            };
            const string ExpectedErrorMessage = "Unable to process as the Dob is invalid.";

            PersonValidator personValidator = new PersonValidator();

            //Act
            var result = personValidator.Validate(person);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PersonValidator_WhenAddressIsInvalid_ReturnsError()
        {
            //Arrange
            var person = new Person
            {
                Id = 1,
                Address = "",
                FirstName = "John",
                Surname = "David",
                Dob = "12/04/2021",
                Postcode = "NF12 5RW"
            };
            const string ExpectedErrorMessage = "Unable to process as the Address is invalid.";

            PersonValidator personValidator = new PersonValidator();

            //Act
            var result = personValidator.Validate(person);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PersonValidator_WhenPostCodeIsInvalid_ReturnsError()
        {
            //Arrange
            var person = new Person
            {
                Id = 1,
                Address = "Abcd",
                FirstName = "John",
                Surname = "David",
                Dob = "12/04/2021",
                Postcode = "000"
            };
            const string ExpectedErrorMessage = "Unable to process as the Postcode is invalid.";

            PersonValidator personValidator = new PersonValidator();

            //Act
            var result = personValidator.Validate(person);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }
    }
}

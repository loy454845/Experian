using System;
using ExperianTechTest.Models;
using ExperianTechTest.Validators;

namespace ExperianTechTest.Tests.Validators
{
    [TestClass]
    public class FinanceValidatorTest
    {
        [TestMethod]
        public void PersonValidator_WhenIdIsEmpty_ReturnsError()
        {
            //Arrange
            var finance = new Finance
            {
                //Id = 1,
                FirstName= "John",
                Surname= "Smith",
                Dob= "23/09/1980",
                Postcode= "NF12 5RW",
                AccountType= "Mortgage",
                InitialAmount= "190000",
                TotalPaymentAmount = "5000",
                RepaymentAmount= "780",
                RemainingAmount= "189220",
                TransactionDate = "12/07/2021",
                MinimumPaymentAmount= "960",
                InterestRate= "3.1",
                InitialTerm= "240",
                RemainingTerm= "239",
                Status = "OK"
            };
            const string ExpectedErrorMessage = "Unable to process as the Id is null";

            FinanceValidator financeValidator = new FinanceValidator();

            //Act
            var result = financeValidator.Validate(finance);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PersonValidator_WhenFirstNameIsInvalid_ReturnsError()
        {
            //Arrange
            var finance = new Finance
            {
                Id = 1,
                FirstName = "",
                Surname = "Smith",
                Dob = "23/09/1980",
                Postcode = "NF12 5RW",
                AccountType = "Mortgage",
                InitialAmount = "190000",
                TotalPaymentAmount = "5000",
                RepaymentAmount = "780",
                RemainingAmount = "189220",
                TransactionDate = "12/07/2021",
                MinimumPaymentAmount = "960",
                InterestRate = "3.1",
                InitialTerm = "240",
                RemainingTerm = "239",
                Status = "OK"
            };
            const string ExpectedErrorMessage = "Unable to process as the FirstName is invalid.";

            FinanceValidator financeValidator = new FinanceValidator();

            //Act
            var result = financeValidator.Validate(finance);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PersonValidator_WhenSurNameIsInvalid_ReturnsError()
        {
            //Arrange
            var finance = new Finance
            {
                Id = 1,
                FirstName = "John",
                Surname = "",
                Dob = "23/09/1980",
                Postcode = "NF12 5RW",
                AccountType = "Mortgage",
                InitialAmount = "190000",
                TotalPaymentAmount = "5000",
                RepaymentAmount = "780",
                RemainingAmount = "189220",
                TransactionDate = "12/07/2021",
                MinimumPaymentAmount = "960",
                InterestRate = "3.1",
                InitialTerm = "240",
                RemainingTerm = "239",
                Status = "OK"
            };
            const string ExpectedErrorMessage = "Unable to process as the SurName is invalid.";

            FinanceValidator financeValidator = new FinanceValidator();

            //Act
            var result = financeValidator.Validate(finance);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PersonValidator_WhenDobIsInvalid_ReturnsError()
        {
            //Arrange
            var finance = new Finance
            {
                Id = 1,
                FirstName = "John",
                Surname = "David",
                Dob = "2-09-1980",
                Postcode = "NF12 5RW",
                AccountType = "Mortgage",
                InitialAmount = "190000",
                TotalPaymentAmount = "5000",
                RepaymentAmount = "780",
                RemainingAmount = "189220",
                TransactionDate = "12/07/2021",
                MinimumPaymentAmount = "960",
                InterestRate = "3.1",
                InitialTerm = "240",
                RemainingTerm = "239",
                Status = "OK"
            };
            const string ExpectedErrorMessage = "Unable to process as the Dob is invalid.";

            FinanceValidator financeValidator = new FinanceValidator();

            //Act
            var result = financeValidator.Validate(finance);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PersonValidator_WhenPostcodeIsInvalid_ReturnsError()
        {
            //Arrange
            var finance = new Finance
            {
                Id = 1,
                FirstName = "John",
                Surname = "David",
                Dob = "12/09/1980",
                Postcode = "0RW",
                AccountType = "Mortgage",
                InitialAmount = "190000",
                TotalPaymentAmount = "5000",
                RepaymentAmount = "780",
                RemainingAmount = "189220",
                TransactionDate = "12/07/2021",
                MinimumPaymentAmount = "960",
                InterestRate = "3.1",
                InitialTerm = "240",
                RemainingTerm = "239",
                Status = "OK"
            };
            const string ExpectedErrorMessage = "Unable to process as the Postcode is invalid.";

            FinanceValidator financeValidator = new FinanceValidator();

            //Act
            var result = financeValidator.Validate(finance);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PersonValidator_WhenAccountTypeIsInvalid_ReturnsError()
        {
            //Arrange
            var finance = new Finance
            {
                Id = 1,
                FirstName = "John",
                Surname = "David",
                Dob = "12/09/1980",
                Postcode = "NF12 3RW",
                AccountType = "",
                InitialAmount = "190000",
                TotalPaymentAmount = "5000",
                RepaymentAmount = "780",
                RemainingAmount = "189220",
                TransactionDate = "12/07/2021",
                MinimumPaymentAmount = "960",
                InterestRate = "3.1",
                InitialTerm = "240",
                RemainingTerm = "239",
                Status = "OK"
            };
            const string ExpectedErrorMessage = "Unable to process as the AccountType is invalid.";

            FinanceValidator financeValidator = new FinanceValidator();

            //Act
            var result = financeValidator.Validate(finance);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PersonValidator_WhenTransactionDateIsInvalid_ReturnsError()
        {
            //Arrange
            var finance = new Finance
            {
                Id = 1,
                FirstName = "John",
                Surname = "David",
                Dob = "12/09/1980",
                Postcode = "NF12 3RW",
                AccountType = "Mortgage",
                InitialAmount = "180000",
                TotalPaymentAmount = "5000",
                RepaymentAmount = "780",
                RemainingAmount = "189220",
                TransactionDate = "12-07-2021",
                MinimumPaymentAmount = "960",
                InterestRate = "3.1",
                InitialTerm = "240",
                RemainingTerm = "239",
                Status = "OK"
            };
            const string ExpectedErrorMessage = "Unable to process as the TransactionDate is invalid.";

            FinanceValidator financeValidator = new FinanceValidator();

            //Act
            var result = financeValidator.Validate(finance);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PersonValidator_WhenStatusIsInvalid_ReturnsError()
        {
            //Arrange
            var finance = new Finance
            {
                Id = 1,
                FirstName = "John",
                Surname = "David",
                Dob = "12/09/1980",
                Postcode = "NF12 3RW",
                AccountType = "Mortgage",
                InitialAmount = "180000",
                TotalPaymentAmount = "5000",
                RepaymentAmount = "780",
                RemainingAmount = "189220",
                TransactionDate = "12/07/2021",
                MinimumPaymentAmount = "960",
                InterestRate = "3.1",
                InitialTerm = "240",
                RemainingTerm = "239",
                Status = ""
            };
            const string ExpectedErrorMessage = "Unable to process as the Status is invalid.";

            FinanceValidator financeValidator = new FinanceValidator();

            //Act
            var result = financeValidator.Validate(finance);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ExpectedErrorMessage, result.Errors[0].ErrorMessage);
        }
    }
}
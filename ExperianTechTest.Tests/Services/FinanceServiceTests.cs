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
    public class FinanceServiceTests
    {
        private readonly Mock<ICsvToObject<Finance>> _csvToFinanceMock;
        private readonly IValidator<Finance> _financeValidator;
        private readonly Mock<ILogger<FinancialService>> _loggerMock;

        public FinanceServiceTests()
        {
            _csvToFinanceMock = new Mock<ICsvToObject<Finance>>();
            _financeValidator = new FinanceValidator();
            _loggerMock = new Mock<ILogger<FinancialService>>();
        }

        [TestMethod]
        public void FinanceValidationResult_WhenListIsNotValid_ReturnFailure()
        {

            var newFinanceList = CreateFinanceList();
            _csvToFinanceMock.Setup(x => x.GetRecords(It.IsAny<string>())).Returns(newFinanceList);

            var financeService = new FinancialService(_csvToFinanceMock.Object, _financeValidator, _loggerMock.Object);
            FinancialValidationResult result = financeService.ParseAndValidate(content);

            //Expected = 3, Valid Records = 3, Error Records = 0
            Assert.AreEqual(3, result.TotalRecordCount);
            Assert.AreEqual(3, result.ValidRecords);
            Assert.AreEqual(0, result.ErrorRecords.Count);
            _csvToFinanceMock.Verify(x => x.GetRecords(It.IsAny<string>()), Times.Once);

        }


        private List<Finance> CreateFinanceList()
        {
            return new List<Finance> { new Finance {
                Id = 1,
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
            },new Finance {
                Id = 2,
                FirstName= "Ben",
                Surname= "Torc",
                Dob= "23/12/1990",
                Postcode= "PA12 5RW",
                AccountType= "Mortgage",
                InitialAmount= "70000",
                TotalPaymentAmount = "8600",
                RepaymentAmount= "580",
                RemainingAmount= "89220",
                TransactionDate = "12/07/2022",
                MinimumPaymentAmount= "960",
                InterestRate= "3.1",
                InitialTerm= "240",
                RemainingTerm= "239",
                Status = "OK"
            },new Finance {
                Id = 3,
                FirstName= "David",
                Surname= "Loan",
                Dob= "13/09/1990",
                Postcode= "NF02 5PW",
                AccountType= "Mortgage",
                InitialAmount= "290000",
                TotalPaymentAmount = "10000",
                RepaymentAmount= "1780",
                RemainingAmount= "289220",
                TransactionDate = "10/07/2020",
                MinimumPaymentAmount= "960",
                InterestRate= "5.1",
                InitialTerm= "240",
                RemainingTerm= "239",
                Status = "OK"
            } };
        }

        private string content = @"Id,FirstName,Surname,Dob,Postcode,AccountType,InitialAmount,TotalPaymentAmount,RepaymentAmount,RemainingAmount,TransactionDate,MinimumPaymentAmount,InterestRate,InitialTerm,RemainingTerm,Status
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
    }
}


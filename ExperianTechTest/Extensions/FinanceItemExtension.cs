
using ExperianTechTest.Items;
using ExperianTechTest.Models;

namespace ExperianTechTest.Extensions
{
    public static class FinanceItemExtension
    {
        public static FinanceItem ToItem(this Finance finance)
        {
            return new FinanceItem
            {
                FullName = finance.FirstName + ' ' + finance.Surname,
                FinanceId = finance.Id,
                FirstName = finance.FirstName,
                Surname = finance.Surname,
                Dob = finance.Dob,
                Postcode = finance.Postcode,
                AccountType = finance.AccountType,
                InitialAmount = finance.InitialAmount,
                TotalPaymentAmount = finance.TotalPaymentAmount,
                RepaymentAmount = finance.RepaymentAmount,
                RemainingAmount = finance.RemainingAmount,
                TransactionDate = finance.TransactionDate,
                MinimumPaymentAmount = finance.MinimumPaymentAmount,
                InterestRate = finance.InterestRate,
                InitialTerm = finance.InitialTerm,
                RemainingTerm = finance.RemainingTerm,
                Status = finance.Status
            };
        }
    }
}


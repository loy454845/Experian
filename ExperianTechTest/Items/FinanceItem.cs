using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Attributes;
using Newtonsoft.Json;

namespace ExperianTechTest.Items
{
    [PartitionKeyPath("/fullName")]
    public class FinanceItem : Item
    {
        public int FinanceId { get; set; }
        public string FullName {get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Dob { get; set; }
        public string Postcode { get; set; }
        public string AccountType { get; set; }

        public string InitialAmount { get; set; }
        public string TotalPaymentAmount { get; set; }
        public string RepaymentAmount { get; set; }
        public string RemainingAmount { get; set; }
        public string TransactionDate { get; set; }

        public string MinimumPaymentAmount { get; set; }
        public string InterestRate { get; set; }
        public string InitialTerm { get; set; }
        public string RemainingTerm { get; set; }
        public string Status { get; set; }

        protected override string GetPartitionKeyValue() => FullName;
    }
}
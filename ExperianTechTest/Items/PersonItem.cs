using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Attributes;
using Newtonsoft.Json;

namespace ExperianTechTest.Items
{
    [PartitionKeyPath("/fullName")]
    public class PersonItem : Item
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Dob { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }

        protected override string GetPartitionKeyValue() => FullName;
    }
}


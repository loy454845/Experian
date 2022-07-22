using ExperianTechTest.Items;
using ExperianTechTest.Models;

namespace ExperianTechTest.Extensions
{
    public static class PersonItemExtension
    {
        public static PersonItem ToItem(this Person person)
        {
            return new PersonItem
            {
                Id = person.FirstName+' '+person.Surname,
                PersonId = person.Id,
                FullName = person.FirstName + ' ' + person.Surname,
                FirstName = person.FirstName,
                Surname = person.Surname,
                Dob = person.Dob,
                Address = person.Address,
                Postcode = person.Postcode
            };
        }
    }
}


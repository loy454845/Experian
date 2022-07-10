using System.Collections.Generic;
using ExperianTechTest.Services.ServiceResponse;
using FluentValidation;
using System.Linq;
using Microsoft.Extensions.Logging;
using ExperianTechTest.Models;

namespace ExperianTechTest.Services
{
    public class PersonService : IPersonService
    {
        private readonly ICsvToObject<Person> _csvToPerson;
        private readonly IValidator<Person> _personValidator;
        private readonly ILogger<PersonService> _logger;
        
        public PersonService(ICsvToObject<Person> csvToPerson, IValidator<Person> personValidator, ILogger<PersonService> logger)
        {
            _csvToPerson = csvToPerson;
            _personValidator = personValidator;
            _logger = logger;
        }

        public PersonValidationResult ParseAndValidate(string persons)
        {
            var lstPersons = _csvToPerson.GetRecords(persons);
            List<Person> validRecords = new List<Person>();
            Dictionary<int, List<string>> errorRecords = new Dictionary<int, List<string>>();
            foreach (Person person in lstPersons)
            {
                var res = _personValidator.Validate(person);
                if (res.IsValid)
                {
                    validRecords.Add(person);
                    continue;
                }
                var errorMessage = res.Errors;
                var errorMessages = (from item in errorMessage
                                     select item.ErrorMessage).ToList();
                errorRecords.Add(person.Id, value: errorMessages);
                _logger.LogInformation($"Person with {person.Id} is not a valid record.");
            }

            return new PersonValidationResult
            {
                ValidRecords = validRecords.Count,
                ErrorRecords = errorRecords
            };
        }

    }
}
using System.Collections.Generic;
using ExperianTechTest.Models;

namespace ExperianTechTest.Services.ServiceResponse
{
    public class PersonValidationResult
    {
        public PersonValidationResult()
        {
            ErrorRecords = new Dictionary<int, List<string>>();
        }

        public int TotalRecordCount
        {
            get
            {
                return ValidRecords + InvalidRecords;
            }
        }

        public int ValidRecords { get; set; }
        
        public int InvalidRecords
        {
            get
            {
                return ErrorRecords.Count;
            }
        }

        public Dictionary<int, List<string>> ErrorRecords { get; set; }

        public List<Person> SucessRecords { get; set; }
    }
}
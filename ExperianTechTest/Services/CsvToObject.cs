using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace ExperianTechTest.Services
{
    public class CsvToObject<T> : ICsvToObject<T> where T : class
    {
        public IEnumerable<T> GetRecords(string strCsv)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };
            var textReader = new StringReader(strCsv);
            var csvr = new CsvReader(textReader, config);
            // var csvr = new CsvReader(textReader, System.Globalization.CultureInfo.CurrentCulture);
            var records = csvr.GetRecords<T>();
            
            return records;
        }
    }
}
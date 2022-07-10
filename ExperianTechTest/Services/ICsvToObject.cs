using System.Collections.Generic;

namespace ExperianTechTest.Services
{
    public interface ICsvToObject<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        IEnumerable<T> GetRecords(string csv);
    }
}
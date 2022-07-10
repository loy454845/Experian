using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ExperianTechTest.Infrastructure
{
    public class FileHelper: IFileHelper
    {
        public async Task<string> ReadFileContentToStringAsync(IFormFile file)
        {

            var result = new StringBuilder();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }
            return result.ToString();
        }
    }
}


using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ExperianTechTest.Infrastructure
{
    public interface IFileHelper
    {
        Task<string> ReadFileContentToStringAsync(IFormFile file);
    }
}


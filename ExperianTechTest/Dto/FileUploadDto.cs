using Microsoft.AspNetCore.Http;

namespace ExperianTechTest.Dto;

public class FileUploadDto
{
    public IFormFile File { get; set; }
}


using System.IO;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ExperianTechTest.Validators;

public class FileValidator : AbstractValidator<IFormFile?>
{
    private readonly IConfiguration _configuration;

    public FileValidator(IConfiguration configuration)
    {
        _configuration = configuration;
        //ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x).NotNull().NotEmpty().WithMessage("Unable to process as the File is not valid.");
        RuleFor(x => x.Length).NotNull().GreaterThanOrEqualTo(0).WithMessage("Unable to process as the File Length is not valid.");
        RuleFor(x => x).NotEmpty().Must(x => IsFileExtensionValid(x)).WithMessage("Unable to process as the File extension is not valid.");
        RuleFor(x => x).NotEmpty().Must(x => IsFileSizeLimitValidation(x)).WithMessage("Unable to process as the File size is not valid.");
    }

    private bool IsFileExtensionValid(IFormFile file)
    {
        var ext = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(ext) || ext != ".csv")
        {
            return false;
        }
        return true;
    }

    private bool IsFileSizeLimitValidation(IFormFile file)
    {
        var fileSizeLimit = _configuration.GetValue<long>("FileSizeLimitInKB");

        if (file.Length > fileSizeLimit || file.Length <= 0)
        {
            return false;
        }
        return true;
    }

}
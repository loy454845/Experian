using ExperianTechTest.Services.ServiceResponse;

namespace ExperianTechTest.Services;

public interface IPersonService
{
    public PersonValidationResult ParseAndValidate(string persons);
}
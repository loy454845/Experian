using ExperianTechTest.Services.ServiceResponse;

namespace ExperianTechTest.Services;

public interface IFinancialService
{
    public FinancialValidationResult ParseAndValidate(string financial);
}


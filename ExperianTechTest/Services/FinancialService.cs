using System.Collections.Generic;
using System.Linq;
using ExperianTechTest.Exceptions;
using ExperianTechTest.Models;
using ExperianTechTest.Services.ServiceResponse;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ExperianTechTest.Services;

public class FinancialService : IFinancialService
{
    private readonly ICsvToObject<Finance> _csvToFinance;
    private readonly IValidator<Finance> _financeValidator;
    private readonly ILogger<FinancialService> _logger;

    public FinancialService(ICsvToObject<Finance> csvToFinance, IValidator<Finance> financeValidator, ILogger<FinancialService> logger)
    {
        _csvToFinance = csvToFinance;
        _financeValidator = financeValidator;
        _logger = logger;
    }

    public FinancialValidationResult ParseAndValidate(string financials)
    {
        var lstFinance = _csvToFinance.GetRecords(financials);
        List<Finance> financeValidRecords = new List<Finance>();
        Dictionary<int, List<string>> errorRecords = new Dictionary<int, List<string>>();

        if (lstFinance != null)
        {
            foreach (Finance finance in lstFinance)
            {
                var res = _financeValidator.Validate(finance);
                if (res.IsValid)
                {
                    financeValidRecords.Add(finance);
                    continue;
                }
                var errorMessage = res.Errors;
                var errorMessages = (from item in errorMessage
                                     select item.ErrorMessage).ToList();
                errorRecords.Add(finance.Id, value: errorMessages);
                _logger.LogInformation($"Finance with Id: {finance.Id} is not a valid record.");

            }
            return new FinancialValidationResult
            {
                ValidRecords = financeValidRecords.Count,
                ErrorRecords = errorRecords
            };
        }
        throw new FileInvalidException();
    }
}


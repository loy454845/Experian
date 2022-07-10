using System;
using System.Globalization;
using ExperianTechTest.Models;
using FluentValidation;

namespace ExperianTechTest.Validators;

public class FinanceValidator : AbstractValidator<Finance>
{
    public FinanceValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Unable to process as the Id is null");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Unable to process as the FirstName is invalid.");
        RuleFor(x => x.Surname).NotEmpty().WithMessage("Unable to process as the SurName is invalid.");
        RuleFor(x => x.Dob).NotEmpty().Must(x => IsCorrectDateFormat(x)).WithMessage("Unable to process as the Dob is invalid.");
        RuleFor(x => x.Postcode).NotEmpty().WithMessage("Unable to process as the Postcode is invalid.");

        RuleFor(x => x.AccountType).NotEmpty().WithMessage("Unable to process as the AccountType is invalid.");
        Transform(from: x => x.InitialAmount, to: value => double.TryParse(value, out double val) ? (double?)val : null)
            .GreaterThan(0).WithMessage("Unable to process as the InitialAmount is invalid.");
        Transform(from: x => x.TotalPaymentAmount, to: value => double.TryParse(value, out double val) ? (double?)val : null)
           .GreaterThan(0).WithMessage("Unable to process as the TotalPaymentAmount is invalid.");
        Transform(from: x => x.RepaymentAmount, to: value => double.TryParse(value, out double val) ? (double?)val : null)
           .GreaterThan(0).WithMessage("Unable to process as the RepaymentAmount is invalid.");
        Transform(from: x => x.RemainingAmount, to: value => double.TryParse(value, out double val) ? (double?)val : null)
           .GreaterThan(0).WithMessage("Unable to process as the RemainingAmount is invalid.");
        Transform(from: x => x.MinimumPaymentAmount, to: value => double.TryParse(value, out double val) ? (double?)val : null)
           .GreaterThan(0).WithMessage("Unable to process as the MinimumPaymentAmount is invalid.");
        Transform(from: x => x.InterestRate, to: value => double.TryParse(value, out double val) ? (double?)val : null)
           .GreaterThan(0).WithMessage("Unable to process as the InterestRate is invalid.");
        Transform(from: x => x.InitialTerm, to: value => double.TryParse(value, out double val) ? (double?)val : null)
           .GreaterThan(0).WithMessage("Unable to process as the InitialTerm is invalid.");
        Transform(from: x => x.RemainingTerm, to: value => double.TryParse(value, out double val) ? (double?)val : null)
           .GreaterThan(0).WithMessage("Unable to process as the RemainingTerm is invalid.");

        RuleFor(x => x.TransactionDate).NotEmpty().Must(x => IsCorrectDateFormat(x)).WithMessage("Unable to process as the TransactionDate is invalid.");

        RuleFor(x => x.Status).NotEmpty().WithMessage("Unable to process as the Status is invalid.");
    }

    private bool IsCorrectDateFormat(string format) => DateTime.TryParseExact(format, "dd/mm/yyyy", null, DateTimeStyles.None, out _);

}
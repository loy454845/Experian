using System;
using System.Globalization;
using ExperianTechTest.Models;
using FluentValidation;


namespace ExperianTechTest.Validators;
public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(x => x.Id).GreaterThanOrEqualTo(1).WithMessage("Unable to process as the Id is negative or zero.");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Unable to process as the FirstName is invalid.");
        RuleFor(x => x.Surname).NotEmpty().WithMessage("Unable to process as the SurName is invalid.");
        RuleFor(x => x.Dob).NotEmpty().Must(x => IsCorrectDateFormat(x)).WithMessage("Unable to process as the Dob is invalid.");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Unable to process as the Address is invalid.");
        RuleFor(x => x.Postcode).NotEmpty().WithMessage("Unable to process as the Postcode is invalid.");
    }

    private bool IsCorrectDateFormat(string format) => DateTime.TryParseExact(format, "dd/mm/yyyy", null, DateTimeStyles.None, out _);

}
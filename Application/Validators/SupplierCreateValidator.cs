using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
  public class SupplierCreateValidator : AbstractValidator<SupplierCreateDto>
  {
    public SupplierCreateValidator()
    {
      RuleFor(x => x.SupplierName).NotEmpty().WithMessage("SupplierName is required.");
      RuleFor(x => x.ContactEmail).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));
    }
  }
}

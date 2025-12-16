using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
  public class CreateProductValidator : AbstractValidator<CreateProductRequest>
  {
    public CreateProductValidator()
    {
      RuleFor(x => x.ProductName).NotEmpty();
      RuleFor(x => x.SellPrice).GreaterThanOrEqualTo(0);
      RuleFor(x => x.CostPrice).GreaterThanOrEqualTo(0);
    }
  }
}

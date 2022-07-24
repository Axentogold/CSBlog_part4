using API.Contracts.Tags;
using FluentValidation;

namespace API.Validation;

public class AddTagRequestValidator : AbstractValidator<AddTagRequest>
{
  public AddTagRequestValidator()
  {
    RuleFor(x => x.TagName)
      .NotEmpty()
      .Matches(@"^\w+$")
      .MinimumLength(2)
      .MaximumLength(20)
      .WithMessage("Special characters are not allowed");
  }
}
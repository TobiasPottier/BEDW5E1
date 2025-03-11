namespace Sneakers.API.Validitors;

public class SneakerValidator : AbstractValidator<AddSneakerDTO>
{
    public SneakerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Sneaker name is mandatory.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        RuleFor(x => x.BrandId).NotNull().WithMessage("BrandId is mandatory.");
        RuleFor(x => x.OccasionIds).NotNull().Must(x => x.Any()).WithMessage("At least one OccasionIds must be provided.");
    }
}
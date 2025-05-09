using FluentValidation;

namespace OrderManagementSystem.Entities.FluentValidation;

using FluentValidation;
using OrderManagementSystem.Entities;
//najpierw komenda na dodanie biblioteki
//dotnet add package FluentValidation.AspNetCore



// NAJWAZNIEJSZE ZE WALIDACJA POWINNA BYĆ POD DTO
public class OrderValidator : AbstractValidator<Order>
{ //fluent validation jest bardziej elastyczna niz np data atrybutes
    public OrderValidator()
    {
        RuleFor(o => o.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Length(2, 50).WithMessage("Status must be between 2 and 50 characters.");

        RuleFor(o => o.TotalAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Total amount must be >= 0.");

        RuleFor(o => o.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than 0.");
    }
    
    //builder.Services.AddControllers()
    //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserDtoValidator>());
    
    
    
}

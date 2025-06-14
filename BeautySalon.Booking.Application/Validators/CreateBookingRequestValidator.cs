using BeautySalon.Booking.Application.DTO.Booking;

namespace BeautySalon.Booking.Application.Validators;

using FluentValidation;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.ServiceId).NotEmpty();

        RuleFor(x => x.StartTime)
            .Must(start => start > DateTime.UtcNow)
            .WithMessage("Дата начала должна быть в будущем");

        RuleFor(x => x.Duration)
            .Must(duration => duration >= TimeSpan.FromMinutes(30) && duration <= TimeSpan.FromHours(3))
            .WithMessage("Длительность должна быть от 30 минут до 3 часов");
    }
}

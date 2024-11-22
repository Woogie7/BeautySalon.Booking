using AutoMapper;
using BeautySalon.Booking.Application.DTO;
using BeautySalon.Booking.Application.Features.Booking.CreateBooking;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;

namespace BeautySalon.Booking.Application.AutoMapper
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<CreateBookingRequest, CreateBookingCommand>();

            CreateMap<CreateBookingCommand, CreateBookingRequest>()
                .ForMember(b => b.StartTime, i => i.MapFrom(b => b.StartTime))
                .ForMember(b => b.Duration, i => i.MapFrom(b => b.Duration))
                .ForMember(b => b.Discount, i => i.MapFrom(b => b.Discount))
                .ForMember(b => b.ServiceId, i => i.MapFrom(b => b.ServiceId))
                .ForMember(b => b.EmployeeId, i => i.MapFrom(b => b.EmployeeId));

            CreateMap<Employee, EmployeeDTO>()
                .ForMember(e => e.Id, i => i.MapFrom(e => e.Id.Value))
                .ForMember(e => e.Name, i => i.MapFrom(e => e.Name));

        }
    }
}

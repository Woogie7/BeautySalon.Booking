using AutoMapper;
using BeautySalon.Booking.Application.DTO;
using BeautySalon.Booking.Application.DTO.Booking;
using BeautySalon.Booking.Application.Features.Bookings.CreateBooking;
using BeautySalon.Booking.Application.Models;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Booking.Domain.SeedWork;
using BeautySalon.Domain.AggregatesModel.BookingAggregate;
using BeautySalon.Domain.AggregatesModel.BookingAggregate.ValueObjects;

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
                .ForMember(b => b.ServiceId, i => i.MapFrom(b => b.ServiceId))
                .ForMember(b => b.EmployeeId, i => i.MapFrom(b => b.EmployeeId));

            CreateMap<EmployeeReadModel, EmployeeDTO>()
                .ForMember(e => e.Id, i => i.MapFrom(e => e.Id))
                .ForMember(e => e.Name, i => i.MapFrom(e => e.Name));

            CreateMap<Book, BookDto>()
               .ForMember(dto => dto.Id, opt => opt.MapFrom(b => b.Id.Value))
               .ForMember(dto => dto.ClientName, opt => opt.MapFrom(b => b.ClientId.Value))
               .ForMember(dto => dto.EmployeeName, opt => opt.MapFrom(b => b.EmployeeId.Value))
               .ForMember(dto => dto.StartTime, opt => opt.MapFrom(b => b.Time.StartTime))
               .ForMember(dto => dto.EndTime, opt => opt.MapFrom(b => b.Time.EndTime))
               .ForMember(dto => dto.Status, opt => opt.MapFrom(b => b.BookStatus.ToString()));
        }
    }
}

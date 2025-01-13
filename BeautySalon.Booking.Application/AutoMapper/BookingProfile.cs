using AutoMapper;
using BeautySalon.Booking.Application.DTO;
using BeautySalon.Booking.Application.DTO.Booking;
using BeautySalon.Booking.Application.Features.Booking.CreateBooking;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate.ValueObjects;
using BeautySalon.Booking.Domain.AggregatesModel.BookingAggregate;
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
                .ForMember(b => b.Discount, i => i.MapFrom(b => b.Discount))
                .ForMember(b => b.ServiceId, i => i.MapFrom(b => b.ServiceId))
                .ForMember(b => b.EmployeeId, i => i.MapFrom(b => b.EmployeeId));

            CreateMap<Employee, EmployeeDTO>()
                .ForMember(e => e.Id, i => i.MapFrom(e => e.Id.Value))
                .ForMember(e => e.Name, i => i.MapFrom(e => e.Name));

            CreateMap<Book, BookDto>()
               .ForMember(dto => dto.Id, opt => opt.MapFrom(b => b.Id.Value))
               .ForMember(dto => dto.ClientName, opt => opt.MapFrom(b => b.ClientId.Value))
               .ForMember(dto => dto.EmployeeName, opt => opt.MapFrom(b => b.EmployeeId.Value))
               .ForMember(dto => dto.StartTime, opt => opt.MapFrom(b => b.Time.StartTime))
               .ForMember(dto => dto.EndTime, opt => opt.MapFrom(b => b.Time.EndTime))
               .ForMember(dto => dto.Status, opt => opt.MapFrom(b => b.BookStatus.ToString()));


            CreateMap<BookDto, Book>()
                .ForMember(b => b.ClientId, opt => opt.MapFrom(dto => ClientId.Create(Guid.Parse(dto.ClientName)))) 
                .ForMember(b => b.EmployeeId, opt => opt.MapFrom(dto => EmployeeId.Create(Guid.Parse(dto.EmployeeName))))
                .ForMember(b => b.Time, opt => opt.MapFrom(dto => new BookingTime(dto.StartTime, dto.EndTime - dto.StartTime)))
                .ForMember(b => b.BookStatus, opt => opt.MapFrom(dto => BookStatus.FromDisplayName<BookStatus>(dto.Status)));
        }
    }
}

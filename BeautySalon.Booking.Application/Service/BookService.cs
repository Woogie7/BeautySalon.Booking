using AutoMapper;
using BeautySalon.Booking.Application.DTO.Booking;
using BeautySalon.Booking.Application.Interface;

namespace BeautySalon.Booking.Application.Service
{
    public class BookService : IBookService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public BookService(ICacheService cacheService, IBookingRepository bookRepository, IMapper mapper)
        {
            _cacheService = cacheService;
            _bookingRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetBookingsAsync(BookingFilter bookingFilter)
        {
            var cacheKey = GenerateCacheKey(bookingFilter);
            var cacheBookings = await _cacheService.GetAsync<IEnumerable<BookDto>>(cacheKey);

            if (cacheBookings == null)
            {
                var bookingsDb = await _bookingRepository.GetBookingsAsync(bookingFilter);

                var bookingsDto = _mapper.Map<IEnumerable<BookDto>>(bookingsDb);

                await _cacheService.SetAsync(cacheKey, bookingsDto, TimeSpan.FromMinutes(10));

                return bookingsDto;
            }

            return cacheBookings;
        }

        private string GenerateCacheKey(BookingFilter filter)
        {
            return $"bookings:{filter.ClientId}:{filter.EmployeeId}:{filter.StartDate}:{filter.EndDate}";
        }

    }

    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetBookingsAsync(BookingFilter bookingFilter);
    }
}

namespace BeautySalon.Booking.Application.DTO
{
    public record BookingResponse(
        string id,
        TimeResponse Time,
        ClientResponse ClientResponse,
        ServiceResponse ServiceResponse,
        EmployeeResponse EmployeeResponse);

    public record TimeResponse(
        DateTime StartTime,
        DateTime EndTime);

    public record ClientResponse(
        string Id,
        string Name,
        string Surnname);

    public record ServiceResponse(
        string Id,
        string Name);
    public record EmployeeResponse(
        string id,
        string Name);
}

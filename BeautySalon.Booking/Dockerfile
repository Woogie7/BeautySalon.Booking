FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BeautySalon.Booking/BeautySalon.Booking.csproj", "BeautySalon.Booking/"]
RUN dotnet restore "./BeautySalon.Booking/BeautySalon.Booking.csproj"
COPY . .
WORKDIR "/src/BeautySalon.Booking"
RUN dotnet build "./BeautySalon.Booking.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BeautySalon.Booking.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BeautySalon.Booking.dll"]
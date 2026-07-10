namespace PruebaTekus.Application.Dashboard;

public record CountByCountryDto(string Country, int Count);

public record DashboardSummaryDto(
    int TotalProviders,
    int TotalServices,
    decimal AverageHourlyRate,
    IReadOnlyList<CountByCountryDto> ProvidersByCountry,
    IReadOnlyList<CountByCountryDto> ServicesByCountry);

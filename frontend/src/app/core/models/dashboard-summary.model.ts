export interface CountByCountry {
  country: string;
  count: number;
}

export interface DashboardSummary {
  totalProviders: number;
  totalServices: number;
  averageHourlyRate: number;
  providersByCountry: CountByCountry[];
  servicesByCountry: CountByCountry[];
}

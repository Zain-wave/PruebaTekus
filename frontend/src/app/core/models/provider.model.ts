export interface Provider {
  id: number;
  nit: string;
  name: string;
  website: string;
  email: string;
  country: string;
  servicesCount: number;
}

export interface CreateProviderRequest {
  nit: string;
  name: string;
  website: string;
  email: string;
  country: string;
}

export interface UpdateProviderRequest {
  id: number;
  name: string;
  website: string;
  email: string;
  country: string;
}

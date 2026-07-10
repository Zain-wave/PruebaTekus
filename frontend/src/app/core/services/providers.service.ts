import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedRequest, PagedResult } from '../models/paged-result.model';
import { CreateProviderRequest, Provider, UpdateProviderRequest } from '../models/provider.model';

@Injectable({ providedIn: 'root' })
export class ProvidersService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/providers`;

  getPaged(request: PagedRequest): Observable<PagedResult<Provider>> {
    let params = new HttpParams();

    if (request.page !== undefined) {
      params = params.set('page', request.page);
    }
    if (request.pageSize !== undefined) {
      params = params.set('pageSize', request.pageSize);
    }
    if (request.search) {
      params = params.set('search', request.search);
    }
    if (request.sortBy) {
      params = params.set('sortBy', request.sortBy);
    }
    if (request.sortDescending !== undefined) {
      params = params.set('sortDescending', request.sortDescending);
    }

    return this.http.get<PagedResult<Provider>>(this.baseUrl, { params });
  }

  getById(id: number): Observable<Provider> {
    return this.http.get<Provider>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateProviderRequest): Observable<number> {
    return this.http.post<number>(this.baseUrl, request);
  }

  update(request: UpdateProviderRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${request.id}`, request);
  }
}

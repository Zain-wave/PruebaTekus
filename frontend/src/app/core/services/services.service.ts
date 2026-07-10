import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedRequest, PagedResult } from '../models/paged-result.model';
import { CreateServiceRequest, Service, UpdateServiceRequest } from '../models/service.model';

@Injectable({ providedIn: 'root' })
export class ServicesService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/services`;

  getPaged(request: PagedRequest): Observable<PagedResult<Service>> {
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

    return this.http.get<PagedResult<Service>>(this.baseUrl, { params });
  }

  getById(id: number): Observable<Service> {
    return this.http.get<Service>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateServiceRequest): Observable<number> {
    return this.http.post<number>(this.baseUrl, request);
  }

  update(request: UpdateServiceRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${request.id}`, request);
  }
}

import { DecimalPipe } from '@angular/common';
import { Component, computed, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { DashboardService } from '../../core/services/dashboard.service';
import { CountByCountry } from '../../core/models/dashboard-summary.model';

interface BarRow extends CountByCountry {
  pct: number;
}

@Component({
  selector: 'app-dashboard',
  imports: [DecimalPipe],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard {
  private readonly dashboardService = inject(DashboardService);

  private readonly summary = toSignal(this.dashboardService.getSummary());

  protected readonly totalProviders = computed(() => this.summary()?.totalProviders ?? 0);
  protected readonly totalServices = computed(() => this.summary()?.totalServices ?? 0);
  protected readonly averageHourlyRate = computed(() => this.summary()?.averageHourlyRate ?? 0);

  protected readonly providersByCountry = computed(() => this.toBarRows(this.summary()?.providersByCountry ?? []));
  protected readonly servicesByCountry = computed(() => this.toBarRows(this.summary()?.servicesByCountry ?? []));

  private toBarRows(items: CountByCountry[]): BarRow[] {
    const max = items.reduce((current, item) => Math.max(current, item.count), 0);

    return items.map((item) => ({
      ...item,
      pct: max === 0 ? 0 : (item.count / max) * 100,
    }));
  }
}

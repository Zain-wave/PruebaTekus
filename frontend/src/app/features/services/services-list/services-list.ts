import { Component, computed, effect, inject, signal } from '@angular/core';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { map, switchMap } from 'rxjs';
import { Service } from '../../../core/models/service.model';
import { ProvidersService } from '../../../core/services/providers.service';
import { ServicesService } from '../../../core/services/services.service';
import { ServiceDialog, ServiceDialogData } from '../service-dialog/service-dialog';

@Component({
  selector: 'app-services-list',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatSelectModule,
    MatSortModule,
    MatTableModule,
  ],
  templateUrl: './services-list.html',
  styleUrl: './services-list.scss',
})
export class ServicesList {
  private readonly providersService = inject(ProvidersService);
  private readonly servicesService = inject(ServicesService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  protected readonly displayedColumns = ['id', 'name', 'hourlyRate', 'edit'];
  protected readonly providerControl = new FormControl<number | null>(null);

  private readonly refreshTick = signal(0);
  private readonly sortBy = signal<string | undefined>(undefined);
  private readonly sortDescending = signal(false);

  protected readonly providers = toSignal(
    this.providersService.getPaged({ page: 1, pageSize: 100, sortBy: 'name' }).pipe(map((result) => result.items)),
    { initialValue: [] },
  );

  private readonly selectedProviderId = toSignal(this.providerControl.valueChanges, {
    initialValue: null as number | null,
  });

  protected readonly effectiveProviderId = computed(() => this.selectedProviderId() ?? this.providers()[0]?.id ?? null);

  protected readonly selectedProvider = computed(() =>
    this.providers().find((provider) => provider.id === this.effectiveProviderId()),
  );

  private readonly services = toSignal(
    toObservable(
      computed(() => {
        this.refreshTick();
        return { sortBy: this.sortBy(), sortDescending: this.sortDescending() };
      }),
    ).pipe(switchMap((query) => this.servicesService.getPaged({ ...query, page: 1, pageSize: 100 }))),
    { initialValue: undefined },
  );

  protected readonly filteredServices = computed(() => {
    const providerId = this.effectiveProviderId();
    return (this.services()?.items ?? []).filter((service) => service.providerId === providerId);
  });

  constructor() {
    effect(() => {
      const defaultId = this.providers()[0]?.id;
      if (defaultId !== undefined && this.providerControl.value === null) {
        this.providerControl.setValue(defaultId);
      }
    });
  }

  protected formatId(id: number): string {
    return `#${String(id).padStart(4, '0')}`;
  }

  protected onSort(sort: Sort): void {
    this.sortBy.set(sort.direction ? sort.active : undefined);
    this.sortDescending.set(sort.direction === 'desc');
  }

  protected openNew(): void {
    const provider = this.selectedProvider();
    if (!provider) {
      return;
    }
    this.openDialog({ mode: 'create', providerId: provider.id, providerName: provider.name });
  }

  protected openEdit(service: Service): void {
    const provider = this.selectedProvider();
    if (!provider) {
      return;
    }
    this.openDialog({ mode: 'edit', providerId: provider.id, providerName: provider.name, service });
  }

  private openDialog(data: ServiceDialogData): void {
    this.dialog
      .open(ServiceDialog, { data })
      .afterClosed()
      .subscribe((result) => {
        if (!result) {
          return;
        }

        this.refreshTick.update((tick) => tick + 1);
        const message =
          data.mode === 'create'
            ? `Service "${result.name}" created — notification email sent to procurement@tekus.com about ${result.providerName}.`
            : `Service "${result.name}" updated.`;
        this.snackBar.open(message, 'DISMISS');
      });
  }
}

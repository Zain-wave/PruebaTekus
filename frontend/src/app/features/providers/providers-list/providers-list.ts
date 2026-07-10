import { Component, computed, inject, signal } from '@angular/core';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { debounceTime, startWith, switchMap } from 'rxjs';
import { PagedRequest } from '../../../core/models/paged-result.model';
import { Provider } from '../../../core/models/provider.model';
import { ProvidersService } from '../../../core/services/providers.service';
import { ProviderDialog, ProviderDialogData } from '../provider-dialog/provider-dialog';

@Component({
  selector: 'app-providers-list',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatPaginatorModule,
    MatSortModule,
    MatTableModule,
  ],
  templateUrl: './providers-list.html',
  styleUrl: './providers-list.scss',
})
export class ProvidersList {
  private readonly providersService = inject(ProvidersService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  protected readonly displayedColumns = ['id', 'name', 'nit', 'email', 'website', 'country', 'edit'];
  protected readonly searchControl = new FormControl('', { nonNullable: true });

  private readonly search = toSignal(
    this.searchControl.valueChanges.pipe(debounceTime(300), startWith('')),
    { initialValue: '' },
  );

  private readonly pageIndex = signal(0);
  private readonly pageSize = signal(10);
  private readonly sortBy = signal<string | undefined>(undefined);
  private readonly sortDescending = signal(false);
  private readonly refreshTick = signal(0);

  private readonly query = computed<PagedRequest>(() => {
    this.refreshTick();

    return {
      page: this.pageIndex() + 1,
      pageSize: this.pageSize(),
      search: this.search() || undefined,
      sortBy: this.sortBy(),
      sortDescending: this.sortDescending(),
    };
  });

  private readonly result = toSignal(
    toObservable(this.query).pipe(switchMap((query) => this.providersService.getPaged(query))),
    { initialValue: undefined },
  );

  protected readonly providers = computed(() => this.result()?.items ?? []);
  protected readonly totalCount = computed(() => this.result()?.totalCount ?? 0);

  protected formatId(id: number): string {
    return `#${String(id).padStart(4, '0')}`;
  }

  protected onPage(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  protected onSort(sort: Sort): void {
    this.sortBy.set(sort.direction ? sort.active : undefined);
    this.sortDescending.set(sort.direction === 'desc');
  }

  protected openNew(): void {
    this.openDialog({ mode: 'create' });
  }

  protected openEdit(provider: Provider): void {
    this.openDialog({ mode: 'edit', provider });
  }

  private openDialog(data: ProviderDialogData): void {
    this.dialog
      .open(ProviderDialog, { data })
      .afterClosed()
      .subscribe((result) => {
        if (!result) {
          return;
        }

        this.refreshTick.update((tick) => tick + 1);
        const action = data.mode === 'create' ? 'created' : 'updated';
        this.snackBar.open(`Provider "${result.name}" ${action}.`, 'DISMISS');
      });
  }
}

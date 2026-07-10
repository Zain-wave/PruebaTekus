import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Provider } from '../../../core/models/provider.model';
import { ProvidersService } from '../../../core/services/providers.service';

export interface ProviderDialogData {
  mode: 'create' | 'edit';
  provider?: Provider;
}

export interface ProviderDialogResult {
  name: string;
}

export const COUNTRIES = ['Colombia', 'United States', 'Mexico', 'Argentina', 'Chile', 'Brazil', 'Peru'];

@Component({
  selector: 'app-provider-dialog',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
  ],
  templateUrl: './provider-dialog.html',
  styleUrl: './provider-dialog.scss',
})
export class ProviderDialog {
  private readonly fb = inject(FormBuilder);
  private readonly providersService = inject(ProvidersService);
  private readonly dialogRef = inject(MatDialogRef<ProviderDialog, ProviderDialogResult>);
  protected readonly data = inject<ProviderDialogData>(MAT_DIALOG_DATA);

  protected readonly countries = COUNTRIES;
  protected readonly saving = signal(false);
  protected readonly isEdit = this.data.mode === 'edit';
  protected readonly title = computed(() => (this.isEdit ? 'Edit provider' : 'New provider'));

  protected readonly form = this.fb.nonNullable.group({
    name: [this.data.provider?.name ?? '', Validators.required],
    nit: [{ value: this.data.provider?.nit ?? '', disabled: this.isEdit }, Validators.required],
    country: [this.data.provider?.country ?? this.countries[0], Validators.required],
    website: [this.data.provider?.website ?? ''],
    email: [this.data.provider?.email ?? '', [Validators.required, Validators.email]],
  });

  protected cancel(): void {
    this.dialogRef.close();
  }

  protected save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const value = this.form.getRawValue();

    const onSuccess = () => this.dialogRef.close({ name: value.name });
    const onError = () => this.saving.set(false);

    if (this.isEdit) {
      this.providersService
        .update({
          id: this.data.provider!.id,
          name: value.name,
          website: value.website,
          email: value.email,
          country: value.country,
        })
        .subscribe({ next: onSuccess, error: onError });
    } else {
      this.providersService
        .create({
          nit: value.nit,
          name: value.name,
          website: value.website,
          email: value.email,
          country: value.country,
        })
        .subscribe({ next: onSuccess, error: onError });
    }
  }
}

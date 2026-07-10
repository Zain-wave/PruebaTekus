import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Service } from '../../../core/models/service.model';
import { ServicesService } from '../../../core/services/services.service';

export interface ServiceDialogData {
  mode: 'create' | 'edit';
  providerId: number;
  providerName: string;
  service?: Service;
}

export interface ServiceDialogResult {
  name: string;
  providerName: string;
}

@Component({
  selector: 'app-service-dialog',
  imports: [ReactiveFormsModule, MatButtonModule, MatDialogModule, MatFormFieldModule, MatIconModule, MatInputModule],
  templateUrl: './service-dialog.html',
  styleUrl: './service-dialog.scss',
})
export class ServiceDialog {
  private readonly fb = inject(FormBuilder);
  private readonly servicesService = inject(ServicesService);
  private readonly dialogRef = inject(MatDialogRef<ServiceDialog, ServiceDialogResult>);
  protected readonly data = inject<ServiceDialogData>(MAT_DIALOG_DATA);

  protected readonly saving = signal(false);
  protected readonly isEdit = this.data.mode === 'edit';
  protected readonly title = computed(() => (this.isEdit ? 'Edit service' : 'New service'));

  protected readonly form = this.fb.nonNullable.group({
    name: [this.data.service?.name ?? '', Validators.required],
    hourlyRate: [this.data.service?.hourlyRate ?? 0, [Validators.required, Validators.min(0)]],
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

    const onSuccess = () => this.dialogRef.close({ name: value.name, providerName: this.data.providerName });
    const onError = () => this.saving.set(false);

    if (this.isEdit) {
      this.servicesService
        .update({
          id: this.data.service!.id,
          name: value.name,
          hourlyRate: value.hourlyRate,
        })
        .subscribe({ next: onSuccess, error: onError });
    } else {
      this.servicesService
        .create({
          name: value.name,
          hourlyRate: value.hourlyRate,
          providerId: this.data.providerId,
        })
        .subscribe({ next: onSuccess, error: onError });
    }
  }
}

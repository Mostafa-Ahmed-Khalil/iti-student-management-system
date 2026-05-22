import { Component, OnInit, computed, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { TrackService } from '../../../core/services/track.service';
import { Track } from '../../../core/models/track.model';
import { ToastService } from '../../../core/toast.service';

import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DatePickerModule } from 'primeng/datepicker';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-track-management',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, TableModule, DialogModule, ButtonModule, InputTextModule, DatePickerModule, ConfirmDialogModule, CardModule],
  providers: [ConfirmationService],
  templateUrl: './track-management.component.html',
  styleUrls: ['./track-management.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TrackManagementComponent implements OnInit {
  private trackService = inject(TrackService);
  private fb = inject(FormBuilder);
  private toastService = inject(ToastService);

  myBranch = signal<any>(null);
  tracks = signal<Track[]>([]);
  isLoading = signal<boolean>(false);
  
  isModalOpen = signal<boolean>(false);
  modalTitle = signal<string>('Add Track');
  editingTrackId = signal<number | null>(null);

  trackForm: FormGroup = this.fb.group({
    name: ['', Validators.required],
    startDate: ['', Validators.required]
  });

  ngOnInit() {
    this.loadMyBranchAndTracks();
  }

  loadMyBranchAndTracks() {
    this.isLoading.set(true);
    this.trackService.getMyBranch().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.myBranch.set(response.data);
          this.loadTracks(response.data.id);
        } else {
          this.isLoading.set(false);
          this.toastService.error('You are not assigned to any branch.');
        }
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  loadTracks(branchId: number) {
    this.trackService.getTracksByBranch(branchId).subscribe({
      next: (response) => {
        if (response.success) {
          this.tracks.set(response.data);
        }
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  openAddModal() {
    this.modalTitle.set('Add Track');
    this.editingTrackId.set(null);
    this.trackForm.reset();
    this.isModalOpen.set(true);
  }

  openEditModal(track: Track) {
    this.modalTitle.set('Edit Track');
    this.editingTrackId.set(track.id);
    
    // Format date for input type="date"
    const date = new Date(track.startDate);
    const dateString = date.toISOString().split('T')[0];

    this.trackForm.patchValue({
      name: track.name,
      startDate: dateString
    });
    this.isModalOpen.set(true);
  }

  closeModal() {
    this.isModalOpen.set(false);
  }

  saveTrack() {
    if (this.trackForm.invalid) return;

    const branch = this.myBranch();
    if (!branch) return;

    this.isLoading.set(true);
    const request = this.trackForm.value;
    const id = this.editingTrackId();

    if (id) {
      // Editing track implies we keep its current IsActive state unless changed, but backend handles this
      this.trackService.updateTrack(id, { ...request, isActive: true }).subscribe({
        next: (response) => {
          if (response.success) {
            this.toastService.success('Track updated successfully');
            this.loadTracks(branch.id);
            this.closeModal();
          }
        },
        complete: () => this.isLoading.set(false)
      });
    } else {
      this.trackService.createTrack(branch.id, request).subscribe({
        next: (response) => {
          if (response.success) {
            this.toastService.success('Track created successfully');
            this.loadTracks(branch.id);
            this.closeModal();
          }
        },
        complete: () => this.isLoading.set(false)
      });
    }
  }

  deactivateTrack(track: Track) {
    if (confirm(`Are you sure you want to deactivate ${track.name}?`)) {
      this.isLoading.set(true);
      this.trackService.deleteTrack(track.id).subscribe({
        next: (response) => {
          if (response.success) {
            this.toastService.success('Track deactivated successfully');
            this.loadTracks(this.myBranch().id);
          }
        },
        complete: () => this.isLoading.set(false)
      });
    }
  }
}

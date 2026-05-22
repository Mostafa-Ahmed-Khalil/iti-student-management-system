import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TrackService } from '../../../core/services/track.service';
import { Track } from '../../../core/models/track.model';
import { ToastService } from '../../../core/toast.service';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-supervisor-tracks',
  standalone: true,
  imports: [CommonModule, RouterModule, TableModule, ButtonModule, CardModule],
  template: `
    <div class="management-container">
      <p-card styleClass="premium-card">
        <div class="header-row">
          <div class="title-section">
            <h2>My Assigned Tracks</h2>
            <p>Select a track to manage its courses.</p>
          </div>
        </div>

        <p-table [value]="tracks()" [loading]="isLoading()" dataKey="id" [paginator]="true" [rows]="10" styleClass="p-datatable-striped">
          <ng-template #header>
            <tr>
              <th>Track Name</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </ng-template>
          <ng-template #body let-track>
            <tr>
              <td class="font-medium">{{ track.name }}</td>
              <td>
                <span [class]="'status-badge ' + (track.isActive ? 'status-active' : 'status-inactive')">
                  {{ track.isActive ? 'Active' : 'Inactive' }}
                </span>
              </td>
              <td class="actions-cell">
                <p-button label="Manage Courses" icon="pi pi-book" size="small" variant="outlined" [routerLink]="['/supervisor/tracks', track.id, 'courses']"></p-button>
              </td>
            </tr>
          </ng-template>
          <ng-template #empty>
            <tr>
              <td colspan="3" class="empty-message">You are not assigned to any tracks.</td>
            </tr>
          </ng-template>
        </p-table>
      </p-card>
    </div>
  `,
  styles: [`
    .management-container { padding: 2rem; }
    .header-row { display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem; }
    .title-section h2 { margin: 0; font-size: 1.5rem; font-weight: 600; color: var(--text-primary); }
    .title-section p { margin: 0.5rem 0 0; color: var(--text-secondary); }
    .actions-cell { display: flex; gap: 0.5rem; }
    .empty-message { text-align: center; padding: 3rem !important; color: var(--text-secondary); }
    .status-badge { padding: 0.25rem 0.75rem; border-radius: 9999px; font-size: 0.875rem; font-weight: 500; }
    .status-active { background: rgba(34, 197, 94, 0.1); color: rgb(21, 128, 61); }
    .status-inactive { background: rgba(239, 68, 68, 0.1); color: rgb(185, 28, 28); }
  `]
})
export class SupervisorTracksComponent implements OnInit {
  private trackService = inject(TrackService);
  private toastService = inject(ToastService);

  tracks = signal<Track[]>([]);
  isLoading = signal<boolean>(false);

  ngOnInit() {
    this.loadTracks();
  }

  loadTracks() {
    this.isLoading.set(true);
    this.trackService.getMyTracks().subscribe({
      next: (response) => {
        if (response.success) {
          this.tracks.set(response.data);
        }
        this.isLoading.set(false);
      },
      error: () => {
        this.toastService.error('Failed to load tracks.');
        this.isLoading.set(false);
      }
    });
  }
}

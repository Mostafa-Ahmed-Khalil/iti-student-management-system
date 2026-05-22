import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { SpinnerService } from '../../../core/spinner.service';

@Component({
  selector: 'app-spinner',
  standalone: true,
  imports: [CommonModule, ProgressSpinnerModule],
  template: `
    <div class="spinner-overlay" *ngIf="spinnerService.isLoading$ | async">
      <p-progress-spinner></p-progress-spinner>
    </div>
  `,
  styles: [`
    .spinner-overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(0, 0, 0, 0.4);
      z-index: 10000;
      display: flex;
      justify-content: center;
      align-items: center;
      backdrop-filter: blur(2px);
    }
  `]
})
export class SpinnerComponent {
  spinnerService = inject(SpinnerService);
}

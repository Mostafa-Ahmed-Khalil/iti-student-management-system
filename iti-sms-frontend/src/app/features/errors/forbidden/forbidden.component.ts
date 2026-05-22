import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-forbidden',
  standalone: true,
  imports: [CommonModule, RouterModule, CardModule, ButtonModule],
  template: `
    <div class="forbidden-wrapper">
      <p-card styleClass="forbidden-card">
        <h1>403</h1>
        <h2>Access Denied</h2>
        <p>You do not have the required permissions to view this page.</p>
        <p-button label="Return to Dashboard" icon="pi pi-home" routerLink="/"></p-button>
      </p-card>
    </div>
  `,
  styles: [`
    .forbidden-wrapper {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: calc(100vh - 60px);
      background-color: var(--p-surface-50);
      padding: 1rem;
    }
    ::ng-deep .forbidden-card {
      text-align: center;
      width: 100%;
      max-width: 28rem;
    }
    h1 {
      font-size: 3.75rem;
      color: var(--p-red-500);
      margin: 0 0 1rem;
    }
    h2 {
      font-size: 1.5rem;
      color: var(--p-surface-900);
      margin: 0 0 0.5rem;
    }
    p {
      color: var(--p-surface-600);
      margin-bottom: 2rem;
    }
  `]
})
export class ForbiddenComponent {}

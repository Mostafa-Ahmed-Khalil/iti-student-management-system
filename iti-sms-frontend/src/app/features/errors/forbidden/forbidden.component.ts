import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-forbidden',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="forbidden-container">
      <div class="forbidden-content">
        <h1>403</h1>
        <h2>Access Denied</h2>
        <p>You do not have the required permissions to view this page.</p>
        <a routerLink="/" class="btn btn-primary">Return to Dashboard</a>
      </div>
    </div>
  `,
  styles: [`
    .forbidden-container {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: calc(100vh - 60px);
      background-color: var(--background, #f6f8fa);
      text-align: center;
    }
    .forbidden-content {
      background-color: var(--card-bg, #ffffff);
      padding: 3rem;
      border-radius: 8px;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    }
    h1 {
      font-size: 4rem;
      color: var(--error-color, #cf222e);
      margin: 0 0 1rem;
    }
    h2 {
      margin-top: 0;
      color: #24292f;
    }
    p {
      color: #57606a;
      margin-bottom: 2rem;
    }
    .btn {
      display: inline-block;
      text-decoration: none;
      padding: 0.5rem 1rem;
      border-radius: 6px;
      background-color: var(--primary-color, #0969da);
      color: white;
      font-weight: 500;
      transition: background-color 0.2s;
    }
    .btn:hover {
      background-color: #0550ae;
    }
  `]
})
export class ForbiddenComponent {}

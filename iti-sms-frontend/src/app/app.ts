import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthStore } from './core/auth/auth.store';
import { ToastService } from './core/toast.service';

import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule, CommonModule, ToastModule, ToolbarModule, ButtonModule, SelectModule, FormsModule],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class AppComponent implements OnInit {
  authStore = inject(AuthStore);
  toastService = inject(ToastService);
  private router = inject(Router);

  ngOnInit() {
    this.authStore.checkInitialState();
  }

  switchRole(role: string) {
    if (role) {
      this.authStore.switchRole(role);
      this.router.navigate(['/']); // Redirect to home on role change
    }
  }

  logout() {
    this.authStore.logout();
    this.router.navigate(['/login']);
  }
}

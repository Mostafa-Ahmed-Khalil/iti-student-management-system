import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet, Router, RouterModule, NavigationStart, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthStore } from './core/auth/auth.store';
import { ToastService } from './core/toast.service';
import { SpinnerService } from './core/spinner.service';

import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';
import { SpinnerComponent } from './shared/components/spinner/spinner.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule, CommonModule, ToastModule, ToolbarModule, ButtonModule, SelectModule, FormsModule, SpinnerComponent],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class AppComponent implements OnInit {
  authStore = inject(AuthStore);
  toastService = inject(ToastService);
  spinnerService = inject(SpinnerService);
  private router = inject(Router);

  ngOnInit() {
    this.authStore.checkInitialState();
    
    // Show spinner on routing transitions
    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.spinnerService.show();
      } else if (
        event instanceof NavigationEnd ||
        event instanceof NavigationCancel ||
        event instanceof NavigationError
      ) {
        // Add a micro-delay for smooth rendering and transitions
        setTimeout(() => {
          this.spinnerService.hide();
        }, 200);
      }
    });
  }

  isLoginRoute() {
    return this.router.url === '/login';
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

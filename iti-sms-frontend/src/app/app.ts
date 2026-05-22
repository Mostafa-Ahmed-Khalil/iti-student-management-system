import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthStore } from './core/auth/auth.store';
import { ToastService } from './core/toast.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
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

  switchRole(event: Event) {
    const select = event.target as HTMLSelectElement;
    if (select.value) {
      this.authStore.switchRole(select.value);
      this.router.navigate(['/']); // Redirect to home on role change
    }
  }

  logout() {
    this.authStore.logout();
    this.router.navigate(['/login']);
  }
}

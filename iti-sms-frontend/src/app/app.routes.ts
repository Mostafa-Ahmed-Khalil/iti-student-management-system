import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { ForbiddenComponent } from './features/errors/forbidden/forbidden.component';
import { authGuard } from './core/auth/auth.guard';
import { guestGuard } from './core/auth/guest.guard';

export const routes: Routes = [
  { 
    path: 'login', 
    component: LoginComponent,
    canActivate: [guestGuard]
  },
  { path: '403', component: ForbiddenComponent },
  
  { 
    path: 'admin/branches', 
    loadComponent: () => import('./features/admin/branch-management/branch-management.component').then(m => m.BranchManagementComponent),
    canActivate: [authGuard]
  },

  { path: '', redirectTo: '/admin/branches', pathMatch: 'full' },
  { path: '**', redirectTo: '/admin/branches' }
];

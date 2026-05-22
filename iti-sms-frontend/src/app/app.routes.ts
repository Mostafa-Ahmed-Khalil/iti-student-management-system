import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { ForbiddenComponent } from './features/errors/forbidden/forbidden.component';
import { authGuard } from './core/auth/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '403', component: ForbiddenComponent },
  
  // Future dashboard routes can be protected here like this:
  // { 
  //   path: 'dashboard', 
  //   loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent),
  //   canActivate: [authGuard]
  // },

  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];

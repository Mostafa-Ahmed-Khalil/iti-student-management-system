import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { ForbiddenComponent } from './features/errors/forbidden/forbidden.component';
import { authGuard } from './core/auth/auth.guard';
import { guestGuard } from './core/auth/guest.guard';
import { roleGuard } from './core/auth/role.guard';
import { homeRedirectGuard } from './core/auth/home.guard';

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
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin'] }
  },
  { 
    path: 'admin/users', 
    loadComponent: () => import('./features/admin/user-management/user-management.component').then(m => m.UserManagementComponent),
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin'] }
  },
  {
    path: 'branch-manager',
    loadChildren: () => import('./features/branch-manager/branch-manager.routes').then(m => m.BRANCH_MANAGER_ROUTES),
    canActivate: [roleGuard],
    data: { roles: ['Branch Manager'] }
  },
  {
    path: 'supervisor',
    loadChildren: () => import('./features/supervisor/supervisor.routes').then(m => m.SUPERVISOR_ROUTES),
    canActivate: [roleGuard],
    data: { roles: ['Technical Supervisor'] }
  },
  {
    path: 'instructor',
    loadChildren: () => import('./features/instructor/instructor.routes').then(m => m.INSTRUCTOR_ROUTES),
    canActivate: [roleGuard],
    data: { roles: ['Instructor'] }
  },

  { 
    path: '', 
    canActivate: [homeRedirectGuard], 
    children: [] 
  },
  { 
    path: '**', 
    canActivate: [homeRedirectGuard], 
    children: [] 
  }
];

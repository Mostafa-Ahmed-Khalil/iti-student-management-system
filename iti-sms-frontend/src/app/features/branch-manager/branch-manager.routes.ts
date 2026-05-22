import { Routes } from '@angular/router';

export const BRANCH_MANAGER_ROUTES: Routes = [
  {
    path: 'tracks',
    loadComponent: () => import('./track-management/track-management.component').then(c => c.TrackManagementComponent),
    title: 'Track Management'
  },
  {
    path: '',
    redirectTo: 'tracks',
    pathMatch: 'full'
  }
];

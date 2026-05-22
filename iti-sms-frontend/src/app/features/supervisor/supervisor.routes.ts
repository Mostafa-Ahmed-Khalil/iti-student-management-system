import { Routes } from '@angular/router';

export const SUPERVISOR_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'tracks',
    pathMatch: 'full'
  },
  {
    path: 'tracks',
    loadComponent: () => import('./supervisor-tracks/supervisor-tracks.component').then(m => m.SupervisorTracksComponent)
  },
  {
    path: 'tracks/:id/courses',
    loadComponent: () => import('./course-management/course-management.component').then(m => m.CourseManagementComponent)
  },
  {
    path: 'tracks/:trackId/enrollments',
    loadComponent: () => import('./supervisor-enrollment/supervisor-enrollment.component').then(m => m.SupervisorEnrollmentComponent)
  },
  {
    path: 'students',
    loadComponent: () => import('./supervisor-students/supervisor-students.component').then(m => m.SupervisorStudentsComponent)
  }
];

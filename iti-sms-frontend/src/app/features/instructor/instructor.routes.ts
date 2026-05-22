import { Routes } from '@angular/router';

export const INSTRUCTOR_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'courses',
    pathMatch: 'full'
  },
  {
    path: 'courses',
    loadComponent: () => import('./instructor-courses/instructor-courses.component').then(m => m.InstructorCoursesComponent)
  },
  {
    path: 'courses/:courseId/evaluations',
    loadComponent: () => import('./lab-evaluation/lab-evaluation.component').then(m => m.LabEvaluationComponent)
  }
];

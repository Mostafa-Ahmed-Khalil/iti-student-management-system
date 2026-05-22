import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthStore } from './auth.store';

export const homeRedirectGuard: CanActivateFn = () => {
  const router = inject(Router);
  const authStore = inject(AuthStore);

  const role = authStore.activeRole();
  if (role === 'Admin') {
    return router.parseUrl('/admin/branches');
  }
  if (role === 'Branch Manager') {
    return router.parseUrl('/branch-manager/tracks');
  }
  if (role === 'Technical Supervisor') {
    return router.parseUrl('/supervisor/tracks');
  }
  
  return router.parseUrl('/403');
};

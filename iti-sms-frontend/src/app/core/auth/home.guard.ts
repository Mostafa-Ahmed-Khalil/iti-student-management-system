import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthStore } from './auth.store';

export const homeRedirectGuard: CanActivateFn = () => {
  const router = inject(Router);
  const authStore = inject(AuthStore);

  const role = authStore.activeRole();
  if (role === 'Branch Manager') {
    return router.parseUrl('/branch-manager/tracks');
  }
  
  return router.parseUrl('/admin/branches');
};

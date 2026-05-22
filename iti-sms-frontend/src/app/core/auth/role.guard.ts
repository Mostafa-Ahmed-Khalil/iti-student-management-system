import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthStore } from './auth.store';

export const roleGuard: CanActivateFn = (route, state) => {
  const store = inject(AuthStore);
  const router = inject(Router);
  
  const expectedRoles = route.data?.['roles'] as Array<string>;
  const activeRole = store.activeRole();

  if (!store.isAuthenticated() || !activeRole) {
    return router.createUrlTree(['/login']);
  }

  if (expectedRoles && expectedRoles.length > 0 && !expectedRoles.includes(activeRole)) {
    return router.createUrlTree(['/403']);
  }

  return true;
};

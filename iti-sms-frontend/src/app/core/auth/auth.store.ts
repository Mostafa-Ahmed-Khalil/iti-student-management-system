import { computed } from '@angular/core';
import { signalStore, withState, withMethods, withComputed, patchState } from '@ngrx/signals';
import { jwtDecode } from 'jwt-decode';

export interface AuthState {
  currentUser: any | null;
  activeRole: string | null;
  isAuthenticated: boolean;
  allRoles: string[];
}

const initialState: AuthState = {
  currentUser: null,
  activeRole: null,
  isAuthenticated: false,
  allRoles: []
};

function decodeToken(token: string) {
  try {
    const decoded: any = jwtDecode(token);
    // Based on FR-1.2, users have multiple roles in the 'roles' array
    const roles = Array.isArray(decoded.roles) ? decoded.roles : (decoded.roles ? [decoded.roles] : []);
    
    return {
      currentUser: {
        userId: decoded.userId || decoded.sub || '',
        email: decoded.email || ''
      },
      roles
    };
  } catch (error) {
    return null;
  }
}

export const AuthStore = signalStore(
  { providedIn: 'root' },
  withState(initialState),
  withComputed(({ isAuthenticated }) => ({
    isLoggedIn: computed(() => isAuthenticated())
  })),
  withMethods((store) => ({
    login(token: string) {
      localStorage.setItem('token', token);
      const decoded = decodeToken(token);
      
      if (decoded) {
        patchState(store, {
          currentUser: decoded.currentUser,
          allRoles: decoded.roles,
          activeRole: decoded.roles.length > 0 ? decoded.roles[0] : null,
          isAuthenticated: true
        });
      }
    },
    
    switchRole(role: string) {
      if (store.allRoles().includes(role)) {
        patchState(store, { activeRole: role });
      }
    },
    
    logout() {
      localStorage.removeItem('token');
      patchState(store, initialState);
    },
    
    checkInitialState() {
      const token = localStorage.getItem('token');
      if (token) {
        const decoded = decodeToken(token);
        if (decoded) {
          patchState(store, {
            currentUser: decoded.currentUser,
            allRoles: decoded.roles,
            activeRole: decoded.roles.length > 0 ? decoded.roles[0] : null,
            isAuthenticated: true
          });
        } else {
          localStorage.removeItem('token');
        }
      }
    }
  }))
);

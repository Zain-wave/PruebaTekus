import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then((m) => m.Login),
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () => import('./layout/shell/shell').then((m) => m.Shell),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      {
        path: 'dashboard',
        loadComponent: () => import('./features/dashboard/dashboard').then((m) => m.Dashboard),
      },
      {
        path: 'providers',
        loadComponent: () =>
          import('./features/providers/providers-list/providers-list').then((m) => m.ProvidersList),
      },
      {
        path: 'services',
        loadComponent: () =>
          import('./features/services/services-list/services-list').then((m) => m.ServicesList),
      },
    ],
  },
];

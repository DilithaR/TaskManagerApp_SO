import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login').then((m) => m.Login),
  },
  {
    path: 'tasks',
    loadComponent: () =>
      import('./features/tasks/task-page/task-page').then((m) => m.TaskPage),
  },
];
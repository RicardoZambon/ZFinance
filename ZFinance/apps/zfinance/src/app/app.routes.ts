import { Routes } from '@angular/router';
import { AuthGuard, MainLayoutComponent, SharedAuthModule } from '@shared';

export const routes: Routes = [  { path: '', component: MainLayoutComponent, canActivate: [ AuthGuard ], children: [
    { path: 'security', loadChildren: () => import('./features/security').then((m) => m.FeaturesSecurityModule) },
  ] },
  { path: '', loadChildren: () => SharedAuthModule },
];
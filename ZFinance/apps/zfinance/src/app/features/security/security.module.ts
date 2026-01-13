import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


const routes: Routes = [
  { path: 'actions', loadChildren: () => import('./actions').then((m) => m.FeaturesSecurityActionsModule) },
  { path: 'menus', loadChildren: () => import('./menus').then((m) => m.FeaturesSecurityMenusModule) },
  { path: 'roles', loadChildren: () => import('./roles').then((m) => m.FeaturesSecurityRolesModule) },
  { path: 'users', loadChildren: () => import('./users').then((m) => m.FeaturesSecurityUsersModule) },
];

@NgModule({
  declarations: [ ],
  imports: [
    RouterModule.forChild(routes),
  ]
})
export class FeaturesSecurityModule { }
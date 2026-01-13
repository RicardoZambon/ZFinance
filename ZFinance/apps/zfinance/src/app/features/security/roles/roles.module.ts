import { inject, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DefaultDetailsTabViewComponent, DefaultTabViewComponent } from '@framework';
import { ServicesHistoryViewComponent } from '@shared';
import { RolesDataProvider } from '../../../data-providers';
import { RolesService } from '../../../services';
import { RolesFormComponent } from './roles-form/roles-form';
import { RolesListComponent } from './roles-list/roles-list';


const routes: Routes = [
  {
    path: '',
    component: DefaultTabViewComponent,
    children: [
      { path: '', component: RolesListComponent, pathMatch: 'full' },
    ]
  },
  {
    path: ':id',
    component: DefaultDetailsTabViewComponent,
    data: {
      dataProvider: () => new RolesDataProvider(inject(RolesService)),
      defaultTitle: 'Roles-Details-Title-New',
    },
    children: [
      {
        path: '',
        component: RolesFormComponent,
        data: { icon: 'fa-dice-d6', title: 'Button-Views-Details' },
        pathMatch: 'full',
      },
      {
        path: 'audit',
        component: ServicesHistoryViewComponent,
        data: { allowedActions: [ '' ], controllerName: 'Roles', icon: 'fa-history', title: 'Button-Views-History' },
      },
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
  ],
})
export class FeaturesSecurityRolesModule { }
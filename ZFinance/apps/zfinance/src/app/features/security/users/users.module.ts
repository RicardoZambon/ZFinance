import { inject, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DefaultDetailsTabViewComponent, DefaultTabViewComponent } from '@framework';
import { UsersDataProvider } from '../../../data-providers';
import { UsersService } from '../../../services';
import { UsersFormComponent } from './users-form/users-form';
import { UsersListComponent } from './users-list/users-list';
import { ServicesHistoryViewComponent } from '@shared';


const routes: Routes = [
  {
    path: '',
    component: DefaultTabViewComponent,
    children: [
      { path: '', component: UsersListComponent, pathMatch: 'full' },
    ]
  },
  {
    path: ':id',
    component: DefaultDetailsTabViewComponent,
    data: {
      dataProvider: () => new UsersDataProvider(inject(UsersService)),
      defaultTitle: 'Users-Details-Title-New',
    },
    children: [
      {
        path: '',
        component: UsersFormComponent,
        data: { icon: 'fa-dice-d6', title: 'Button-Views-Details' },
        pathMatch: 'full',
      },
      {
        path: 'audit',
        component: ServicesHistoryViewComponent,
        data: { allowedActions: [ '' ], controllerName: 'Users', icon: 'fa-history', title: 'Button-Views-History' },
      },
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
  ],
})
export class FeaturesSecurityUsersModule { }
import { inject, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DefaultDetailsTabViewComponent, DefaultTabViewComponent } from '@framework';
import { ServicesHistoryViewComponent } from '@shared';
import { ActionsDataProvider } from '../../../data-providers';
import { ActionsService } from '../../../services';
import { ActionsFormComponent } from './actions-form/actions-form';
import { ActionsListComponent } from './actions-list/actions-list';


const routes: Routes = [
  {
    path: '',
    component: DefaultTabViewComponent,
    children: [
      { path: '', component: ActionsListComponent, pathMatch: 'full' },
    ]
  },
  {
    path: ':id',
    component: DefaultDetailsTabViewComponent,
    data: {
      dataProvider: () => new ActionsDataProvider(inject(ActionsService)),
      defaultTitle: 'Actions-Details-Title-New',
    },
    children: [
      {
        path: '',
        component: ActionsFormComponent,
        data: { icon: 'fa-dice-d6', title: 'Button-Views-Details' },
        pathMatch: 'full',
      },
      {
        path: 'audit',
        component: ServicesHistoryViewComponent,
        data: { allowedActions: [ '' ], controllerName: 'Actions', icon: 'fa-history', title: 'Button-Views-History' },
      },
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
  ],
})
export class FeaturesSecurityActionsModule { }
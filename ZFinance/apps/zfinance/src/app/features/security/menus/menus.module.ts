import { inject, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DefaultDetailsTabViewComponent, DefaultTabViewComponent } from '@framework';
import { ServicesHistoryViewComponent } from '@shared';
import { MenusDataProvider } from '../../../data-providers';
import { MenusService } from '../../../services';
import { MenusFormComponent } from './menus-form/menus-form';
import { MenusListComponent } from './menus-list/menus-list';


const routes: Routes = [
  {
    path: '',
    component: DefaultTabViewComponent,
    children: [
      { path: '', component: MenusListComponent, pathMatch: 'full' },
    ]
  },
  {
    path: ':id',
    component: DefaultDetailsTabViewComponent,
    data: {
      dataProvider: () => new MenusDataProvider(inject(MenusService)),
      defaultTitle: 'Menus-Details-Title-New',
    },
    children: [
      {
        path: '',
        component: MenusFormComponent,
        data: { icon: 'fa-dice-d6', title: 'Button-Views-Details' },
        pathMatch: 'full',
      },
      {
        path: 'audit',
        component: ServicesHistoryViewComponent,
        data: { allowedActions: [ '' ], controllerName: 'Menus', icon: 'fa-history', title: 'Button-Views-History' },
      },
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
  ],
})
export class FeaturesSecurityMenusModule { }
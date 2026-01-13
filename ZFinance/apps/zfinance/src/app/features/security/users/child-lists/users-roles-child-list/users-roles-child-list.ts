
import { Component } from '@angular/core';
import { ButtonEditComponent, ButtonRefreshComponent, ChildList } from '@framework';
import { DataGridComponent, DataGridDataset, MultiSelectResultDataset } from '@library';
import { UsersRolesDataset, UsersRolesResultDataset } from '../../../../../datasets';
import { IUsersDisplay } from '../../../../../models';
import { RolesMultiSelectModalComponent } from '../../../roles/modals';

@Component({
  selector: 'app-users-roles-child-list',
  templateUrl: './users-roles-child-list.html',
  styleUrls: ['./users-roles-child-list.scss'],
  imports: [
    ButtonEditComponent,
    ButtonRefreshComponent,
    DataGridComponent,
    RolesMultiSelectModalComponent,
  ],
  providers: [
    { provide: DataGridDataset, useClass: UsersRolesDataset },
    { provide: MultiSelectResultDataset, useClass: UsersRolesResultDataset }
  ]
})
export class UsersRolesChildListComponent extends ChildList<IUsersDisplay> {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  //#endregion

  //#region Private methods
  //#endregion
}
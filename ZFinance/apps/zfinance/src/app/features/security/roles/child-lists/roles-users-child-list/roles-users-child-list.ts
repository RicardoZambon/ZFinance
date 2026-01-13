import { Component, OnInit } from '@angular/core';
import { ButtonEditComponent, ButtonRefreshComponent, ChildList } from '@framework';
import { DataGridComponent, DataGridDataset, MultiSelectResultDataset } from '@library';
import { IRolesUpdate } from '../../../../..//models';
import { RolesUsersDataset, RolesUsersResultDataset } from '../../../../../datasets';
import { UsersMultiSelectModalComponent } from '../../../users/modals/users-multi-select-modal/users-multi-select-modal';

@Component({
  selector: 'app-roles-users-child-list',
  templateUrl: './roles-users-child-list.html',
  styleUrls: ['./roles-users-child-list.scss'],
  imports: [
    ButtonEditComponent,
    ButtonRefreshComponent,
    DataGridComponent,
    UsersMultiSelectModalComponent,
  ],
  providers: [
    { provide: DataGridDataset, useClass: RolesUsersDataset },
    { provide: MultiSelectResultDataset, useClass: RolesUsersResultDataset },
  ]
})
export class RolesUsersChildListComponent extends ChildList<IRolesUpdate> implements OnInit {
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
import { Component, OnInit } from '@angular/core';
import { ButtonEditComponent, ButtonRefreshComponent, ChildList } from '@framework';
import { DataGridComponent, DataGridDataset, MultiSelectResultDataset } from '@library';
import { RolesActionsDataset, RolesActionsResultDataset } from '../../../../../datasets';
import { IRolesUpdate } from '../../../../../models';
import { ActionsMultiSelectModalComponent } from '../../../actions/modals';

@Component({
  selector: 'app-roles-actions-child-list',
  templateUrl: './roles-actions-child-list.html',
  styleUrls: ['./roles-actions-child-list.scss'],
  imports: [
    ActionsMultiSelectModalComponent,
    ButtonEditComponent,
    ButtonRefreshComponent,
    DataGridComponent,
  ],
  providers: [
    { provide: DataGridDataset, useClass: RolesActionsDataset },
    { provide: MultiSelectResultDataset, useClass: RolesActionsResultDataset }
  ]
})
export class RolesActionsChildListComponent extends ChildList<IRolesUpdate> implements OnInit {
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
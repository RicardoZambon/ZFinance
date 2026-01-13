import { Component, OnInit } from '@angular/core';
import { ButtonEditComponent, ButtonRefreshComponent, ChildList } from '@framework';
import { DataGridComponent, DataGridDataset, MultiSelectResultDataset } from '@library';
import { RolesMenusDataset, RolesMenusResultDataset } from '../../../../../datasets';
import { IRolesUpdate } from '../../../../../models';
import { MenusMultiSelectModalComponent } from '../../../menus/modals';

@Component({
  selector: 'app-roles-menus-child-list',
  templateUrl: './roles-menus-child-list.html',
  styleUrls: ['./roles-menus-child-list.scss'],
  imports: [
    ButtonEditComponent,
    ButtonRefreshComponent,
    DataGridComponent,
    MenusMultiSelectModalComponent,
  ],
  providers: [
    { provide: DataGridDataset, useClass: RolesMenusDataset },
    { provide: MultiSelectResultDataset, useClass: RolesMenusResultDataset },
  ]
})
export class RolesMenusChildListComponent extends ChildList<IRolesUpdate> implements OnInit {
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
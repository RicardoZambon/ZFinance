import { Component } from '@angular/core';
import { MultiSelectModal } from '@framework';
import { DataGridDataset, MultiSelectComponent } from '@library';
import { TranslatePipe } from '@ngx-translate/core';
import { UsersDataset } from '../../../../../datasets';

@Component({
  selector: 'app-users-multi-select-modal',
  templateUrl: './users-multi-select-modal.html',
  imports: [
    MultiSelectComponent,
    TranslatePipe,
  ],
  providers: [{ provide: DataGridDataset, useClass: UsersDataset }]
})
export class UsersMultiSelectModalComponent extends MultiSelectModal {
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
import { Component, OnInit } from '@angular/core';
import { MultiSelectModal } from '@framework';
import { DataGridDataset, MultiSelectComponent } from '@library';
import { TranslatePipe } from '@ngx-translate/core';
import { RolesDataset } from '../../../../../datasets';

@Component({
  selector: 'app-roles-multi-select-modal',
  templateUrl: './roles-multi-select-modal.html',
  styleUrls: ['./roles-multi-select-modal.scss'],
  imports: [
    MultiSelectComponent,
    TranslatePipe
  ],
  providers: [{ provide: DataGridDataset, useClass: RolesDataset }]
})
export class RolesMultiSelectModalComponent extends MultiSelectModal implements OnInit {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  constructor(private dataGridDataset: DataGridDataset) {
    super();
  }

  public ngOnInit(): void {
    this.dataGridDataset.columns = [
      { field: 'name', headerName: 'Roles-Column-Name' },
    ];
  }
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  //#endregion

  //#region Private methods
  //#endregion
}
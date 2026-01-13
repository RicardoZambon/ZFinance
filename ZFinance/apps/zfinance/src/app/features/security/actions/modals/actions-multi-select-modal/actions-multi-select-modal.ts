import { Component, OnInit } from '@angular/core';
import { MultiSelectModal } from '@framework';
import { DataGridDataset, MultiSelectComponent } from '@library';
import { TranslatePipe } from '@ngx-translate/core';
import { ActionsDataset } from '../../../../../datasets';

@Component({
  selector: 'app-actions-multi-select-modal',
  templateUrl: './actions-multi-select-modal.html',
  styleUrls: ['./actions-multi-select-modal.scss'],
  imports: [
    MultiSelectComponent,
    TranslatePipe,
  ],
  providers: [{ provide: DataGridDataset, useClass: ActionsDataset }]
})
export class ActionsMultiSelectModalComponent extends MultiSelectModal implements OnInit {
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
      { field: 'entity', headerName: 'Actions-Column-Entity', size: 'minmax(0,0.6fr)' },
      { field: 'code', headerName: 'Actions-Column-Code' },
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
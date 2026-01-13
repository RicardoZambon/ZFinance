import { Component, OnInit } from '@angular/core';
import { MultiSelectModal } from '@framework';
import { DataGridDataset, MultiSelectComponent } from '@library';
import { TranslatePipe } from '@ngx-translate/core';
import { MenusDataset } from '../../../../../datasets';

@Component({
  selector: 'app-menus-multi-select-modal',
  templateUrl: './menus-multi-select-modal.html',
  styleUrls: ['./menus-multi-select-modal.scss'],
  imports: [
    MultiSelectComponent,
    TranslatePipe,
  ],
  providers: [{ provide: DataGridDataset, useClass: MenusDataset }]
})
export class MenusMultiSelectModalComponent extends MultiSelectModal implements OnInit {
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
      { field: 'label', headerName: 'Menus-Column-Label' },
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
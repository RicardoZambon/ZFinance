import { Component, OnInit } from '@angular/core';
import { ButtonDeleteComponent, ButtonNewComponent, ButtonOpenRecordComponent, ButtonRefreshComponent, TabViewList } from '@framework';
import { DataGridComponent, DataGridDataset, RibbonGroupComponent } from '@library';
import { TranslatePipe } from '@ngx-translate/core';
import { Observable, of } from 'rxjs';
import { ActionsDataset } from '../../../../datasets';
import { IActionsList } from '../../../../models';
import { ActionsService } from '../../../../services';
import { ActionsFilterComponent } from '../buttons/actions-filter/actions-filter';

@Component({
  selector: 'app-actions-list',
  templateUrl: './actions-list.html',
  styleUrls: ['./actions-list.scss'],
  imports: [
    ActionsFilterComponent,
    ButtonDeleteComponent,
    ButtonNewComponent,
    ButtonOpenRecordComponent,
    ButtonRefreshComponent,
    DataGridComponent,
    RibbonGroupComponent,
    TranslatePipe,
  ],
  providers: [{ provide: DataGridDataset, useClass: ActionsDataset }]
})
export class ActionsListComponent extends TabViewList<IActionsList> implements OnInit {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  constructor(protected actionsService: ActionsService) {
    super();
  }
  //#endregion

  //#region Event handlers
  public onDelete(): Observable<unknown> {
    if (!this.dataGridDataset.hasSelectedRows) {
      return of();
    }

    const selectedKey: string = this.dataGridDataset.selectedRowKeys[0];
    return this.actionsService.delete(this.dataGridDataset.getRowID(selectedKey));
  }
  //#endregion

  //#region Public methods
  //#endregion

  //#region Private methods
  //#endregion
}
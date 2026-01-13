import { Component, OnInit } from '@angular/core';
import { ButtonDeleteComponent, ButtonNewComponent, ButtonOpenRecordComponent, ButtonRefreshComponent, TabViewList } from '@framework';
import { DataGridComponent, DataGridDataset, RibbonGroupComponent } from '@library';
import { TranslatePipe } from '@ngx-translate/core';
import { Observable, of } from 'rxjs';
import { MenusDataset } from '../../../../datasets';
import { IMenusList } from '../../../../models';
import { MenusService } from '../../../../services';
import { MenusFilterComponent } from '../buttons/menus-filter/menus-filter';

@Component({
  selector: 'app-menus-list',
  templateUrl: './menus-list.html',
  styleUrls: ['./menus-list.scss'],
  imports: [
    ButtonDeleteComponent,
    ButtonNewComponent,
    ButtonOpenRecordComponent,
    ButtonRefreshComponent,
    DataGridComponent,
    MenusFilterComponent,
    RibbonGroupComponent,
    TranslatePipe,
  ],
  providers: [{ provide: DataGridDataset, useClass: MenusDataset }]
})
export class MenusListComponent extends TabViewList<IMenusList> implements OnInit {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  constructor(protected menusService: MenusService) {
    super();
  }
  //#endregion

  //#region Event handlers
  protected onDelete(): Observable<Object> {
    if (!this.dataGridDataset.hasSelectedRows) {
      return of();
    }

    const selectedKey: string = this.dataGridDataset.selectedRowKeys[0];
    return this.menusService.delete(this.dataGridDataset.getRowID(selectedKey));
  }
  //#endregion

  //#region Public methods
  //#endregion

  //#region Private methods
  //#endregion
}

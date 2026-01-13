import { Component, OnInit } from '@angular/core';
import { ButtonDeleteComponent, ButtonNewComponent, ButtonOpenRecordComponent, ButtonRefreshComponent, TabViewList } from '@framework';
import { DataGridComponent, DataGridDataset, RibbonGroupComponent } from '@library';
import { TranslatePipe } from '@ngx-translate/core';
import { Observable, of } from 'rxjs';
import { RolesDataset } from '../../../../datasets';
import { IRolesList } from '../../../../models';
import { RolesService } from '../../../../services';
import { RolesFilterComponent } from '../buttons';

@Component({
  selector: 'app-roles-list',
  templateUrl: './roles-list.html',
  styleUrls: ['./roles-list.scss'],
  imports: [
    ButtonDeleteComponent,
    ButtonNewComponent,
    ButtonOpenRecordComponent,
    ButtonRefreshComponent,
    DataGridComponent,
    RibbonGroupComponent,
    RolesFilterComponent,
    TranslatePipe,
  ],
  providers: [{ provide: DataGridDataset, useClass: RolesDataset }]
})
export class RolesListComponent extends TabViewList<IRolesList> implements OnInit {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  constructor(protected rolesService: RolesService) {
    super();
  }
  //#endregion

  //#region Event handlers
  public onDelete(): Observable<unknown> {
    if (!this.dataGridDataset.hasSelectedRows) {
      return of();
    }
    
    const selectedKey: string = this.dataGridDataset.selectedRowKeys[0];
    return this.rolesService.delete(this.dataGridDataset.getRowID(selectedKey));
  }
  //#endregion

  //#region Public methods
  //#endregion

  //#region Private methods
  //#endregion
}
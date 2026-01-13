import { Component, OnInit } from '@angular/core';
import { ButtonDeleteComponent, ButtonNewComponent, ButtonOpenRecordComponent, ButtonRefreshComponent, TabViewList } from '@framework';
import { DataGridComponent, DataGridDataset, RibbonGroupComponent } from '@library';
import { TranslatePipe } from '@ngx-translate/core';
import { Observable, of } from 'rxjs';
import { UsersDataset } from '../../../../datasets';
import { IUsersList } from '../../../../models';
import { UsersService } from '../../../../services';
import { UsersFilterComponent } from '../buttons';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.html',
  styleUrls: ['./users-list.scss'],
  imports: [
    ButtonDeleteComponent,
    ButtonNewComponent,
    ButtonOpenRecordComponent,
    ButtonRefreshComponent,
    DataGridComponent,
    RibbonGroupComponent,
    TranslatePipe,
    UsersFilterComponent,
  ],
  providers: [{ provide: DataGridDataset, useClass: UsersDataset }]
})
export class UsersListComponent extends TabViewList<IUsersList> {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  constructor(protected usersService: UsersService) {
    super();
  }
  //#endregion

  //#region Event handlers
  public onDelete(): Observable<unknown> {
    if (!this.dataGridDataset.hasSelectedRows) {
      return of();
    }

    const selectedKey: string = this.dataGridDataset.selectedRowKeys[0];
    return this.usersService.delete(this.dataGridDataset.getRowID(selectedKey));
  }
  //#endregion

  //#region Public methods
  //#endregion

  //#region Private methods
  //#endregion
}
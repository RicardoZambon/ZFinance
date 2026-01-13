import { Injectable } from '@angular/core';
import { DataGridDataset, IGridColumn, IListParameters } from '@library';
import { Observable } from 'rxjs';
import { IUsersList } from '../../../models';
import { UsersService } from '../../../services';

@Injectable()
export class UsersDataset extends DataGridDataset {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  public override columns: IGridColumn[] = [
    { field: 'name', headerName: 'Users-Column-Name' },
    { field: 'email', headerName: 'Users-Column-Email' },
  ];
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  constructor(protected usersService: UsersService) {
    super();
  }
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  public getData(params: IListParameters): Observable<IUsersList[]> {
    return this.usersService.list(params);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
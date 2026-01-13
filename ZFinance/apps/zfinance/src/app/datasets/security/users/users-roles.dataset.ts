import { Injectable } from '@angular/core';
import { DataGridDataset, IGridColumn, IListParameters } from '@library';
import { Observable, of } from 'rxjs';
import { IUsersRolesList } from '../../../models';
import { UsersService } from '../../../services';

@Injectable()
export class UsersRolesDataset extends DataGridDataset {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  public override columns: IGridColumn[] = [
    { field: 'name', headerName: 'Roles-Column-Name' },
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
  public getData(params: IListParameters): Observable<IUsersRolesList[]> {
    return !!this.dataProvider?.entityID
      ? this.usersService.listRoles(this.dataProvider.entityID, params)
      : of<IUsersRolesList[]>([]);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
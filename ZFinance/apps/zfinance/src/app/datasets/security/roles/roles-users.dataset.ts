import { Injectable } from '@angular/core';
import { DataGridDataset, IGridColumn, IListParameters } from '@library';
import { Observable, of } from 'rxjs';
import { IRolesUsersList } from '../../../models';
import { RolesService } from '../../../services';

@Injectable()
export class RolesUsersDataset extends DataGridDataset {
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
  constructor(protected rolesService: RolesService) {
    super();
  }
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  public getData(params: IListParameters): Observable<IRolesUsersList[]> {
    return !!this.parentEntityId
      ? this.rolesService.listUsers(this.parentEntityId, params)
      : of<IRolesUsersList[]>([]);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
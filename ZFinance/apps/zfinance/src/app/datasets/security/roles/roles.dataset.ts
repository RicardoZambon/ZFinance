import { Injectable } from '@angular/core';
import { DataGridDataset, IGridColumn, IListParameters } from '@library';
import { Observable } from 'rxjs';
import { IRolesList } from '../../../models';
import { RolesService } from '../../../services';

@Injectable()
export class RolesDataset extends DataGridDataset {
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
  constructor(protected rolesService: RolesService) {
    super();
  }
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  public getData(params: IListParameters): Observable<IRolesList[]> {
    return this.rolesService.list(params);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
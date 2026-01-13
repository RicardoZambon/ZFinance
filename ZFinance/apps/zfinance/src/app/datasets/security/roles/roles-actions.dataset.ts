import { Injectable } from '@angular/core';
import { DataGridDataset, IGridColumn, IListParameters } from '@library';
import { Observable, of } from 'rxjs';
import { IRolesActionsList } from '../../../models';
import { RolesService } from '../../../services';

@Injectable()
export class RolesActionsDataset extends DataGridDataset {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  public override columns: IGridColumn[] = [
    { field: 'entity', headerName: 'Actions-Column-Entity', size: 'minmax(0,0.7fr)' },
    { field: 'name', headerName: 'Actions-Column-Name' },
    { field: 'description', headerName: 'Actions-Column-Description' },
    { field: 'code', headerName: 'Actions-Column-Code' },
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
  public getData(params: IListParameters): Observable<IRolesActionsList[]> {
    return !!this.parentEntityId
      ? this.rolesService.listActions(this.parentEntityId, params)
      : of<IRolesActionsList[]>([]);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
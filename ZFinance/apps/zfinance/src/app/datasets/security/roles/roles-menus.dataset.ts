import { Injectable } from '@angular/core';
import { DataGridDataset, IGridColumn, IListParameters } from '@library';
import { Observable, of } from 'rxjs';
import { IRolesMenusList } from '../../../models';
import { RolesService } from '../../../services';

@Injectable()
export class RolesMenusDataset extends DataGridDataset {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  public override columns: IGridColumn[] = [
    { field: 'label', headerName: 'Menus-Column-Label' },
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
  public getData(params: IListParameters): Observable<IRolesMenusList[]> {
    return !!this.parentEntityId
      ? this.rolesService.listMenus(this.parentEntityId, params)
      : of<IRolesMenusList[]>([]);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
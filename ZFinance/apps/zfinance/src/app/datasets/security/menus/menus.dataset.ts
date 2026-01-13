import { Injectable } from '@angular/core';
import { DataGridDataset, IGridColumn, IListParameters } from '@library';
import { Observable } from 'rxjs';
import { IMenusList } from '../../../models';
import { MenusService } from '../../../services';

@Injectable()
export class MenusDataset extends DataGridDataset {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  public override columns: IGridColumn[] = [
    { field: 'label', headerName: 'Menus-Column-Label' },
    { field: 'icon', headerName: 'Menus-Column-Icon' },
    { field: 'url', headerName: 'Menus-Column-URL' },
    { field: 'order', headerName: 'Menus-Column-Order', size: 'minmax(5rem,min-content)' },
  ];
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  constructor(protected menusService: MenusService) {
    super();
  }
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  public getData(params: IListParameters): Observable<IMenusList[]> {
    return this.menusService.list(params);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
import { Injectable } from '@angular/core';
import { DataGridDataset, IGridColumn, IListParameters } from '@library';
import { Observable } from 'rxjs';
import { IActionsList } from '../../../models';
import { ActionsService } from '../../../services';

@Injectable()
export class ActionsDataset extends DataGridDataset {
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
  constructor(protected actionsService: ActionsService,
  ) {
    super();
  }
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  public getData(params: IListParameters): Observable<IActionsList[]> {
    return this.actionsService.list(params);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
import { Injectable } from '@angular/core';
import { IListParameters, IMultiSelectorChanges, MultiSelectResultDataset } from '@library';
import { Observable, of } from 'rxjs';
import { IRolesMenusList } from '../../../models';
import { RolesService } from '../../../services';

@Injectable()
export class RolesMenusResultDataset extends MultiSelectResultDataset {
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
  //#endregion

  //#region Public methods
  public getData(params: IListParameters): Observable<IRolesMenusList[]> {
    return !!this.parentEntityId
      ? this.rolesService.listMenus(this.parentEntityId, params)
      : of<IRolesMenusList[]>([]);
  }

  public saveData(changedIds: IMultiSelectorChanges): Observable<Object> {
    return this.rolesService.postMenus(this.parentEntityId, changedIds);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
import { Injectable } from '@angular/core';
import { IListParameters, IMultiSelectorChanges, MultiSelectResultDataset } from '@library';
import { Observable, of } from 'rxjs';
import { IRolesActionsList } from '../../../models';
import { RolesService } from '../../../services';

@Injectable()
export class RolesActionsResultDataset extends MultiSelectResultDataset {
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
  public getData(params: IListParameters): Observable<IRolesActionsList[]> {
    return !!this.parentEntityId
      ? this.rolesService.listActions(this.parentEntityId, params)
      : of<IRolesActionsList[]>([]);
  }

  public saveData(changedIds: IMultiSelectorChanges): Observable<Object> {
    return this.rolesService.postActions(this.parentEntityId, changedIds);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
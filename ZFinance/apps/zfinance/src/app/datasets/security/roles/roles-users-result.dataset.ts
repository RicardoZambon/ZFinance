import { Injectable } from '@angular/core';
import { IListParameters, IMultiSelectorChanges, MultiSelectResultDataset } from '@library';
import { Observable, of } from 'rxjs';
import { IRolesUsersList } from '../../../models';
import { RolesService } from '../../../services';

@Injectable()
export class RolesUsersResultDataset extends MultiSelectResultDataset {
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
  public getData(params: IListParameters): Observable<IRolesUsersList[]> {
    return !!this.parentEntityId
      ? this.rolesService.listUsers(this.parentEntityId, params)
      : of<IRolesUsersList[]>([]);
  }

  public saveData(changedIds: IMultiSelectorChanges): Observable<Object> {
    return this.rolesService.postUsers(this.parentEntityId, changedIds);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
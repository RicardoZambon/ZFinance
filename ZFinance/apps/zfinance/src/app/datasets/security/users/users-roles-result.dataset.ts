import { Injectable } from '@angular/core';
import { IListParameters, IMultiSelectorChanges, MultiSelectResultDataset } from '@library';
import { Observable, of } from 'rxjs';
import { IUsersRolesList } from '../../../models';
import { UsersService } from '../../../services';

@Injectable()
export class UsersRolesResultDataset extends MultiSelectResultDataset {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
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

  public saveData(changedIds: IMultiSelectorChanges): Observable<Object> {
    return this.usersService.postRoles(this.dataProvider?.entityID ?? 0, changedIds);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
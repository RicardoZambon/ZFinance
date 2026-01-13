import { Injectable } from '@angular/core';
import { DataProviderService } from '@library';
import { Observable, of } from 'rxjs';
import { IRolesInsert, IRolesUpdate } from '../../models';
import { RolesService } from '../../services';

@Injectable()
export class RolesDataProvider extends DataProviderService<IRolesUpdate> {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  constructor(private rolesService: RolesService) {
    super();
  }
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  public getTitle(entity: IRolesUpdate): string {
    return entity.name ?? '';
  }
  
  public override saveModel(model: IRolesInsert | IRolesUpdate): Observable<IRolesUpdate> {
    return !this.hasEntityID
      ? this.rolesService.put(model)
      : this.rolesService.post(<IRolesUpdate>{ id: this.entityID!, ...model });
  }
  //#endregion

  //#region Private methods
  protected override loadModel(entityID: number): Observable<IRolesUpdate | null> {
    if (entityID) {
      return this.rolesService.get(entityID);
    }
    return of(null);
  }
  //#endregion
}
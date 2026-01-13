import { Injectable } from '@angular/core';
import { DataProviderService } from '@library';
import { Observable, of } from 'rxjs';
import { IMenusDisplay, IMenusInsert, IMenusUpdate } from '../../models';
import { MenusService } from '../../services';

@Injectable()
export class MenusDataProvider extends DataProviderService<IMenusDisplay> {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  constructor(private menusService: MenusService) {
    super();
  }
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  public getTitle(entity: IMenusDisplay): string {
    return entity.label ?? '';
  }
  
  public override saveModel(model: IMenusInsert | IMenusUpdate): Observable<IMenusDisplay> {
    return !this.hasEntityID
      ? this.menusService.put(model)
      : this.menusService.post(<IMenusUpdate>{ id: this.entityID!, ...model });
  }
  //#endregion

  //#region Private methods
  protected override loadModel(entityID: number): Observable<IMenusDisplay | null> {
    if (entityID) {
      return this.menusService.get(entityID);
    }
    return of(null);
  }
  //#endregion
}
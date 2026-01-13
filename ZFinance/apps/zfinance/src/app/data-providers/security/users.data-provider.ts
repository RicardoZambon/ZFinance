import { Injectable } from '@angular/core';
import { DataProviderService } from '@library';
import { Observable, of } from 'rxjs';
import { IUsersDisplay, IUsersInsert, IUsersUpdate } from '../../models';
import { UsersService } from '../../services';

@Injectable()
export class UsersDataProvider extends DataProviderService<IUsersDisplay> {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  constructor(private usersService: UsersService) {
    super();
  }
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  public override getTitle(entity: IUsersDisplay): string {
    return entity.name ?? '';
  }
  
  public override saveModel(model: IUsersInsert | IUsersUpdate): Observable<IUsersDisplay> {
    return !this.hasEntityID
      ? this.usersService.put(model)
      : this.usersService.post(<IUsersUpdate>{ id: this.entityID!, ...model });
  }
  //#endregion

  //#region Private methods
  protected override loadModel(entityID: number): Observable<IUsersDisplay | null> {
    if (entityID) {
      return this.usersService.get(entityID);
    }
    return of(null);
  }
  //#endregion
}
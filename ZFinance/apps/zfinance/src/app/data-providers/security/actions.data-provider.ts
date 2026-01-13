import { Injectable } from '@angular/core';
import { DataProviderService } from '@library';
import { Observable, of } from 'rxjs';
import { IActionsInsert, IActionsUpdate } from '../../models';
import { ActionsService } from '../../services';

@Injectable()
export class ActionsDataProvider extends DataProviderService<IActionsUpdate> {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  constructor(private actionsService: ActionsService) {
    super();
  }
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  public getTitle(entity: IActionsUpdate): string {
    return entity.name ?? '';
  }
  
  public override saveModel(model: IActionsInsert | IActionsUpdate): Observable<IActionsUpdate> {
    return !this.hasEntityID
      ? this.actionsService.put(model)
      : this.actionsService.post(<IActionsUpdate>{ id: this.entityID!, ...model });
  }
  //#endregion

  //#region Private methods
  protected override loadModel(entityID: number): Observable<IActionsUpdate | null> {
    if (entityID) {
      return this.actionsService.get(entityID);
    }
    return of(null);
  }
  //#endregion
}
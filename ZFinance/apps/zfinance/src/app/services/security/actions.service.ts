import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IListParameters } from '@library';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IActionsInsert, IActionsList, IActionsUpdate } from '../../models';

const BASE_URL: string = `${environment.apiUrl}/Actions`;

@Injectable({
  providedIn: 'root'
})
export class ActionsService {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  constructor(private http: HttpClient) {
  }
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  public delete(actionID: number): Observable<Object> {
    return this.http.delete(`${BASE_URL}/${actionID}`);
  }

  public get(actionID: number): Observable<IActionsUpdate> {
    return this.http.get<IActionsUpdate>(`${BASE_URL}/${actionID}`);
  }

  public list(parameters: IListParameters): Observable<IActionsList[]> {
    return this.http.post<IActionsList[]>(`${BASE_URL}/List`, parameters);
  }

  public post(action: IActionsUpdate): Observable<IActionsUpdate> {
    return this.http.post<IActionsUpdate>(BASE_URL, action);
  }

  public put(action: IActionsInsert): Observable<IActionsUpdate> {
    return this.http.put<IActionsUpdate>(BASE_URL, action);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
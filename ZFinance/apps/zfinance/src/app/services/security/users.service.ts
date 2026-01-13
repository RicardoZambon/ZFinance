import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IListParameters, IMultiSelectorChanges } from '@library';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IUsersDisplay, IUsersInsert, IUsersList, IUsersRolesList, IUsersUpdate } from '../../models';

const BASE_URL: string = `${environment.apiUrl}/Users`;

@Injectable({
  providedIn: 'root'
})
export class UsersService {
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
  public delete(userID: number): Observable<Object> {
    return this.http.delete(`${BASE_URL}/${userID}`);
  }

  public get(userID: number): Observable<IUsersDisplay> {
    return this.http.get<IUsersDisplay>(`${BASE_URL}/${userID}`);
  }

  public list(parameters: IListParameters): Observable<IUsersList[]> {
    return this.http.post<IUsersList[]>(`${BASE_URL}/List`, parameters);
  }

  public listRoles(userID: number, parameters: IListParameters): Observable<IUsersRolesList[]> {
    return this.http.post<IUsersRolesList[]>(`${BASE_URL}/${userID}/Roles/List`, parameters);
  }

  public post(user: IUsersUpdate): Observable<IUsersDisplay> {
    return this.http.post<IUsersDisplay>(BASE_URL, user);
  }

  public postRoles(userID: number, relationship: IMultiSelectorChanges): Observable<Object> {
    return this.http.post(`${BASE_URL}/${userID}/Roles`, relationship);
  }

  public put(user: IUsersInsert): Observable<IUsersDisplay> {
    return this.http.put<IUsersDisplay>(BASE_URL, user);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
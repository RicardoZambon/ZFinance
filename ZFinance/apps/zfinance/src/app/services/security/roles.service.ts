import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IListParameters, IMultiSelectorChanges } from '@library';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IRolesActionsList, IRolesInsert, IRolesList, IRolesMenusList, IRolesUpdate, IRolesUsersList } from '../../models';

const BASE_URL: string = `${environment.apiUrl}/Roles`;

@Injectable({
  providedIn: 'root'
})
export class RolesService {
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
  public delete(roleID: number): Observable<Object> {
    return this.http.delete(`${BASE_URL}/${roleID}`);
  }

  public get(roleID: number): Observable<IRolesUpdate> {
    return this.http.get<IRolesUpdate>(`${BASE_URL}/${roleID}`);
  }

  public list(parameters: IListParameters): Observable<IRolesList[]> {
    return this.http.post<IRolesList[]>(`${BASE_URL}/List`, parameters);
  }

  public listActions(roleID: number, parameters: IListParameters): Observable<IRolesActionsList[]> {
    return this.http.post<IRolesActionsList[]>(`${BASE_URL}/${roleID}/Actions/List`, parameters);
  }

  public listMenus(roleID: number, parameters: IListParameters): Observable<IRolesMenusList[]> {
    return this.http.post<IRolesMenusList[]>(`${BASE_URL}/${roleID}/Menus/List`, parameters);
  }

  public listUsers(roleID: number, parameters: IListParameters): Observable<IRolesUsersList[]> {
    return this.http.post<IRolesUsersList[]>(`${BASE_URL}/${roleID}/Users/List`, parameters);
  }

  public post(role: IRolesUpdate): Observable<IRolesUpdate> {
    return this.http.post<IRolesUpdate>(BASE_URL, role);
  }

  public postActions(roleID: number, relationship: IMultiSelectorChanges): Observable<Object> {
    return this.http.post(`${BASE_URL}/${roleID}/Actions`, relationship);
  }

  public postMenus(roleID: number, relationship: IMultiSelectorChanges): Observable<Object> {
    return this.http.post(`${BASE_URL}/${roleID}/Menus`, relationship);
  }

  public postUsers(roleID: number, relationship: IMultiSelectorChanges): Observable<Object> {
    return this.http.post(`${BASE_URL}/${roleID}/Users`, relationship);
  }

  public put(role: IRolesInsert): Observable<IRolesUpdate> {
    return this.http.put<IRolesUpdate>(BASE_URL, role);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
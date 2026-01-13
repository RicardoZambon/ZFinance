import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IListParameters } from '@library';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IMenusDisplay, IMenusInsert, IMenusList, IMenusUpdate } from '../../models';

const BASE_URL: string = `${environment.apiUrl}/Menus`;

@Injectable({
  providedIn: 'root'
})
export class MenusService {
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
  public delete(menuID: number): Observable<Object> {
    return this.http.delete(`${BASE_URL}/${menuID}`);
  }

  public get(menuID: number): Observable<IMenusDisplay> {
    return this.http.get<IMenusDisplay>(`${BASE_URL}/${menuID}`);
  }

  public list(parameters: IListParameters): Observable<IMenusList[]> {
    return this.http.post<IMenusList[]>(`${BASE_URL}/List`, parameters);
  }

  public post(menu: IMenusUpdate): Observable<IMenusDisplay> {
    return this.http.post<IMenusDisplay>(`${BASE_URL}`, menu);
  }

  public put(menu: IMenusInsert): Observable<IMenusDisplay> {
    return this.http.put<IMenusDisplay>(`${BASE_URL}`, menu);
  }
  //#endregion

  //#region Private methods
  //#endregion
}
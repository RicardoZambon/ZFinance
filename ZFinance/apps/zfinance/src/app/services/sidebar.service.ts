import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ISidebarProfile, SidebarMenu, SidebarService } from '@library';
import { AuthenticationService } from '@shared';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ICurrentUserInfo } from '../models';

const BASE_URL: string = `${environment.apiUrl}/Menus`;

@Injectable({
  providedIn: 'root'
})
export class AppSidebarService extends SidebarService {
  //#region Variables
  //#endregion

  //#region Properties
  //#endregion

  //#region Constructor and Angular life cycle methods
  constructor(
    private authenticationService: AuthenticationService,
    private http: HttpClient,
  ) {
    super();
  }
  //#endregion

  //#region Public methods
  public override getMenuFromUrl(url: string): Observable<SidebarMenu> {
    return this.http.get<SidebarMenu>(`${BASE_URL}/GetItem`, { params: { url } });
  }

  public override getUserProfile(): ISidebarProfile | undefined {
    const userInfo: ICurrentUserInfo = <ICurrentUserInfo>this.authenticationService.getUserInfo();

    return {
      image: '/profile-image-test.png',
      name: userInfo?.name,
      title: userInfo?.username,
    } as ISidebarProfile;
  }
  //#endregion

  //#region Private methods
  protected override loadMenus(parentMenu: SidebarMenu | null): Observable<SidebarMenu[]> {
    return this.http.post<SidebarMenu[]>(`${BASE_URL}/Drawer${!!parentMenu ? `/${parentMenu.id}` : ''}`, { });
  }
  //#endregion
}
import { registerLocaleData } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpBackend, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import localeEN from '@angular/common/locales/en';
import localeES from '@angular/common/locales/es';
import localeENExtra from '@angular/common/locales/extra/en';
import localeESExtra from '@angular/common/locales/extra/es';
import localePTExtra from '@angular/common/locales/extra/pt';
import localePT from '@angular/common/locales/pt';
import { ApplicationConfig, forwardRef, importProvidersFrom, LOCALE_ID, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, RouteReuseStrategy } from '@angular/router';
import { JwtInterceptor, JwtModule } from '@auth0/angular-jwt';
import { APP_CONFIG, AuthService, CustomReuseStrategy, FrameworkGridConfigsProvider } from '@framework';
import { GridConfigsProvider, SIDEBAR_CONFIGS, SidebarConfigs, SidebarService } from '@library';
import { provideTranslateService, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { AuthenticationService, AuthInterceptor } from '@shared';
import { provideMarkdown } from 'ngx-markdown';
import { environment } from '../environments/environment';
import { routes } from './app.routes';
import { httpLoaderFactory } from './i18n';
import { AppSidebarService } from './services';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideTranslateService({
      loader: {
        provide: TranslateLoader,
        useFactory: httpLoaderFactory,
        deps: [HttpBackend],
      },
    }),
    provideMarkdown(),
    importProvidersFrom(
      JwtModule.forRoot({
        config: {
          tokenGetter: () => localStorage.getItem('token') ?? sessionStorage.getItem('token'),
          allowedDomains: [
            (environment.apiUrl.match(/(?:^https?:\/\/([^\/]+)(?:[\/,]|$)|^(.*)$)/) ?? ['', ''] )[1]
          ],
          disallowedRoutes: [
            `${environment.apiUrl}/Authentication/RefreshToken`,
            `${environment.apiUrl}/Authentication/SignIn`
          ],
        }
      })
    ),
    JwtInterceptor,
    { provide: APP_CONFIG, useValue: { BASE_URL: environment.apiUrl } },
    { provide: AuthService, useExisting: forwardRef(() => AuthenticationService) },
    { provide: GridConfigsProvider, useClass: FrameworkGridConfigsProvider },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    {
      provide: LOCALE_ID,
      deps: [TranslateService],
      useFactory: (translateService: TranslateService) => {
        translateService.addLangs(['pt', 'en']);

        const defaultLang: string = translateService.langs[0];
        translateService.setDefaultLang(defaultLang);

        const language: string = localStorage.getItem('language')
          // ?? translate.getBrowserLang()
          ?? defaultLang;

        translateService.use(translateService.langs.includes(language) ? language : defaultLang);
        
        switch (translateService.currentLang) {
          case "en":
            registerLocaleData(localeEN, 'en', localeENExtra);
            break;
          case "es":
            registerLocaleData(localeES, 'es', localeESExtra);
            break;
          default:
            registerLocaleData(localePT, 'pt', localePTExtra);
            break;
        }
        return translateService.currentLang;
      }
    },
    { provide: RouteReuseStrategy, useClass: CustomReuseStrategy },
    {
      provide: SIDEBAR_CONFIGS,
      useValue: new SidebarConfigs({
        logoCollapsedPath: '/logo-small-menu.png',
        logoExpandedPath: '/logo-menu.png',
      })
    },
    { provide: SidebarService, useClass: AppSidebarService },
  ],
};
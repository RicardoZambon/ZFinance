import { HttpBackend } from '@angular/common/http';
import { MultiTranslateHttpLoader } from 'ngx-translate-multi-http-loader';

export function httpLoaderFactory(httpBackend: HttpBackend): MultiTranslateHttpLoader {
  return new MultiTranslateHttpLoader(httpBackend, [
    // #################
    // #### Modules ####
    // #################

    // Framework
    './assets/i18n/framework/',

    // Shared
    './assets/i18n/shared/language-selector/',
    './assets/i18n/shared/login/',
    './assets/i18n/shared/operations-history/',

    // ##################
    // #### ZFinance ####
    // ##################

    './assets/i18n/zfinance/',

    // Configurations
    './assets/i18n/zfinance/configs/',

    // Dashboards
    './assets/i18n/zfinance/dashboards/',

    // Transactions
    './assets/i18n/zfinance/transactions/',

    // Security
    './assets/i18n/zfinance/security/actions/',
    './assets/i18n/zfinance/security/menus/',
    './assets/i18n/zfinance/security/roles/',
    './assets/i18n/zfinance/security/users/',
    './assets/i18n/zfinance/security/',
  ]);
}
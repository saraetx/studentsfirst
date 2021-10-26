import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserCacheLocation, Configuration, InteractionType, PublicClientApplication } from '@azure/msal-browser';
import { environment } from '../environments/environment';
import { MsalGuardConfiguration, MsalInterceptor, MsalInterceptorConfiguration, MsalModule, ProtectedResourceScopes } from '@azure/msal-angular';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

const protectedResourceMap: [string, (string | ProtectedResourceScopes)[] | null][] = [
  [`${environment.apiBase}/*`, [`${environment.apiAppUri}/login`]]
];

const msalConfig: Configuration = {
  auth: environment.auth,
  cache: {
    cacheLocation: BrowserCacheLocation.LocalStorage
  }
};

const msalGuardConfig: MsalGuardConfiguration = {
  interactionType: InteractionType.Redirect,
  authRequest: {
    scopes: [`${environment.apiAppUri}/login`]
  }
};

const msalInterceptorConfig: MsalInterceptorConfiguration = {
  interactionType: InteractionType.Popup,
  protectedResourceMap: new Map(protectedResourceMap)
}

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MsalModule.forRoot(new PublicClientApplication(msalConfig), msalGuardConfig, msalInterceptorConfig)
  ],
  exports: [
    MsalModule
  ]
})
export class AppAuthenticationModule { }

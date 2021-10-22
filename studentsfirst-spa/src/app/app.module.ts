import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { EffectsModule } from '@ngrx/effects';
import { StoreRouterConnectingModule } from '@ngrx/router-store';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from 'src/environments/environment';
import { ENVIRONMENT } from 'src/environments/environment.token';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { appEffects } from './state/app.effects';
import { appReducers } from './state/app.reducers';
import { routerConfig } from './state/router/router.config';
import { AppAuthenticationModule } from './app-authentication.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { MsalBroadcastService, MsalGuard, MsalInterceptor, MsalRedirectComponent, MsalService } from '@azure/msal-angular';
import { CoreModule } from './core/core.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    CoreModule
  ],
  providers: [
    { provide: ENVIRONMENT, useValue: environment }
  ],
  bootstrap: [AppComponent, MsalRedirectComponent]
})
export class AppModule { }

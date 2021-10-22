import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { StoreRouterConnectingModule } from '@ngrx/router-store';
import { EffectsModule } from '@ngrx/effects';

import { AppAuthenticationModule } from '../app-authentication.module';
import { AppRoutingModule } from '../app-routing.module';
import { appReducers } from '../state/app.reducers';
import { routerConfig } from '../state/router/router.config';
import { appEffects } from '../state/app.effects';
import { LayoutModule } from '../features/layout/layout.module';

@NgModule({
  declarations: [],
  imports: [
    BrowserModule,
    AppAuthenticationModule,
    AppRoutingModule,
    StoreModule.forRoot(appReducers),
    StoreDevtoolsModule.instrument(),
    StoreRouterConnectingModule.forRoot(routerConfig),
    EffectsModule.forRoot(appEffects),
    LayoutModule
  ],
  exports: [
    BrowserModule,
    AppAuthenticationModule,
    AppRoutingModule,
    LayoutModule
  ]
})
export class CoreModule { }

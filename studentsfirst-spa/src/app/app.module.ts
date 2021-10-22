import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { EffectsModule } from '@ngrx/effects';
import { StoreRouterConnectingModule } from '@ngrx/router-store';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { appEffects } from './state/app.effects';
import { appReducers } from './state/app.reducers';
import { routerConfig } from './state/router/router.config';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    StoreModule.forRoot(appReducers),
    StoreDevtoolsModule.instrument(),
    StoreRouterConnectingModule.forRoot(routerConfig),
    EffectsModule.forRoot(appEffects)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

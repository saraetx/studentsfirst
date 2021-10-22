import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MsalGuard, MsalInterceptor } from '@azure/msal-angular';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppAuthenticationModule } from '../app-authentication.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    AppAuthenticationModule
  ],
  providers: [
    MsalGuard,
    { provide: HTTP_INTERCEPTORS, useClass: MsalInterceptor, multi: true }
  ]
})
export class SharedModule { }

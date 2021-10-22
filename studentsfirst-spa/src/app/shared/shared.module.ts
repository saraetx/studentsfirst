import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MsalGuard, MsalInterceptor } from '@azure/msal-angular';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppAuthenticationModule } from '../app-authentication.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    AppAuthenticationModule,
    RouterModule
  ],
  exports: [
    CommonModule,
    RouterModule
  ],
  providers: [
    MsalGuard,
    { provide: HTTP_INTERCEPTORS, useClass: MsalInterceptor, multi: true }
  ]
})
export class SharedModule { }

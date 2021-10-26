import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MsalGuard, MsalInterceptor } from '@azure/msal-angular';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppAuthenticationModule } from '../app-authentication.module';
import { EntityListControlsComponent } from './components/entity-list-controls/entity-list-controls.component';
import { ReactiveFormsModule } from '@angular/forms';
import { PaginationComponent } from './components/pagination/pagination.component';

@NgModule({
  declarations: [
    EntityListControlsComponent,
    PaginationComponent
  ],
  imports: [
    CommonModule,
    AppAuthenticationModule,
    RouterModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  exports: [
    CommonModule,
    RouterModule,
    EntityListControlsComponent
  ],
  providers: [
    MsalGuard,
    { provide: HTTP_INTERCEPTORS, useClass: MsalInterceptor, multi: true }
  ]
})
export class SharedModule { }

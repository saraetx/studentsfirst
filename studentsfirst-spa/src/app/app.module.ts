import { NgModule } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ENVIRONMENT } from 'src/environments/environment.token';
import { MsalRedirectComponent } from '@azure/msal-angular';

import { AppComponent } from './app.component';
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

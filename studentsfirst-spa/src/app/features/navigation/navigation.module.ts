import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';
import { NavigationFacade } from './facades/navigation.facade';
import { NavigationBarComponent } from './components/navigation-bar/navigation-bar.component';
import { NavigationBarContainerComponent } from './containers/navigation-bar-container/navigation-bar-container.component';

@NgModule({
  declarations: [
    NavigationBarComponent,
    NavigationBarContainerComponent
  ],
  exports: [
    NavigationBarContainerComponent
  ],
  imports: [
    SharedModule
  ],
  providers: [
    NavigationFacade
  ]
})
export class NavigationModule { }

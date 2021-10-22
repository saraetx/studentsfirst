import { NgModule } from '@angular/core';
import { LayoutContainerComponent } from './containers/layout-container/layout-container.component';
import { LayoutComponent } from './components/layout/layout.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { NavigationModule } from '../navigation/navigation.module';

@NgModule({
  declarations: [
    LayoutContainerComponent,
    LayoutComponent
  ],
  imports: [
    SharedModule,
    NavigationModule
  ],
  exports: [
    LayoutContainerComponent
  ]
})
export class LayoutModule { }

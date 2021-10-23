import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GroupsRoutingModule } from './groups-routing.module';
import { GroupsApiService } from './services/groups-api.service';


@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    GroupsRoutingModule
  ],
  providers: [
    GroupsApiService
  ]
})
export class GroupsModule { }

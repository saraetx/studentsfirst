import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

import { GroupsRoutingModule } from './groups-routing.module';
import { GroupsApiService } from './services/groups-api.service';
import { GROUPS_STATE_TOKEN } from './state/groups.state-token';
import { groupsReducers } from './state/groups.reducers';
import { groupsEffects } from './state/groups.effects';


@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    GroupsRoutingModule,
    StoreModule.forFeature(GROUPS_STATE_TOKEN, groupsReducers),
    EffectsModule.forFeature(groupsEffects)
  ],
  providers: [
    GroupsApiService
  ]
})
export class GroupsModule { }

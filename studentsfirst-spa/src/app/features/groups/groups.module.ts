import { NgModule } from '@angular/core';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

import { SharedModule } from '../../shared/shared.module';
import { GroupsRoutingModule } from './groups-routing.module';
import { GroupsApiService } from './services/groups-api.service';
import { GroupsListContainerComponent } from './containers/groups-list-container/groups-list-container.component';
import { GROUPS_STATE_TOKEN } from './state/groups.state-token';
import { groupsReducers } from './state/groups.reducers';
import { groupsEffects } from './state/groups.effects';
import { GroupsFacade } from './facades/groups.facade';


@NgModule({
  declarations: [
    GroupsListContainerComponent
  ],
  imports: [
    SharedModule,
    GroupsRoutingModule,
    StoreModule.forFeature(GROUPS_STATE_TOKEN, groupsReducers),
    EffectsModule.forFeature(groupsEffects)
  ],
  providers: [
    GroupsApiService,
    GroupsFacade
  ]
})
export class GroupsModule { }

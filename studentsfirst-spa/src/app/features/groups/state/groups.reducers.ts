import { ActionReducerMap } from '@ngrx/store';

import { groupsEntitiesReducer } from './groups-entities/groups-entities.reducer';
import { GroupsState } from './groups.state';

export const groupsReducers: ActionReducerMap<GroupsState> = {
  groupsEntities: groupsEntitiesReducer
};

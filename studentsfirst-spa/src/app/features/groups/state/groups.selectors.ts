import { createFeatureSelector } from '@ngrx/store';

import { GroupsState } from './groups.state';
import { GROUPS_STATE_TOKEN } from './groups.state-token';

export const selectGroupsFeatureState = createFeatureSelector<GroupsState>(GROUPS_STATE_TOKEN);

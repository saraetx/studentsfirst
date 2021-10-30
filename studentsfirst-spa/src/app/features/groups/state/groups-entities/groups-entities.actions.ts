import { createAction, props } from '@ngrx/store';
import { Group } from '../../models/group.model';

export const setGroupsPagingFilter = createAction(
  'studentsfirst-spa/groups/groups-entities/set-groups-paging-filter',
  props<{ nameIncludes: string, ownOnly: boolean }>()
);

export const setGroupsPagingOptions = createAction(
  'studentsfirst-spa/groups/groups-entities/set-groups-paging-options',
  props<{ skip: number, take: number }>()
);

export const loadPagedGroups = createAction('studentsfirst-spa/groups/groups-entities/load-paged-groups');

export const loadPagedGroupsSuccess = createAction(
  'studentsfirst-spa/groups/groups-entities/load-paged-groups-success',
  props<{ data: Group[], total: number }>()
);

export const loadPagedGroupsFail = createAction('studentsfirst-spa/groups/groups-entities/load-paged-groups-fail');

export const unloadPagedGroups = createAction('studentsfirst-spa/groups/groups-entities/unload-paged-groups');

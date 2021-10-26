import { createReducer, on } from '@ngrx/store';
import { convertListToEntities } from '../../../../shared/utils/convert-list-to-entities';
import { loadPagedGroupsSuccess, setPagingOptions, unloadPagedGroups } from './groups-entities.actions';
import { groupsEntitiesInitialState } from './groups-entities.state';

export const groupsEntitiesReducer = createReducer(
  groupsEntitiesInitialState,
  on(setPagingOptions, (state, { skip, take }) => ({
    ...state,
    pagingSkip: skip,
    pagingTake: take
  })),
  on(loadPagedGroupsSuccess, (state, { data, total }) => ({
    ...state,
    entitiesMap: convertListToEntities(
      Object.values(state.entitiesMap)
        .filter(group => state.unpagedEntityIds.includes(group.id))
        .concat(data)
    ),
    pagedEntitiesTotalCount: total,
    pagedEntitiesLoaded: true,
    pagedEntityIds: data.map(g => g.id)
  })),
  on(unloadPagedGroups, state => ({
    ...state,
    entitiesMap: convertListToEntities(
      Object.values(state.entitiesMap).filter(group => state.unpagedEntityIds.includes(group.id))
    ),
    pagedEntitiesLoaded: false,
    pagedEntityIds: []
  }))
);

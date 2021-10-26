import { createSelector } from '@ngrx/store';
import { selectGroupsFeatureState } from '../groups.selectors';

export const selectGroupsEntities = createSelector(
  selectGroupsFeatureState,
  groupsFeatureState => groupsFeatureState.groupsEntities
);

export const selectGroupsEntityMap = createSelector(
  selectGroupsEntities,
  groupsEntities => groupsEntities.entitiesMap
);

export const selectGroupsPagedEntitiesLoaded = createSelector(
  selectGroupsEntities,
  groupsEntities => groupsEntities.pagedEntitiesLoaded
);

export const selectGroupsPagedEntitiesSkip = createSelector(
  selectGroupsEntities,
  groupsEntities => groupsEntities.pagingSkip
);

export const selectGroupsPagedEntitiesTake = createSelector(
  selectGroupsEntities,
  groupsEntities => groupsEntities.pagingTake
);

export const selectPagedGroupsIds = createSelector(
  selectGroupsEntities,
  groupsEntities => groupsEntities.pagedEntityIds
);

export const selectUnpagedGroupsIds = createSelector(
  selectGroupsEntities,
  groupsEntities => groupsEntities.unpagedEntityIds
);

export const selectPagedGroupsTotalCount = createSelector(
  selectGroupsEntities,
  groupsEntities => groupsEntities.pagedEntitiesTotalCount
);

export const selectPagedGroups = createSelector(
  selectPagedGroupsIds,
  selectGroupsEntityMap,
  (groupsPagedEntityIds, groupsEntityMap) => groupsPagedEntityIds.map(id => groupsEntityMap[id])
)

import { Group } from '../../models/group.model';

export interface GroupsEntitiesState {
  entitiesMap: { [id: string]: Group },
  pagedEntitiesLoaded: boolean,
  pagedEntityIds: string[],
  unpagedEntityIds: string[],
  pagedEntitiesTotalCount: number,
  pagingSkip: number,
  pagingTake: number,
  pagingFilterNameIncludes: string,
  pagingFilterOwnOnly: boolean
}

export const groupsEntitiesInitialState: GroupsEntitiesState = {
  entitiesMap: {},
  pagedEntitiesLoaded: false,
  pagedEntityIds: [],
  unpagedEntityIds: [],
  pagingFilterNameIncludes: '',
  pagingFilterOwnOnly: false,
  pagedEntitiesTotalCount: 0,
  pagingSkip: 0,
  pagingTake: 0
};

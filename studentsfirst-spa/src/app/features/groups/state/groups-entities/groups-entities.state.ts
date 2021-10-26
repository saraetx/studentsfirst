import { Group } from '../../models/group.model';

export interface GroupsEntitiesState {
  entitiesMap: { [id: string]: Group },
  pagedEntitiesLoaded: boolean,
  pagedEntityIds: string[],
  unpagedEntityIds: string[],
  pagingSkip: number,
  pagingTake: number
}

export const groupsEntitiesInitialState: GroupsEntitiesState = {
  entitiesMap: {},
  pagedEntitiesLoaded: false,
  pagedEntityIds: [],
  pagingSkip: 0,
  pagingTake: 0
};

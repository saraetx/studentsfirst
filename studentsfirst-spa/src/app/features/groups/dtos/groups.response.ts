import { Group } from '../models/group.model';

export interface GroupsResponse {
  groups: Group[];
  filtering: boolean;
  total: number;
  skipping: number;
  taking: number;
}

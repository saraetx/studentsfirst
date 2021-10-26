import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ENVIRONMENT } from '../../../../environments/environment.token';
import { EnvironmentInterface } from '../../../../environments/environment.interface';
import { Group } from '../models/group.model';
import { CreateGroupRequest } from '../dtos/create-group.request';
import { FindAllGroupsParameters } from '../dtos/find-all-groups.parameters';
import { GroupsResponse } from '../dtos/groups.response';

@Injectable()
export class GroupsApiService {
  public constructor(
    private readonly httpClient: HttpClient,
    @Inject(ENVIRONMENT) private readonly environment: EnvironmentInterface
  ) { }

  public findAll(query: FindAllGroupsParameters = {}): Observable<GroupsResponse> {
    return this.httpClient.get<GroupsResponse>(`${this.environment.apiBase}/groups`, { params: { ...query } });
  }

  public findOne(id: string): Observable<Group> {
    return this.httpClient.get<Group>(`${this.environment.apiBase}/groups/${id}`);
  }

  public create(group: CreateGroupRequest): Observable<Group> {
    return this.httpClient.post<Group>(`${this.environment.apiBase}/groups`, group);
  }
}

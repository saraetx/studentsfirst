import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import { ENVIRONMENT } from '../../../../environments/environment.token';
import { EnvironmentInterface } from '../../../../environments/environment.interface';
import { Group } from '../models/group.model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CreateGroupRequest } from '../dtos/create-group.request';

@Injectable()
export class GroupsApiService {
  public constructor(
    private readonly httpClient: HttpClient,
    @Inject(ENVIRONMENT) private readonly environment: EnvironmentInterface
  ) { }

  public findAll(): Observable<Group[]> {
    return this.httpClient.get<Group[]>(`${environment.apiBase}/groups`);
  }

  public findOne(id: string): Observable<Group> {
    return this.httpClient.get<Group>(`${environment.apiBase}/groups/${id}`);
  }

  public create(group: CreateGroupRequest): Observable<Group> {
    return this.httpClient.post<Group>(`${environment.apiBase}/groups`, group);
  }
}

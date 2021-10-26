import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { catchError, filter, map, switchMap, switchMapTo, withLatestFrom } from 'rxjs/operators';

import { GroupsApiService } from '../../services/groups-api.service';
import { loadPagedGroups, loadPagedGroupsFail, loadPagedGroupsSuccess, setPagingOptions } from './groups-entities.actions';
import { selectGroupsPagedEntitiesLoaded, selectGroupsPagedEntitiesSkip, selectGroupsPagedEntitiesTake } from './groups-entities.selectors';

@Injectable()
export class GroupsEntitiesEffects {
  public constructor(
    private readonly actions: Actions,
    private readonly store: Store,
    private readonly groupsApiService: GroupsApiService
  ) { }

  public setPagingOptionsEffect$ = createEffect(() => this.actions.pipe(
    ofType(setPagingOptions),
    switchMapTo(this.store.select(selectGroupsPagedEntitiesLoaded)),
    filter(pagedEntitiesLoaded => pagedEntitiesLoaded),
    map(() => loadPagedGroups())
  ));

  public loadPagedGroupsEffect$ = createEffect(() => this.actions.pipe(
    ofType(loadPagedGroups),
    withLatestFrom(this.store.select(selectGroupsPagedEntitiesSkip), this.store.select(selectGroupsPagedEntitiesTake)),
    switchMap(([_, skip, take]) => this.groupsApiService.findAll({ skip, take }).pipe(
      map(data => loadPagedGroupsSuccess({ data })),
      catchError(() => of(loadPagedGroupsFail()))
    ))
  ));
}

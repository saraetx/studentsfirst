import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { of, pipe } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, filter, map, switchMap, withLatestFrom } from 'rxjs/operators';

import { GroupsApiService } from '../../services/groups-api.service';
import { loadPagedGroups, loadPagedGroupsFail, loadPagedGroupsSuccess, setGroupsPagingFilter, setGroupsPagingOptions } from './groups-entities.actions';
import { selectGroupsPagedEntitiesLoaded, selectPagedGroupsNameIncludes, selectPagedGroupsOwnOnly, selectPagedGroupsSkip, selectPagedGroupsTake } from './groups-entities.selectors';

@Injectable()
export class GroupsEntitiesEffects {
  public constructor(
    private readonly actions: Actions,
    private readonly store: Store,
    private readonly groupsApiService: GroupsApiService
  ) { }

  public setGroupsPagingOptionsEffect$ = createEffect(() => this.actions.pipe(
    ofType(setGroupsPagingOptions),
    withLatestFrom(this.store.select(selectGroupsPagedEntitiesLoaded)),
    filter(([_, pagedEntitiesLoaded]) => pagedEntitiesLoaded),
    map(() => loadPagedGroups())
  ));

  public setGroupsPagingFilterEffect$ = createEffect(() => this.actions.pipe(
    ofType(setGroupsPagingFilter),
    withLatestFrom(this.store.select(selectGroupsPagedEntitiesLoaded)),
    filter(([_, pagedEntitiesLoaded]) => pagedEntitiesLoaded),
    debounceTime(250),
    map(() => loadPagedGroups())
  ));

  public loadPagedGroupsEffect$ = createEffect(() => this.actions.pipe(
    ofType(loadPagedGroups),
    withLatestFrom(
      this.store.select(selectPagedGroupsNameIncludes),
      this.store.select(selectPagedGroupsOwnOnly),
      this.store.select(selectPagedGroupsSkip),
      this.store.select(selectPagedGroupsTake)
    ),
    switchMap(
      ([_, nameIncludes, ownOnly, skip, take]) =>
        this.groupsApiService.findAll({ nameIncludes: nameIncludes, ownOnly, skip, take }).pipe(
          map(({ groups, total }) => loadPagedGroupsSuccess({ data: groups, total })),
          catchError(() => of(loadPagedGroupsFail()))
        )
    )
  ));
}

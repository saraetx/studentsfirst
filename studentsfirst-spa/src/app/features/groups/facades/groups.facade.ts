import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { BehaviorSubject, combineLatest, Observable, ReplaySubject, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, take } from 'rxjs/operators';

import { Group } from '../models/group.model';
import { loadPagedGroups, setGroupsPagingFilter, setGroupsPagingOptions, unloadPagedGroups } from '../state/groups-entities/groups-entities.actions';
import { selectPagedGroups, selectPagedGroupsTotalCount } from '../state/groups-entities/groups-entities.selectors';

@Injectable()
export class GroupsFacade {
  public constructor(private readonly store: Store) { }

  public setFilter(nameIncludes: string, ownOnly: boolean) {
    this.store.dispatch(setGroupsPagingFilter({ nameIncludes, ownOnly }));
  }

  public setPaging(skip: number, take: number): void {
    this.store.dispatch(setGroupsPagingOptions({ skip, take }));
  }

  public getAllGroupsInPage$(): Observable<Group[]> {
    this.store.dispatch(loadPagedGroups());
    return this.store.select(selectPagedGroups);
  }

  public selectGroupsTotalCount$(): Observable<number> {
    return this.store.select(selectPagedGroupsTotalCount);
  }

  public clearLoadedGroups$(): void {
    this.store.dispatch(unloadPagedGroups());
  }
}

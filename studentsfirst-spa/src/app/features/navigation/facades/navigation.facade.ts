import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { NavigationItem } from '../models/navigation-item.model';

@Injectable()
export class NavigationFacade {
  public constructor() { }

  public getNavigationItems$(): Observable<NavigationItem[]> {
    // @TODO
    // Avoid hard coding navigation items.
    return of([
      { name: 'Groups', routerLink: ['groups'], exactMatch: false }
    ]);
  }
}

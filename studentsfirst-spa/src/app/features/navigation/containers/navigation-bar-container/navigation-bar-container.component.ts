import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { NavigationFacade } from '../../facades/navigation.facade';
import { NavigationItem } from '../../models/navigation-item.model';

@Component({
  selector: 'app-navigation-bar-container',
  templateUrl: './navigation-bar-container.component.html',
  styleUrls: ['./navigation-bar-container.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavigationBarContainerComponent implements OnInit {
  public constructor(private readonly navigationFacade: NavigationFacade) { }

  public navigationItems$?: Observable<NavigationItem[]>;

  public ngOnInit(): void {
    this.navigationItems$ = this.navigationFacade.getNavigationItems$();
  }
}

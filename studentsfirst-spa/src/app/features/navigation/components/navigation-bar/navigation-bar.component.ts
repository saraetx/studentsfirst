import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { NavigationItem } from '../../models/navigation-item.model';

@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrls: ['./navigation-bar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavigationBarComponent {
  @Input()
  public navigationItems?: NavigationItem[];
}

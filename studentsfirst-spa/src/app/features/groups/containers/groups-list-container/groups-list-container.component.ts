import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { GroupsFacade } from '../../facades/groups.facade';
import { Group } from '../../models/group.model';

@Component({
  selector: 'app-groups-list-container',
  templateUrl: './groups-list-container.component.html',
  styleUrls: ['./groups-list-container.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GroupsListContainerComponent implements OnInit {
  public constructor(private readonly groupsFacade: GroupsFacade) { }

  public groups$?: Observable<Group[]>;
  public groupsTotalCount$?: Observable<number>;

  public entitiesPerPage: number = 20;
  public offset: number = 0;

  public nameIncludes: string = '';
  public ownOnly: boolean = false;

  public ngOnInit(): void {
    this.updateFilter();
    this.updatePaging();

    this.groups$ = this.groupsFacade.getAllGroupsInPage$();
    this.groupsTotalCount$ = this.groupsFacade.selectGroupsTotalCount$();
  }

  public updateFilter(): void {
    this.groupsFacade.setFilter(this.nameIncludes, this.ownOnly);
  }

  public updatePaging(): void {
    this.groupsFacade.setPaging(this.offset, this.entitiesPerPage);
  }

  public setOffset(offset: number): void {
    this.offset = offset;
    this.updatePaging();
  }

  public setNameIncludes(nameIncludes: string) {
    this.nameIncludes = nameIncludes;
    this.updateFilter();
  }
}

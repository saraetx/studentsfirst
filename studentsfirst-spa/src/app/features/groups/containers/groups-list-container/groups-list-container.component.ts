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

  public offset: number = 0;
  public entitiesPerPage: number = 20;

  public ngOnInit(): void {
    this.updatePaging();

    this.groups$ = this.groupsFacade.getAllGroupsInPage$();
    this.groupsTotalCount$ = this.groupsFacade.selectGroupsTotalCount$();
  }

  public updatePaging(): void {
    this.groupsFacade.setPaging(this.offset, this.entitiesPerPage);
  }

  public setOffset(offset: number): void {
    this.offset = offset;
    this.updatePaging();
  }
}

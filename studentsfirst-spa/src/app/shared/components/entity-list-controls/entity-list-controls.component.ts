import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-entity-list-controls',
  templateUrl: './entity-list-controls.component.html',
  styleUrls: ['./entity-list-controls.component.scss']
})
export class EntityListControlsComponent {
  public constructor() { }

  @Input()
  public totalEntities?: number;
  @Input()
  public entitiesPerPage?: number;
  @Input()
  public currentOffset?: number;

  @Output()
  public offsetChange = new EventEmitter<number>();
  @Output()
  public searchQueryChange = new EventEmitter<string>();

  public queryForm = new FormGroup({
    searchTerm: new FormControl(''),
    currentPage: new FormControl(1)
  });

  public get maxPages(): number {
    return Math.ceil((this.totalEntities ?? 0) / (this.entitiesPerPage ?? 1));
  }
}

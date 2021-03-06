import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-entity-list-controls',
  templateUrl: './entity-list-controls.component.html',
  styleUrls: ['./entity-list-controls.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class EntityListControlsComponent implements OnInit {
  @Input()
  public totalEntities?: number;
  @Input()
  public entitiesPerPage?: number;
  @Input()
  public set offset(value: number) {
    this.patchCurrentPage(this.calculateOffset(value));
  }

  @Input()
  public set searchTerm(value: string | undefined) {
    this.patchSearchTerm(value);
  }

  @Output()
  public offsetChange = new EventEmitter<number>();
  @Output()
  public searchTermChange = new EventEmitter<string>();

  public queryForm = new FormGroup({
    searchTerm: new FormControl(''),
    currentPage: new FormControl(1)
  });

  public ngOnInit(): void {
    this.registerListeners();
  }

  public get maxPages(): number {
    return Math.ceil((this.totalEntities ?? 0) / (this.entitiesPerPage ?? 1));
  }

  protected registerListeners(): void {
    const searchTermControl = this.queryForm.get('searchTerm') as FormControl;
    const currentPageControl = this.queryForm.get('currentPage') as FormControl;

    searchTermControl.valueChanges
      .subscribe((searchTerm: string) => this.searchTermChange.emit(searchTerm))

    currentPageControl.valueChanges.pipe(
      map((currentPage: number) => this.calculateOffset(currentPage))
    ).subscribe((offset: number) => this.offsetChange.emit(offset));
  }

  protected patchSearchTerm(searchTerm: string | undefined) {
    const searchTermControl = this.queryForm.get('searchTerm') as FormControl;
    searchTermControl.setValue(searchTerm);
  }

  protected patchCurrentPage(currentPage: number) {
    const currentPageControl = this.queryForm.get('currentPage') as FormControl;
    currentPageControl.setValue(currentPage);
  }

  protected calculateOffset(page: number): number {
    return (page - 1) * (this.entitiesPerPage ?? 0);
  }
}

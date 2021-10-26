import { ChangeDetectionStrategy, Component, forwardRef, Input, OnChanges, SimpleChanges, TemplateRef, ViewChild, ViewContainerRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => PaginationComponent), multi: true }
  ]
})
export class PaginationComponent implements OnChanges, ControlValueAccessor {
  @Input()
  public max?: number;

  @ViewChild('pageLinksInsertionAnchor', { read: ViewContainerRef, static: true })
  public pageLinksInsertionAnchor!: ViewContainerRef;

  @ViewChild('pageLinkTemplate', { static: true })
  public pageLinkTemplate!: TemplateRef<{ $implicit: number }>;

  public pageNumbers?: number[];
  public selectedPageNumber?: number;

  private onChange?: (pageNumber: Number) => void;
  private onTouched?: () => void;

  public ngOnChanges(changes: SimpleChanges) {
    if (changes.max !== undefined) {
      this.pageNumbers = new Array(this.max).fill(null).map((_, index) => index + 1);

      if (this.selectedPageNumber === undefined || !this.pageNumbers.includes(this.selectedPageNumber)) {
        this.selectedPageNumber = 1;
      }
    }
  }

  public registerOnChange(fn: (pageNumber: Number) => void): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  public writeValue(pageNumber: number): void {
    this.selectedPageNumber = pageNumber;
  }

  public propagateChange(pageNumber: number): void {
    if (this.onChange !== undefined) { this.onChange(pageNumber); }
    if (this.onTouched !== undefined) { this.onTouched(); }

    this.writeValue(pageNumber);
  }
}

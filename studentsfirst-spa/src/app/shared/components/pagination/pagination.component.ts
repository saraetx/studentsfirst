import { AfterContentInit, ChangeDetectionStrategy, Component, forwardRef, Input, TemplateRef, ViewChild, ViewContainerRef } from '@angular/core';
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
export class PaginationComponent implements AfterContentInit, ControlValueAccessor {
  @Input()
  public max?: number;

  @ViewChild('pageLinksInsertionAnchor', { read: ViewContainerRef, static: true })
  public pageLinksInsertionAnchor!: ViewContainerRef;

  @ViewChild('pageLinkTemplate', { static: true })
  public pageLinkTemplate!: TemplateRef<{ $implicit: number }>;

  public selectedPageNumber?: number;

  private onChange?: (pageNumber: Number) => void;
  private onTouched?: () => void;

  public ngAfterContentInit(): void {
    for (let i = 0; i < (this.max ?? 0); i++) {
      const pageNumber = i + 1;

      this.pageLinksInsertionAnchor.createEmbeddedView(this.pageLinkTemplate, {
        $implicit: pageNumber
      });
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

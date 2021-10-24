import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EntityListControlsComponent } from './entity-list-controls.component';

describe('EntityListControlsComponent', () => {
  let component: EntityListControlsComponent;
  let fixture: ComponentFixture<EntityListControlsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EntityListControlsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityListControlsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

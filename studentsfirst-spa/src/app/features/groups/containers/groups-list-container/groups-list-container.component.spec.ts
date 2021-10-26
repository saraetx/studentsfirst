import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupsListContainerComponent } from './groups-list-container.component';

describe('GroupsListContainerComponent', () => {
  let component: GroupsListContainerComponent;
  let fixture: ComponentFixture<GroupsListContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GroupsListContainerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupsListContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

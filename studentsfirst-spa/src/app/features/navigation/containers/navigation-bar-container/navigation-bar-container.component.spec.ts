import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavigationBarContainerComponent } from './navigation-bar-container.component';

describe('NavigationBarContainerComponent', () => {
  let component: NavigationBarContainerComponent;
  let fixture: ComponentFixture<NavigationBarContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NavigationBarContainerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NavigationBarContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

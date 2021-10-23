import { TestBed } from '@angular/core/testing';
import { Observable, of } from 'rxjs';

import { UserRole } from '../auth.constants';
import { UserRoleService } from '../services/user-role.service';
import { TeacherOnlyGuard } from './teacher-only.guard';

describe('TeacherOnlyGuard', () => {
  let guard: TeacherOnlyGuard;
  let roleService: UserRoleService;
  let authorizers: (() => Observable<boolean>)[];

  class MockUserRoleService implements UserRoleService {
    public getUserRole$(): Observable<UserRole | undefined> { return of(undefined); }
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        TeacherOnlyGuard,
        { provide: UserRoleService, useClass: MockUserRoleService }
      ]
    });

    guard = TestBed.inject(TeacherOnlyGuard);
    roleService = TestBed.inject(UserRoleService);

    authorizers = [
      guard.canActivate.bind(guard),
      guard.canActivateChild.bind(guard),
      guard.canLoad.bind(guard)
    ];
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it('should allow activation when user role is teacher', () => {
    spyOn(roleService, 'getUserRole$').and.returnValue(of('teacher'));

    for (const authorizer of authorizers) {
      authorizer().subscribe(result => expect(result).toBeTrue());
    }
  });

  it('should not allow activation when user role is not teacher', () => {
    spyOn(roleService, 'getUserRole$').and.returnValue(of('student'));
    
    for (const authorizer of authorizers) {
      authorizer().subscribe(result => expect(result).toBeFalse());
    }
  });
});

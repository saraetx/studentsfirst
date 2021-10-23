import { TestBed } from '@angular/core/testing';
import { Observable, of } from 'rxjs';
import { UserRole } from '../auth.constants';
import { UserRoleService } from '../services/user-role.service';

import { StudentOnlyGuard } from './student-only.guard';

describe('StudentOnlyGuard', () => {
  let guard: StudentOnlyGuard;
  let roleService: UserRoleService;
  let authorizers: (() => Observable<boolean>)[];

  class MockUserRoleService implements UserRoleService {
    public getUserRole$(): Observable<UserRole | undefined> { return of(undefined); }
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        StudentOnlyGuard,
        { provide: UserRoleService, useClass: MockUserRoleService }
      ]
    });

    guard = TestBed.inject(StudentOnlyGuard);
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

  it('should allow activation when user role is student', () => {
    spyOn(roleService, 'getUserRole$').and.returnValue(of('student'));

    for (const authorizer of authorizers) {
      authorizer().subscribe(result => expect(result).toBeTrue());
    }
  });

  it('should not allow activation when user role is not student', () => {
    spyOn(roleService, 'getUserRole$').and.returnValue(of('teacher'));
    
    for (const authorizer of authorizers) {
      authorizer().subscribe(result => expect(result).toBeFalse());
    }
  });
});

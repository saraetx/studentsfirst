import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, CanLoad } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AuthModule } from '../auth.module';
import { UserRoleService } from '../services/user-role.service';

@Injectable({
  providedIn: AuthModule
})
export class StudentOnlyGuard implements CanActivate, CanActivateChild, CanLoad {
  public constructor(private userRoleService: UserRoleService) {}

  public canActivate(): Observable<boolean> {
    return this.authorize();
  }

  public canActivateChild(): Observable<boolean> {
    return this.authorize();
  }

  public canLoad(): Observable<boolean> {
    return this.authorize();
  }

  protected authorize(): Observable<boolean> {
    return this.userRoleService.getUserRole$().pipe(
      map(userRole => userRole === 'student')
    );
  }
}

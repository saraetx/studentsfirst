import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

import { UserRole } from '../auth.constants';
import { AuthModule } from '../auth.module';

@Injectable({
  providedIn: AuthModule
})
export class UserRoleService {
  public constructor() { }

  public getUserRole$(): Observable<UserRole | undefined> {
    // @TODO
    // Remove hardcode and fetch actual role.
    // Note: inspecting API token is forbidden.

    return of('teacher');
  }
}

import { ActivatedRouteSnapshot, Params, RouterStateSnapshot } from '@angular/router';
import { RouterStateSerializer } from '@ngrx/router-store';
import { RouterStateContent } from './router.state';

export class ApplicationRouteSerializer implements RouterStateSerializer<RouterStateContent> {
  public serialize(routerState: RouterStateSnapshot): RouterStateContent {
    let params: Params = {};
    let stack: ActivatedRouteSnapshot[] = [routerState.root];
    while (stack.length > 0) {
      const route = stack.pop()!;
      params = { ...params, ...route.params };
      stack.push(...route.children);
    }

    return { url: routerState.url, params };
  }
}

import { StoreRouterConfig } from '@ngrx/router-store';
import { ApplicationRouteSerializer } from './serializer';

export const routerConfig: StoreRouterConfig = {
  serializer: ApplicationRouteSerializer
};

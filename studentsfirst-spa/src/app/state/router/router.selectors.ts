import { createSelector } from '@ngrx/store';
import { selectAppFeatureState } from '../app.selectors';

export const selectRouterState = createSelector(selectAppFeatureState, appFeatureState => appFeatureState.router.state);

export const selectRouteUrl = createSelector(selectRouterState, routerState => routerState.url);
export const selectRouteParams = createSelector(selectRouterState, routerState => routerState.params);

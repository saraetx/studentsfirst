import { ActionReducerMap } from '@ngrx/store';
import { AppState } from './app.state';
import { routerReducer } from './router/router.reducer';

export const appReducers: ActionReducerMap<AppState> = {
  router: routerReducer
};

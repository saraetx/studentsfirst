import { Params } from '@angular/router';
import { RouterReducerState } from '@ngrx/router-store';

export interface RouterStateContent {
  url: string;
  params: Params;
}

export type RouterState = RouterReducerState<RouterStateContent>;

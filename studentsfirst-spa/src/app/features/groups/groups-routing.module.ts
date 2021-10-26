import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { GroupsListContainerComponent } from './containers/groups-list-container/groups-list-container.component';

const routes: Routes = [
  { path: '', component: GroupsListContainerComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GroupsRoutingModule { }

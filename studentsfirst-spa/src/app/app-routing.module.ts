import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

const routes: Routes = [
  { path: '', canActivateChild: [MsalGuard], children: [
    { path: 'groups', loadChildren: () => import('./features/groups/groups.module').then(m => m.GroupsModule) }
  ] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

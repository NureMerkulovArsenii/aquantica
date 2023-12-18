import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AuthGuard} from "./@core/guards/auth.guard";

const routes: Routes = [
  {
    path: 'sections', //ToDo: rename to 'sections'
    loadChildren: () => import('./modules/sections/sections.module').then(m => m.SectionsModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'account',//ToDo: rename to 'account
    loadChildren: () => import('./modules/account/account.module').then(m => m.AccountModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}

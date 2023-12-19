import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AuthGuard} from "./@core/guards/auth.guard";

const routes: Routes = [
  {
    path: 'sections',
    loadChildren: () => import('./modules/sections/sections.module').then(m => m.SectionsModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'account',
    loadChildren: () => import('./modules/account/account.module').then(m => m.AccountModule)
  },
  {
    path: 'ruleset',
    loadChildren: () => import('./modules/ruleset/ruleset.module').then(m => m.RulesetModule),
    // canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}

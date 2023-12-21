import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AuthGuard} from "./@core/guards/auth.guard";

const routes: Routes = [
  {
    path: 'sections',
    loadChildren: () => import('./modules/sections/sections.module').then(m => m.SectionsModule),
    canActivate: [AuthGuard]
  },
  // {
  //   path: 'account',
  //   loadChildren: () => import('./modules/account/account.module').then(m => m.AccountModule)
  // },
  {
    path: 'role',
    loadChildren: () => import('./modules/role/role.module').then(m => m.RoleModule),
  },
  {
    path: 'ruleset',
    loadChildren: () => import('./modules/ruleset/ruleset.module').then(m => m.RulesetModule),
    // canActivate: [AuthGuard]
  },
  {
    path: 'user',
    loadChildren: () => import('./modules/user/user.module').then(m => m.UserModule),
    // canActivate: [AuthGuard]
  },
  {
    path: 'jobs',
    loadChildren: () => import('./modules/jobs/jobs.module').then(m => m.JobsModule),
    // canActivate: [AuthGuard]
  },
  {
    path: 'administration',
    loadChildren: () => import('./modules/administration/administration.module').then(m => m.AdministrationModule),
    // canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}

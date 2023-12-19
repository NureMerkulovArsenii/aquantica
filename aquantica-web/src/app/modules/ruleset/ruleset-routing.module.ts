import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {RulesetDetailsComponent} from "./ruleset-details/ruleset-details.component";
import {RulesetListComponent} from "./ruleset-list/ruleset-list.component";

const routes: Routes = [
  {
    path:'',
    component: RulesetListComponent,
  },
  {
    path: ':id',
    component: RulesetDetailsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RulesetRoutingModule { }

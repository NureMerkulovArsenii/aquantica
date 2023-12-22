import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {JobsListComponent} from "./jobs-list/jobs-list.component";

const routes: Routes = [
  {
    path: '**',
    component: JobsListComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class JobsRoutingModule { }

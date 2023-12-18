import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {SectionsListComponent} from "./pages/sections-list/sections-list.component";
import {SectionDetailsComponent} from "./pages/section-details/section-details.component";

const routes: Routes = [
  {
    path: '',
    component: SectionsListComponent
  },
  {
    path:':id',
    component: SectionDetailsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SectionsRoutingModule {
}

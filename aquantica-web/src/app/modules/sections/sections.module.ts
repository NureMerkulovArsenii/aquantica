import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SectionsRoutingModule } from './sections-routing.module';
import { SectionsListComponent } from './pages/sections-list/sections-list.component';


@NgModule({
  declarations: [
    SectionsListComponent
  ],
  imports: [
    CommonModule,
    SectionsRoutingModule
  ]
})
export class SectionsModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdministrationRoutingModule } from './administration-routing.module';
import { DatabaseComponent } from './database/database.component';
import {MatButtonModule} from "@angular/material/button";


@NgModule({
  declarations: [
    DatabaseComponent
  ],
  imports: [
    CommonModule,
    AdministrationRoutingModule,
    MatButtonModule
  ]
})
export class AdministrationModule { }

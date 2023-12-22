import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {SectionsRoutingModule} from './sections-routing.module';
import {SectionsListComponent} from './pages/sections-list/sections-list.component';
import {SectionDetailsComponent} from './pages/section-details/section-details.component';
import {MatTableModule} from "@angular/material/table";
import {MatButtonModule} from "@angular/material/button";
import {MatIconModule} from "@angular/material/icon";
import {MatCardModule} from "@angular/material/card";
import {MatInputModule} from "@angular/material/input";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {_MatSlideToggleRequiredValidatorModule, MatSlideToggleModule} from "@angular/material/slide-toggle";
import {MatListModule} from "@angular/material/list";
import {MatSelectModule} from "@angular/material/select";
import {MatDialogModule} from "@angular/material/dialog";
import {CoreModule} from "../../@core/core.module";
import {TranslateModule} from "@ngx-translate/core";


@NgModule({
  declarations: [
    SectionsListComponent,
    SectionDetailsComponent
  ],
    imports: [
        CommonModule,
        SectionsRoutingModule,
        MatTableModule,
        MatButtonModule,
        MatIconModule,
        MatCardModule,
        MatInputModule,
        FormsModule,
        _MatSlideToggleRequiredValidatorModule,
        MatListModule,
        MatSlideToggleModule,
        MatSelectModule,
        ReactiveFormsModule,
        MatDialogModule,
        CoreModule,
        TranslateModule,
    ],
})
export class SectionsModule {
}

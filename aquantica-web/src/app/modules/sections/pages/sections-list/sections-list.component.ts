import {Component, OnInit} from '@angular/core';
import {SectionService} from "../../../../@core/services/section.service";
import {Section} from "../../../../@core/models/section/section";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {MatDialog} from "@angular/material/dialog";
import {SectionDetailsComponent} from "../section-details/section-details.component";
import {DialogData} from "../../../../@core/models/dialog-data";

@Component({
  selector: 'app-sections-list',
  templateUrl: './sections-list.component.html',
  styleUrls: ['./sections-list.component.scss']
})
export class SectionsListComponent implements OnInit {
  sections: Section[] = [];
  columnsToDisplay = ['name', 'number', 'isEnabled', 'deviceUri', 'actions'];


  constructor(
    private readonly sectionService: SectionService,
    private readonly router: Router,
    private readonly toastr: ToastrService,
    private readonly dialog: MatDialog
  ) {
  }

  async ngOnInit(): Promise<void> {
    await this.refresh();
  }

  async refresh(): Promise<void> {
    try {
      this.sections = await this.sectionService.getSections()
      console.log(this.sections)
    } catch (e) {
      console.error(e)
    }
  }


  async openSection(id: number): Promise<void> {
    const dialogRef = this.dialog.open(SectionDetailsComponent, {
      data: {data: id, isEdit: true} as DialogData<number>,
    });

    dialogRef.afterOpened().subscribe(result => {
      dialogRef.componentRef?.instance.refresh();
    });
  }

  async deleteSection(id: number): Promise<void> {
    try {
      console.log(id)
      this.sectionService.deleteSection(id).subscribe({
        next: (response) => {
          if (response.isSuccess)
            this.refresh();
          else {
            this.toastr.error(response.error)
          }
        },
        error: (error) => {
          this.toastr.error(error.error.error)
        }
      })
      await this.refresh();
    } catch (e) {
      console.error(e)
    }
  }

  async createSection(): Promise<void> {
    try {
      await this.router.navigate(['/sections', 'create'])
    } catch (e) {
      console.error(e)
    }
  }
}

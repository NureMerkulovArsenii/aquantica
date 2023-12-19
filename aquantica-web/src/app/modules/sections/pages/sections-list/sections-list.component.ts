import {Component, OnInit} from '@angular/core';
import {SectionService} from "../../../../@core/services/section.service";
import {Section} from "../../../../@core/models/section/section";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {MatDialog} from "@angular/material/dialog";
import {SectionDetailsComponent} from "../section-details/section-details.component";
import {DialogData} from "../../../../@core/models/dialog-data";
import {DialogService} from "../../../../@core/services/dialog.service";

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
    private readonly dialog: MatDialog,
    private readonly dialogService: DialogService
  ) {
  }

  ngOnInit(): void {
    this.refresh();
  }

  refresh(): void {
    try {
      this.sectionService.getSections().subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.sections = response.data!;
          } else {
            this.toastr.error(response.error)
          }
        },
        error: (error) => {
          this.toastr.error(error.error.error)
        }
      })
    } catch (e) {
      console.error(e)
    }
  }

  async openSection(id: number): Promise<void> {
    const dialogRef = this.dialog.open(SectionDetailsComponent, {
      data: {
        data: id,
        isEdit: true,
        additionalData: this.sections.filter(x => x.id !== id)
      } as DialogData<number, Section[]>,
    });

    dialogRef.afterOpened().subscribe(result => {
      dialogRef.componentRef?.instance.refresh();
    });

    dialogRef.afterClosed().subscribe(x => {
      this.refresh();
    });
  }

  deleteSection(id: number): void {
    this.dialogService.openDialog({
      data: {
        title: "Delete section",
        message: "Are you sure you want to delete this section?",
        okButtonText: "Delete",
        cancelButtonText: "Cancel"
      },
      onClose: (result) => {
        if (result) {
          this.applyDelete(id);
        }
      }
    });
  }

  applyDelete(id: number): void {
    try {
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
    } catch (e) {
      console.error(e)
    }
  }


  createSection(): void {
    const dialogRef = this.dialog.open(SectionDetailsComponent, {
      data: {isEdit: false, additionalData: this.sections} as DialogData<number, Section[]>,
    });

    dialogRef.afterOpened().subscribe(result => {
      dialogRef.componentRef?.instance.refresh();
    });

    dialogRef.afterClosed().subscribe(x => {
      console.log('The dialog was closed');
      this.refresh();
    });
  }
}

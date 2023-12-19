import {Component, Inject, OnInit} from '@angular/core';
import {SectionService} from "../../../../@core/services/section.service";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {SectionDetails} from "../../../../@core/models/section/section-details";
import {RulesetService} from "../../../../@core/services/ruleset.service";
import {Ruleset} from "../../../../@core/models/ruleset/ruleset";
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material/dialog";
import {RulesetDetailsComponent} from "../../../ruleset/ruleset-details/ruleset-details.component";
import {DialogData} from "../../../../@core/models/dialog-data";
import {Location} from "../../../../@core/models/location/location";
import {SectionType} from "../../../../@core/models/section/section-type";
import {DialogService} from "../../../../@core/services/dialog.service";
import {DialogModel} from "../../../../@core/models/dialog-model";
import {Section} from "../../../../@core/models/section/section";

@Component({
  selector: 'app-section-details',
  templateUrl: './section-details.component.html',
  styleUrls: ['./section-details.component.scss']
})
export class SectionDetailsComponent implements OnInit {
  protected section: SectionDetails = {} as SectionDetails;
  protected ruleSets: Ruleset[] = [];
  protected location: Location = {} as Location;
  protected sectionTypes: SectionType[] = [];
  private isLoading: boolean = false;

  constructor(
    public dialogRef: MatDialogRef<SectionDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData<number, Section[]>,
    private readonly sectionService: SectionService,
    private readonly rulesetService: RulesetService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly toastr: ToastrService,
    private readonly dialog: MatDialog,
    private readonly dialogService: DialogService
  ) {
  }


  ngOnInit(): void {

  }

  public refresh(): void {
    this.getRuleSets();
    this.getSectionTypes();
    console.log(this.data.additionalData);

    setTimeout(() => {
      this.isLoading = true;
    }, 40);

    if (this.data.data != null) {
      this.sectionService.getSection(this.data.data).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.section = response.data!;
          } else {
            this.toastr.error(response.error)
          }
        },
        error: (error) => {
          this.toastr.error(error.error.error)
        }
      });

    }

  }

  getRuleSets(): Ruleset[] {
    this.rulesetService.getAll().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.ruleSets = response.data!;
        } else {
          this.toastr.error(response.error)
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error)
      }
    });

    return this.ruleSets;
  }

  getSectionTypes(): void {
    this.sectionService.getSectionTypes().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.sectionTypes = response.data!;
        } else {
          this.toastr.error(response.error)
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error)
      }
    });
  }

  async editRuleSet(): Promise<void> {
    const dialogRef = this.dialog.open(RulesetDetailsComponent, {
      data: {data: this.section.sectionRulesetId, isEdit: true} as DialogData<number, null>,
    });

    dialogRef.afterOpened().subscribe(result => {
      dialogRef.componentRef?.instance.refresh();
    });
  }

  delete(): void {
    this.dialogService.openDialog({
      data: {
        title: 'Delete section',
        message: 'Are you sure you want to delete this section?',
        cancelButtonText: 'Cancel',
        okButtonText: 'Delete',
      },
      onClose: (result) => {
        if (result) {
          this.applyDelete();
        }
      }

    } as DialogModel)
  }

  private applyDelete(): void {
    this.sectionService.deleteSection(this.section.id).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastr.success("Operation successful")
          this.dialogRef.close('success');
        } else {
          this.toastr.error(response.error)
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error)
      }
    });
  }

  applyChanges(): void {
    if (this.data.isEdit) {
      this.applyEdit();
    } else {
      this.applyCreate();
    }

  }

  applyEdit(): void {
    console.log(this.data.data)
    console.log(this.section)
    this.sectionService.updateSection(this.section).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastr.success("Operation successful")
          this.dialogRef.close('success');
        } else {
          this.toastr.error(response.error)
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error)
      }
    });
  }

  applyCreate() {
    this.section.location = this.location;
    this.sectionService.createSection(this.section).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastr.success("Operation successful")
          this.dialogRef.close();
        } else {
          this.toastr.error(response.error)
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error)
      }
    });
  }

}

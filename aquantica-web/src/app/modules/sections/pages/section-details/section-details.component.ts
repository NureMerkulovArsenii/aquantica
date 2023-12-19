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

@Component({
  selector: 'app-section-details',
  templateUrl: './section-details.component.html',
  styleUrls: ['./section-details.component.scss']
})
export class SectionDetailsComponent implements OnInit {
  protected section: SectionDetails = {} as SectionDetails;
  protected ruleSets: Ruleset[] = [];

  constructor(
    public dialogRef: MatDialogRef<SectionDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData<number>,
    private readonly sectionService: SectionService,
    private readonly rulesetService: RulesetService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly toastr: ToastrService,
    private readonly dialog: MatDialog
  ) {
  }

  ngOnInit(): void {
  }

  public refresh(): void {
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

    this.getRuleSets();
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

  async editRuleSet(): Promise<void> {
    const dialogRef = this.dialog.open(RulesetDetailsComponent, {
      data: {data: this.section.sectionRulesetId, isEdit: true} as DialogData<number>,
    });

    dialogRef.afterOpened().subscribe(result => {
      dialogRef.componentRef?.instance.refresh();
    });
  }

  applyChanges(): void {
    console.log(this.section)
    this.sectionService.updateSection(this.section).subscribe({
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

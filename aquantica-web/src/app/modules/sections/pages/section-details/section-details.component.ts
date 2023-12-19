import {Component, Inject, OnInit} from '@angular/core';
import {SectionService} from "../../../../@core/services/section.service";
import {ActivatedRoute, Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {SectionDetails} from "../../../../@core/models/section/section-details";
import {RulesetService} from "../../../../@core/services/ruleset.service";
import {Ruleset} from "../../../../@core/models/ruleset/ruleset";
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material/dialog";
import {RulesetDetailsComponent} from "../../../ruleset/ruleset-details/ruleset-details.component";

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
    @Inject(MAT_DIALOG_DATA) public data: number,
    private readonly sectionService: SectionService,
    private readonly rulesetService: RulesetService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly toastr: ToastrService,
    private readonly dialog: MatDialog
  ) {
  }

  ngOnInit(): void {
    // const id = this.route.snapshot.paramMap.get('id');
    // this.sectionService.getSection(+id!).subscribe({
    //   next: (response) => {
    //     if (response.isSuccess) {
    //       this.section = response.data!;
    //     } else {
    //       this.toastr.error(response.error)
    //     }
    //   },
    //   error: (error) => {
    //     this.toastr.error(error.error.error)
    //   }
    // });
    //
    // this.getRuleSets();
  }

  public refresh(): void {
    //const id = this.route.snapshot.paramMap.get('id');
    this.sectionService.getSection(this.data).subscribe({
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
      data: this.section.sectionRulesetId,
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

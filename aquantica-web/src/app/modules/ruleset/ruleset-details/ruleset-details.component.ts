import {Component, Inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {RulesetService} from "../../../@core/services/ruleset.service";
import {Ruleset} from "../../../@core/models/ruleset/ruleset";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {DialogData} from "../../../@core/models/dialog-data";

@Component({
  selector: 'app-ruleset-details',
  templateUrl: './ruleset-details.component.html',
  styleUrls: ['./ruleset-details.component.scss']
})
export class RulesetDetailsComponent {

  ruleset: Ruleset = {} as Ruleset;

  constructor(
    private readonly toastr: ToastrService,
    private readonly rulesetService: RulesetService,
    public dialogRef: MatDialogRef<RulesetDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData<number, null>,
  ) {
  }

  public refresh(): void {
    try {
      if (this.data.data != null) {
        this.rulesetService.get(this.data.data).subscribe({
          next: (response) => {
            if (response.isSuccess) {
              this.ruleset = response.data!;
            } else {
              this.toastr.error(response.error)
            }
          },
          error: (error) => {
            this.toastr.error(error.error.error)
          }
        });
      }
    } catch (e) {
      console.error(e)
    }
  }

  applyChanges(): void {
    if (this.data.isEdit) {
      this.applyEdit();
    } else {
      this.applyCreate();
    }
  }

  applyEdit(): void {
    this.rulesetService.update(this.ruleset).subscribe({
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

  applyCreate() {
    this.rulesetService.create(this.ruleset).subscribe({
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

  onNoClick(): void {
    this.dialogRef.close();
  }

}

import {Component, Inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {RulesetService} from "../../../@core/services/ruleset.service";
import {Ruleset} from "../../../@core/models/ruleset/ruleset";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-ruleset-details',
  templateUrl: './ruleset-details.component.html',
  styleUrls: ['./ruleset-details.component.scss']
})
export class RulesetDetailsComponent implements OnInit {

  ruleset: Ruleset | null = null;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly toastr: ToastrService,
    private readonly rulesetService: RulesetService,
    private readonly router: Router,
    public dialogRef: MatDialogRef<RulesetDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: number,
  ) {
  }

  ngOnInit(): void {
    //this.refresh();
  }

  public refresh(): void {
    try {
      //const id = this.route.snapshot.paramMap.get('id');
      this.rulesetService.get(this.data).subscribe({
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
    } catch (e) {
      console.error(e)
    }
  }


  applyChanges(): void {
    try {
      this.rulesetService.update(this.ruleset!).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.dialogRef.close();
            this.toastr.success("Changes applied successfully")
          } else {
            this.toastr.error(response.error)
          }
        },
        error: (error) => {
          this.toastr.error(error.error.error)
        }
      });
    } catch (e) {
      console.error(e)
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}

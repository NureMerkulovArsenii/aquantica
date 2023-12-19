import {Component, OnInit} from '@angular/core';
import {ToastrService} from "ngx-toastr";
import {RulesetService} from "../../../@core/services/ruleset.service";
import {Ruleset} from "../../../@core/models/ruleset/ruleset";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {RulesetDetailsComponent} from "../ruleset-details/ruleset-details.component";
import {DialogData} from "../../../@core/models/dialog-data";
import {DialogService} from "../../../@core/services/dialog.service";

@Component({
  selector: 'app-ruleset-list',
  templateUrl: './ruleset-list.component.html',
  styleUrls: ['./ruleset-list.component.scss']
})
export class RulesetListComponent implements OnInit {
  columnsToDisplay = ['name',
    'description',
    'isEnabled',
    'optimalSoilHumidity',
    'humidityGrowthPerRainMm',
    'temperatureThreshold',
    'actions'];

  protected ruleSetList!: Ruleset[];

  constructor(
    private readonly toastr: ToastrService,
    private readonly router: Router,
    private readonly rulesetService: RulesetService,
    private readonly dialog: MatDialog,
    private readonly dialogService: DialogService
  ) {
  }

  ngOnInit(): void {
    this.refresh();
  }

  refresh(): void {
    this.rulesetService.getAll().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.ruleSetList = response.data!;
        } else {
          this.toastr.error(response.error)
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error)
      }
    });
  }

  async openRuleset(id: number): Promise<void> {
    const dialogRef = this.dialog.open(RulesetDetailsComponent, {
      data: {data: id, isEdit: true} as DialogData<number, null>,
    });

    dialogRef.afterOpened().subscribe(result => {
      dialogRef.componentRef?.instance.refresh();
    });
  }

  deleteRuleSet(id: number): void {
    this.dialogService.openDialog({
      data: {
        title: "Delete ruleset",
        message: "Are you sure you want to delete this ruleset?",
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
      this.rulesetService.delete(id).subscribe({
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

  createNewRuleset(): void {
    const dialogRef = this.dialog.open(RulesetDetailsComponent, {
      data: {data: null, isEdit: false} as DialogData<number, null>,
      minWidth: '70vw'
    });

    // dialogRef.afterOpened().subscribe(result => {
    //   dialogRef.componentRef?.instance.refresh();
    // });

    dialogRef.afterClosed().subscribe(x => {
      this.refresh();
    });
  }

}

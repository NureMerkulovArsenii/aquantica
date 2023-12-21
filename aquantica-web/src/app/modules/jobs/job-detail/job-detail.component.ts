import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {DialogData} from "../../../@core/models/dialog-data";

@Component({
  selector: 'app-job-detail',
  templateUrl: './job-detail.component.html',
  styleUrls: ['./job-detail.component.scss']
})
export class JobDetailComponent implements OnInit{

  constructor(
    public dialogRef: MatDialogRef<JobDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData<number, null>,
  ) {
  }

  ngOnInit(): void {
    this.refresh();
  }

  refresh(): void {

  }


}

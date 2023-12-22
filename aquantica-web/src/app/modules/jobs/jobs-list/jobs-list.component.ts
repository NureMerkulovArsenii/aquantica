import {Component, OnInit} from '@angular/core';
import {ToastrService} from "ngx-toastr";
import {MatDialog} from "@angular/material/dialog";
import {DialogService} from "../../../@core/services/dialog.service";
import {JobsService} from "../../../@core/services/jobs.service";
import {Job} from "../../../@core/models/job-control/job";
import {DialogData} from "../../../@core/models/dialog-data";
import {JobRepetitionType} from "../../../@core/enums/job-repetition-type";
import {JobMethod} from "../../../@core/enums/job-method";
import {JobDetailComponent} from "../job-detail/job-detail.component";

@Component({
  selector: 'app-jobs-list',
  templateUrl: './jobs-list.component.html',
  styleUrls: ['./jobs-list.component.scss']
})
export class JobsListComponent implements OnInit {

  columnsToDisplay = [
    'name',
    'isEnabled',
    'jobRepetitionType',
    'jobRepetitionValue',
    'jobMethod',
    'actions'
  ];
  protected jobs: Job[] = [];

  constructor(
    private readonly dialog: MatDialog,
    private readonly toastr: ToastrService,
    private readonly dialogService: DialogService,
    private readonly jobService: JobsService,
  ) {
  }

  ngOnInit() {
    this.refresh();
  }

  refresh() {
    this.jobService.getAllJobs().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.jobs = response.data!;
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error);
      }
    });
  }

  openJob(id: number): void {
    const dialogRef = this.dialog.open(JobDetailComponent, {
      data: {data: id, isEdit: true} as DialogData<number, null>,
    });

    dialogRef.afterClosed().subscribe(result => {
      this.refresh();
    });
  }


  createJob(): void {
    const dialogRef = this.dialog.open(JobDetailComponent, {
      data: {data: null, isEdit: false} as DialogData<number, null>,
    });

    dialogRef.afterClosed().subscribe(result => {
      this.refresh();
    });
  }

  deleteJob(id: number): void {
    this.dialogService.openDialog({
      data: {
        title: 'Delete job',
        message: 'Are you sure you want to delete this job?',
        okButtonText: 'Delete',
        cancelButtonText: 'Cancel',
      },
      onClose: (result) => {
        if (result) {
          this.applyDeleteJob(id);
        }
      }
    });
  }

  applyDeleteJob(id: number): void {
    this.jobService.deleteJob(id).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastr.success('Operation successful');
          this.refresh();
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error);
      }
    });
  }

  startAllJobs(): void {
    this.jobService.startAllJobs().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastr.success('Operation successful');
          this.refresh();
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error);
      }
    });
  }

  stopAllJobs(): void {
    this.jobService.stopAllJobs().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastr.success('Operation successful');
          this.refresh();
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error);
      }
    });
  }

  toggleJob(job: Job): void {
    console.log(job)
    if (job.isEnabled) {
      this.stopJob(job.id);
    } else {
      this.startJob(job.id);
    }
  }

  startJob(id: number): void {
    this.jobService.startJob(id).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastr.success('Operation successful');
          this.refresh();
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error);
      }
    });
  }

  stopJob(id: number): void {
    this.jobService.stopJob(id).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastr.success('Operation successful');
          this.refresh();
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error);
      }
    });
  }

  fireJobAsMethod(id: number): void {
    this.jobService.fireJobAsMethod(id).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastr.success('Operation successful');
          this.refresh();
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error);
      }
    });
  }

  getRepetitionType(type: JobRepetitionType): string {
    switch (type) {
      case JobRepetitionType.days:
        return 'Day';
      case JobRepetitionType.hours:
        return 'Hour';
      case JobRepetitionType.minutes:
        return 'Minute';
      case JobRepetitionType.seconds:
        return 'Second';
      case JobRepetitionType.weeks:
        return 'Weekl';
      case JobRepetitionType.months:
        return 'Month';
      default:
        return 'Unknown';
    }
  }

  getJobMethod(method: JobMethod): string {
    switch (method) {
      case JobMethod.collectSensorData:
        return 'Collect sensor data';
      case JobMethod.getWeatherForecast:
        return 'Get weather forecast';
      case JobMethod.startIrrigation:
        return 'Start irrigation';
      case JobMethod.stopIrrigation:
        return 'Stop irrigation';
      default:
        return 'Unknown';
    }
  }

}

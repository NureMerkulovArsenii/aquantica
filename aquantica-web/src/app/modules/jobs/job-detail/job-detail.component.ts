import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {DialogData} from "../../../@core/models/dialog-data";
import {ToastrService} from "ngx-toastr";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {JobsService} from "../../../@core/services/jobs.service";
import {JobDetailed} from "../../../@core/models/job-control/job-detailed";
import {JobRepetitionType} from "../../../@core/enums/job-repetition-type";
import {JobMethod} from "../../../@core/enums/job-method";

@Component({
  selector: 'app-job-detail',
  templateUrl: './job-detail.component.html',
  styleUrls: ['./job-detail.component.scss']
})
export class JobDetailComponent implements OnInit {
  protected jobForm!: FormGroup;
  protected job!: JobDetailed;
  protected jobRepetitionTypes: number[] = [];
  protected jobMethods: number[] = [];

  constructor(
    public dialogRef: MatDialogRef<JobDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData<number, null>,
    private readonly toastr: ToastrService,
    private readonly formBuilder: FormBuilder,
    private readonly jobService: JobsService,
  ) {
  }

  ngOnInit(): void {
    this.initForm();
    this.refresh();
  }

  initForm(): void {
    this.jobForm = this.formBuilder.group({
      id: [''],
      name: ['', Validators.required],
      isEnabled: [false],
      jobRepetitionType: ['', Validators.required],
      jobRepetitionValue: ['', Validators.required],
      jobMethod: ['', Validators.required],
    });
  }

  refresh(): void {
    if (!this.data.isEdit) {
      return;
    }
    this.jobService.getJob(this.data.data!).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.job = response.data!;
          this.jobForm.patchValue({
            id: response.data!.id,
            name: response.data!.name,
            isEnabled: response.data!.isEnabled,
            jobRepetitionType: response.data!.jobRepetitionType,
            jobRepetitionValue: response.data!.jobRepetitionValue,
            jobMethod: response.data!.jobMethod,
          });
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error);
      }
    });

    this.jobRepetitionTypes = Object.keys(JobRepetitionType)
      .filter(f => !isNaN(Number(f)))
      .map(m => Number(m));

    this.jobMethods = Object.keys(JobMethod)
      .filter(f => !isNaN(Number(f)))
      .map(m => Number(m));
  }

  applyChanges(): void {
    if (this.data.isEdit) {
      this.jobService.updateJob(this.jobForm.value).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.toastr.success('Operation successful');
            this.dialogRef.close();
          }
        },
        error: (error) => {
          this.toastr.error(error.error.error);
        }
      });
    } else {
      this.jobService.createJob(this.jobForm.value).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.toastr.success('Operation successful');
            this.dialogRef.close();
          }
        },
        error: (error) => {
          this.toastr.error(error.error.error);
        }
      });
    }

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

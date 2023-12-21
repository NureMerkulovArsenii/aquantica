import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {DialogData} from "../../../@core/models/dialog-data";
import {RulesetDetailsComponent} from "../../ruleset/ruleset-details/ruleset-details.component";
import {ToastrService} from "ngx-toastr";
import {AccountService} from "../../../@core/services/account.service";
import {RoleService} from "../../../@core/services/role.service";
import {Role} from "../../../@core/models/role/role";
import {UserUpdate} from "../../../@core/models/user/user-update";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-detailed',
  templateUrl: './user-detailed.component.html',
  styleUrls: ['./user-detailed.component.scss']
})
export class UserDetailedComponent implements OnInit {
  userForm!: FormGroup;
  roles: Role[] = [];

  constructor(
    public dialogRef: MatDialogRef<UserDetailedComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData<number, null>,
    private readonly toastr: ToastrService,
    private readonly accountService: AccountService,
    private readonly roleService: RoleService,
    private readonly formBuilder: FormBuilder,
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.refresh();
  }

  initForm(): void {
    this.userForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      password: ['', Validators.required],
      phoneNumber: ['', Validators.pattern('[0-9]*')],
      userRole: [''], // Changed from [null] to ['']
      isEnabled: [false],
      isBlocked: [false],
    });
  }

  refresh(): void {
    this.getRoles();

    if (this.data.data != null) {
      this.accountService.getUser(this.data.data).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.userForm.patchValue({
              email: response.data!.email,
              firstName: response.data!.firstName,
              lastName: response.data!.lastName,
              phoneNumber: response.data!.phoneNumber,
              userRole: response.data!.role?.id,
              isEnabled: response.data!.isEnabled,
              isBlocked: response.data!.isBlocked,
            });           
                       
          } else {
            this.toastr.error(response.error);
          }
        },
        error: (error) => {
          this.toastr.error(error.error.error);
          return;
        },
      });
    }
  }

  getRoles(): void {
    this.roleService.getRoles().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.roles = response.data!;
        } else {
          this.toastr.error(response.error);
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error);
      },
    });
  }

  applyChanges(): void {
    console.log(this.userForm.value);
    if (this.data.isEdit) {
      this.applyEdit();
    } else {
      this.applyCreate();
    }
  }

  applyEdit(): void {
    console.log(this.userForm.value);
    this.accountService.updateUser(this.userForm.value).subscribe({
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

  applyCreate(): void {
    this.accountService.register(this.userForm.value).subscribe({
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

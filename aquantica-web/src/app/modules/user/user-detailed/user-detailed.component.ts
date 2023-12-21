import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {DialogData} from "../../../@core/models/dialog-data";
import {RulesetDetailsComponent} from "../../ruleset/ruleset-details/ruleset-details.component";
import {ToastrService} from "ngx-toastr";
import {AccountService} from "../../../@core/services/account.service";
import {RoleService} from "../../../@core/services/role.service";
import {Role} from "../../../@core/models/role/role";
import {UserUpdate} from "../../../@core/models/user/user-update";

@Component({
  selector: 'app-user-detailed',
  templateUrl: './user-detailed.component.html',
  styleUrls: ['./user-detailed.component.scss']
})
export class UserDetailedComponent implements OnInit {
  protected user: UserUpdate = {} as UserUpdate;
  //protected user: any = {};
  protected roles: Role[] = [];


  constructor(
    public dialogRef: MatDialogRef<RulesetDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData<number, null>,
    private readonly toastr: ToastrService,
    private readonly accountService: AccountService,
    private readonly roleService: RoleService,
  ) {
  }

  ngOnInit(): void {
    this.refresh();
  }

  refresh(): void {
    this.getRoles();

    if (this.data.data != null) {
      this.accountService.getUser(this.data.data).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.user = response.data!;
          } else {
            this.toastr.error(response.error)
          }
        },
        error: (error) => {
          this.toastr.error(error.error.error)
          return;
        }
      });

      // user = {
      //   id: this.userResponse.id,
      //   email: this.userResponse.email,
      //   firstName: this.userResponse.firstName,
      //   lastName: this.userResponse.lastName,
      //   isEnabled: this.userResponse.isEnabled,
      //   isBlocked: this.userResponse.isBlocked,
      //   roles: this.userResponse.role?.id,
      // }

    }
  }

  getRoles(): void {
    this.roleService.getRoles().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.roles = response.data!;
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
    console.log(this.user)
    this.accountService.updateUser(this.user).subscribe({
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
    this.accountService.register(this.user).subscribe({
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

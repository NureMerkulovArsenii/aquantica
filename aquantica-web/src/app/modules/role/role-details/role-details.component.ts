import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {DialogData} from "../../../@core/models/dialog-data";
import {ToastrService} from "ngx-toastr";
import {RoleService} from "../../../@core/services/role.service";
import {RoleDetailed} from "../../../@core/models/role/role-detailed";

@Component({
  selector: 'app-role-details',
  templateUrl: './role-details.component.html',
  styleUrls: ['./role-details.component.scss']
})
export class RoleDetailsComponent {

  role: RoleDetailed = {} as RoleDetailed;

  constructor(
    public dialogRef: MatDialogRef<RoleDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData<number, null>,
    private readonly roleService: RoleService,
    private readonly toastr: ToastrService,
  ) {
  }


  public refresh(): void {
    try {
      if (this.data.data != null) {
        this.roleService.getRole(this.data.data).subscribe({
          next: (response) => {
            if (response.isSuccess) {
              this.role = response.data!;
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
    this.roleService.updateRole(this.role).subscribe({
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
    this.roleService.createRole(this.role).subscribe({
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

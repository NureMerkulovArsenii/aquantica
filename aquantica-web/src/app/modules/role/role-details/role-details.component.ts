import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {DialogData} from "../../../@core/models/dialog-data";
import {ToastrService} from "ngx-toastr";
import {RoleService} from "../../../@core/services/role.service";
import {RoleDetailed} from "../../../@core/models/role/role-detailed";
import {AccessActionService} from "../../../@core/services/access-action.service";
import {AccessAction} from "../../../@core/models/access-action/access-action";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-role-details',
  templateUrl: './role-details.component.html',
  styleUrls: ['./role-details.component.scss']
})
export class RoleDetailsComponent implements OnInit {
  role: RoleDetailed = {} as RoleDetailed;
  protected accessActions!: AccessAction[] | undefined;
  protected roleForm!: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<RoleDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData<number, null>,
    private readonly roleService: RoleService,
    private readonly accessActionService: AccessActionService,
    private readonly toastr: ToastrService,
    private readonly formBuilder: FormBuilder,
  ) {
  }

  ngOnInit(): void {
    this.initForm();
    this.refresh();
  }

  initForm(): void {
    this.roleForm = this.formBuilder.group({
      id: [''],
      name: ['', Validators.required],
      description: [''],
      isEnabled: [false],
      isBlocked: [false],
      isDefault: [false],
      accessActionsIds: [''],
    });
  }

  public refresh(): void {
    this.loadAccessActions();
    console.log(this.accessActions)
    try {
      this.roleService.getRole(this.data.data!).subscribe({
          next: (response) => {
            if (response.isSuccess) {
              this.roleForm.patchValue({
                id: response.data!.id,
                name: response.data!.name,
                description: response.data!.description,
                isEnabled: response.data!.isEnabled,
                isBlocked: response.data!.isBlocked,
                isDefault: response.data!.isDefault,
                accessActionsIds: response.data!.accessActionsIds,
              });
            }
          },
          error:
            (error) => {
              this.toastr.error(error.error.error)
            }
        }
      );
    } catch (e) {
      console.error(e)
    }
  }

  loadAccessActions(): void {
    this.accessActionService.getAccessActions().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          console.log(response)
          this.accessActions = response.data;
        } else {
          this.toastr.error(response.error)
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error)
      }
    });
  }

  applyChanges():void {
    //console.log(this.role)
    if (this.data.isEdit
    ) {
      this.applyEdit();
    } else {
      this.applyCreate();
    }
  }

  applyEdit(): void {
    this.roleService.updateRole(this.roleForm.value).subscribe({
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
    this.roleService.createRole(this.roleForm.value).subscribe({
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

  onNoClick()
    :
    void {
    this.dialogRef.close();
  }

}

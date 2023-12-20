import {Component, OnInit} from '@angular/core';
import {RoleService} from "../../../@core/services/role.service";
import {MatDialog} from "@angular/material/dialog";
import {ToastrService} from "ngx-toastr";
import {Role} from "../../../@core/models/role/role";
import {DialogService} from "../../../@core/services/dialog.service";
import {DialogData} from "../../../@core/models/dialog-data";

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.scss']
})
export class RoleListComponent implements OnInit {

  columnsToDisplay = ['name',
    'description',
    'isEnabled',
    'isBlocked',
    'isDefault',
    'actions'];

  protected roles!: Role[];

  constructor(
    private readonly roleService: RoleService,
    private readonly dialog: MatDialog,
    private readonly toastr: ToastrService,
    private readonly dialogService: DialogService,
  ) {
  }

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    this.roleService.getRoles().subscribe({
      next: (response) => {
        this.roles = response.data!;
      },
      error: (error) => {
        this.toastr.error(error.error.error);
      }
    })
  }

  async openRole(id: number): Promise<void> {
    const dialogRef = this.dialog.open(RoleListComponent, {
      data: {data: id, isEdit: true} as DialogData<number, null>,
    });

    dialogRef.afterOpened().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

  deleteRole(id: number): void {
    this.dialogService.openDialog({
      data: {
        title: "Delete role",
        message: "Are you sure you want to delete this role?",
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
      this.roleService.deleteRole(id).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.toastr.success("Operation successful");
            this.refresh();
          } else {
            this.toastr.error(response.error);
          }
        },
        error: (error) => {
          this.toastr.error(error.error.error);
        }
      });
    } catch (e) {
      console.error(e)
    }
  }


}

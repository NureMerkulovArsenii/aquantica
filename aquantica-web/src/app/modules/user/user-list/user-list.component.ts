import {Component, OnInit} from '@angular/core';
import {AccountService} from "../../../@core/services/account.service";
import {ToastrService} from "ngx-toastr";
import {MatDialog} from "@angular/material/dialog";
import {DialogService} from "../../../@core/services/dialog.service";
import {User} from "../../../@core/models/user/user";
import {UserDetailedComponent} from "../user-detailed/user-detailed.component";
import {DialogData} from "../../../@core/models/dialog-data";

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  protected users!: User[];
  protected columnsToDisplay = [
    'email',
    'firstName',
    'lastName',
    'isEnabled',
    'isBlocked',
    'actions'
  ]

  constructor(
    private readonly accountService: AccountService,
    private readonly toastr: ToastrService,
    private readonly dialog: MatDialog,
    private readonly dialogService: DialogService
  ) {
  }

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    try {
      this.accountService.getAll().subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.users = response.data!;
          } else {
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

  openUser(id: number): void {
    const dialogRef = this.dialog.open(UserDetailedComponent, {
      data: {data: id, isEdit: true} as DialogData<number, null>,
    });

    dialogRef.afterClosed().subscribe(result => {
      this.refresh();
    });
  }

  registerUser(): void {
    const dialogRef = this.dialog.open(UserDetailedComponent, {
      data: {isEdit: false, data: null} as DialogData<number, null>,
    });

    dialogRef.afterClosed().subscribe(result => {
      this.refresh();
    });
  }

}

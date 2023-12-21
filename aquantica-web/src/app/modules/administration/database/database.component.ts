import {Component} from '@angular/core';
import {ToastrService} from "ngx-toastr";
import {AdministrationService} from "../../../@core/services/administration.service";

@Component({
  selector: 'app-database',
  templateUrl: './database.component.html',
  styleUrls: ['./database.component.scss']
})
export class DatabaseComponent {

  constructor(
    private readonly toastr: ToastrService,
    private readonly adminService: AdministrationService,
  ) {

  }

  createBackup(): void {
    this.adminService.createBackup().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastr.success("Backup created successfully");
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

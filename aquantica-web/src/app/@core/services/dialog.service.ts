import {Injectable} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {DialogModel} from "../models/dialog-model";
import {ConfirmDialogComponent} from "../../shared/components/confirm-dialog/confirm-dialog.component";

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(
    private readonly dialog: MatDialog
  ) {
  }

  openDialog(dialog: DialogModel): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: dialog,
    });

    dialogRef.afterClosed().subscribe(result => {
      dialog.onClose(result);
    });
    
  }

}

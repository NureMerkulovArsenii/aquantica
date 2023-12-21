import {Component, OnInit} from '@angular/core';
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-jobs-list',
  templateUrl: './jobs-list.component.html',
  styleUrls: ['./jobs-list.component.scss']
})
export class JobsListComponent implements OnInit{

  constructor(
    private readonly toastr: ToastrService

  ) {
  }

  ngOnInit() {
  }

  refresh(){

  }

}

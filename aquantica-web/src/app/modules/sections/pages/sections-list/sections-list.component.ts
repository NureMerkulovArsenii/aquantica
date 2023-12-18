import {Component, OnInit} from '@angular/core';
import {SectionService} from "../../../../@core/services/section.service";
import {Section} from "../../../../@core/models/section/section";

@Component({
  selector: 'app-sections-list',
  templateUrl: './sections-list.component.html',
  styleUrls: ['./sections-list.component.scss']
})
export class SectionsListComponent implements OnInit {
  sections: Section[] = [];
  columnsToDisplay = ['name', 'number', 'isEnabled', 'deviceUri', 'actions'];


  constructor(private readonly sectionService: SectionService) {
  }

  async ngOnInit(): Promise<void> {
    try {
      this.sections = await this.sectionService.getSections()
      console.log(this.sections)
    } catch (e) {
      console.error(e)
    }
  }
}

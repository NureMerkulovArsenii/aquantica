import {Component, OnInit} from '@angular/core';
import {SectionService} from "../../../../@core/services/section.service";
import {ActivatedRoute} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {SectionDetails} from "../../../../@core/models/section/section-details";
import {RulesetService} from "../../../../@core/services/ruleset.service";
import {Ruleset} from "../../../../@core/models/ruleset/ruleset";

@Component({
  selector: 'app-section-details',
  templateUrl: './section-details.component.html',
  styleUrls: ['./section-details.component.scss']
})
export class SectionDetailsComponent implements OnInit {
  protected section: SectionDetails = {} as SectionDetails;
  protected ruleSets: Ruleset[] = [];

  constructor(
    private readonly sectionService: SectionService,
    private readonly rulesetService: RulesetService,
    private readonly route: ActivatedRoute,
    private readonly toastr: ToastrService
  ) {
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.sectionService.getSection(+id!).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.section = response.data!;
        } else {
          this.toastr.error(response.error)
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error)
      }
    });

    this.getRuleSets();
  }

  getRuleSets(): Ruleset[] {
    this.rulesetService.getAll().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.ruleSets = response.data!;
        } else {
          this.toastr.error(response.error)
        }
      },
      error: (error) => {
        this.toastr.error(error.error.error)
      }
    });

    return this.ruleSets;
  }

  editRuleSet(): void {
    console.log(this.section)

  }

  applyChanges(): void {
    console.log(this.section)
    this.sectionService.updateSection(this.section).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastr.success("Operation successful")
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

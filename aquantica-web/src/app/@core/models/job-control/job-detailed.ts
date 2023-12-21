import {JobRepetitionType} from "../../enums/job-repetition-type";
import {JobMethod} from "../../enums/job-method";
import {Section} from "../section/section";

export interface JobDetailed {
  id: number;
  name: string;
  isEnabled: boolean;
  jobRepetitionType: JobRepetitionType; // Make sure JobRepetitionType is defined in TypeScript
  jobRepetitionValue: number;
  jobMethod: JobMethod;
  irrigationSection: Section | null;

}

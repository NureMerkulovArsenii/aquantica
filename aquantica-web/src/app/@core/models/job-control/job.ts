import {JobRepetitionType} from "../../enums/job-repetition-type";
import {JobMethod} from "../../enums/job-method";

export interface Job {
  id: number;
  name: string;
  isEnabled: boolean;
  jobRepetitionType: JobRepetitionType; // Make sure JobRepetitionType is defined in TypeScript
  jobRepetitionValue: number;
  jobMethod: JobMethod;
}

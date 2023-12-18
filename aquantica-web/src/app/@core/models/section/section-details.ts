import {Location} from "../location/location";

export interface SectionDetails {
  id: number;
  number: number;
  name: string | null;
  parentId: number | null;
  isEnabled: boolean;
  deviceUri: string;
  sectionRulesetId: number | null;
  sectionTypeId: number;
  location: Location | null;
}

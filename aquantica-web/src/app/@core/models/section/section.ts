export interface Section {
  id: number;
  number: number;
  name?: string | null;
  parentId?: number | null;
  isEnabled: boolean;
  deviceUri: string;
  sectionRulesetId?: number | null;
  locationName?: string | null;
}

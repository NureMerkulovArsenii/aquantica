export interface IrrigationSection {
  id: number;
  number: number;
  name?: string | null; // Assuming `string?` corresponds to an optional string in TypeScript
  parentId?: number | null;
  isEnabled: boolean;
  deviceUri: string;
  sectionRulesetId?: number | null;
  parentNumber?: number | null;
  locationId?: number | null;
}

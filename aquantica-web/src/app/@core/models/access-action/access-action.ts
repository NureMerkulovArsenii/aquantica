export interface AccessAction {
  id: number;
  code: string;
  name: string;
  description: string | null;
  isEnabled: boolean;
  roleIds: number[];
}

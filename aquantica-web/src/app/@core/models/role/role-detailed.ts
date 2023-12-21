export interface RoleDetailed {
  id: number;
  name: string;
  description?: string | null;
  isEnabled: boolean;
  isBlocked: boolean;
  isDefault: boolean;
  accessActionsIds: number[] | null;
  userIds: number[] | null;
}

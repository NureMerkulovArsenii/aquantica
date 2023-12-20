export interface RoleDetailed {
  name: string;
  description?: string | null;
  isEnabled: boolean;
  isBlocked: boolean;
  isDefault: boolean;
  accessActionsIds: number[] | null;
  userIds: number[] | null;
}

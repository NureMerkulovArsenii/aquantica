export interface UserUpdate {
  id: number;
  email: string;
  firstName: string | null;
  lastName: string | null;
  isEnabled: boolean;
  isBlocked: boolean;
  roleId: number;
}

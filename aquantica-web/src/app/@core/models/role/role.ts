export interface Role {
  id: number;
  name: string;
  description?: string | null;
  isEnabled: boolean;
  isBlocked: boolean;
  isDefault: boolean;
}

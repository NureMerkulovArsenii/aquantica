import {Role} from "../role/role";

export interface User {
  id: number;
  email: string;
  firstName: string | null;
  lastName: string | null;
  isEnabled: boolean;
  isBlocked: boolean;
  role: Role;
}

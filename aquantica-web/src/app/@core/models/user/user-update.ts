import {Role} from "../role/role";

export interface UserUpdate {
  id: number;
  email: string;
  phoneNumber: string | null;
  firstName: string;
  lastName: string;
  password: string;
  isEnabled: boolean;
  isBlocked: boolean;
  role: Role | null;
}

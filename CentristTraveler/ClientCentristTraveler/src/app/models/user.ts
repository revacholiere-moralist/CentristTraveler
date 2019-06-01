import { Role } from './role';

export class User {
  id: number;
  username: string;
  password: string;
  email: string;
  display_name: string;
  roles: Role[];
}

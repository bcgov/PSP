import { Api_ConcurrentVersion } from 'models/api/ConcurrentVersion';

import { Api_Role } from './Role';
import { Api_User } from './User';
export interface Api_UserRole extends Api_ConcurrentVersion {
  roleId?: number;
  userId?: number;
  role?: Api_Role;
  user?: Api_User;
}

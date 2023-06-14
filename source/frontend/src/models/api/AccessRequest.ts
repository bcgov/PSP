import { Api_ConcurrentVersion } from '@/models/api/ConcurrentVersion';

import { Api_AuditFields } from './AuditFields';
import { Api_Role } from './Role';
import Api_TypeCode from './TypeCode';
import { Api_User } from './User';

export interface Api_AccessRequest extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  userId: number;
  roleId?: number;
  user?: Api_User;
  role?: Api_Role;
  note?: string;
  accessRequestStatusTypeCode?: Api_TypeCode<string>;
  regionCode?: Api_TypeCode<number>;
}

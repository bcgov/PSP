import { Api_ConcurrentVersion } from '@/models/api/ConcurrentVersion';
import { Api_Person } from '@/models/api/Person';

import { Api_AuditFields } from './AuditFields';
import { Api_RegionUser } from './RegionUser';
import Api_TypeCode from './TypeCode';
import { Api_UserRole } from './UserRole';

export interface Api_User extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  businessIdentifierValue?: string;
  guidIdentifierValue?: string;
  approvedById?: number;
  position?: string;
  note?: string;
  isDisabled?: boolean;
  issueDate?: string;
  lastLogin?: string;
  userTypeCode?: Api_TypeCode<string>;
  userRoles: Api_UserRole[];
  userRegions: Api_RegionUser[];
  person?: Api_Person;
}

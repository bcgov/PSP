import { Api_ConcurrentVersion } from 'models/api/ConcurrentVersion';
import { Api_Person } from 'models/api/Person';

import { Api_RegionUser } from './RegionUser';
import { Api_UserRole } from './UserRole';
export interface Api_User extends Api_ConcurrentVersion {
  id?: number;
  businessIdentifierValue?: string;
  guidIdentifierValue?: string;
  approvedById?: number;
  position?: string;
  note?: string;
  isDisabled?: boolean;
  issueDate?: string;
  lastLogin?: string;
  appCreateTimestamp?: string;
  userRoles: Api_UserRole[];
  userRegions: Api_RegionUser[];
  person?: Api_Person;
}

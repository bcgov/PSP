import { Api_ConcurrentVersion } from 'models/api/ConcurrentVersion';
import { Api_Person } from 'models/api/Person';

import { Api_Role } from './Role';
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
  roles?: Api_Role[];
  person?: Api_Person;
}

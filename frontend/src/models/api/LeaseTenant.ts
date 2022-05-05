import ITypeCode from 'interfaces/ITypeCode';

import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';

export interface Api_LeaseTenant extends Api_ConcurrentVersion {
  id?: number;
  leaseId: number;
  personId?: number;
  person?: Api_Person;
  organizationId?: number;
  organization?: Api_Organization;
  lessorTypeCode?: ITypeCode<string>;
  primaryContactId?: number;
  primaryContact?: Api_Person;
  note?: string;
}

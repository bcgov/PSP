import ITypeCode from '@/interfaces/ITypeCode';

import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';
import Api_TypeCode from './TypeCode';

export interface Api_LeaseTenant extends Api_ConcurrentVersion {
  id?: number;
  leaseId: number;
  personId?: number;
  person?: Api_Person;
  organizationId?: number;
  organization?: Api_Organization;
  lessorType?: ITypeCode<string>;
  tenantTypeCode?: Api_TypeCode<string>;
  primaryContactId?: number;
  primaryContact?: Api_Person;
  note?: string;
}

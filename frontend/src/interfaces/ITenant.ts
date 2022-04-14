import { Api_Person } from 'models/api/Person';

import { IOrganization } from '.';
export interface ITenant {
  leaseId?: number;
  person?: Api_Person;
  personId?: number;
  organization?: IOrganization;
  organizationId?: number;
  leaseTenantId?: number;
  note: string;
  name?: string;
  rowVersion?: number;
}

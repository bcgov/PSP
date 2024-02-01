import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';

import { IOrganization } from '.';

export interface ITenant {
  id?: string;
  leaseId?: number;
  person?: ApiGen_Concepts_Person;
  personId?: number;
  organization?: IOrganization;
  organizationId?: number;
  leaseTenantId?: number;
  note: string;
  name?: string;
  rowVersion?: number;
}

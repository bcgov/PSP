import { IOrganization, IPerson } from '.';
export interface ITenant {
  leaseId?: number;
  person?: IPerson;
  personId?: number;
  organization?: IOrganization;
  organizationId?: number;
  leaseTenantId?: number;
  note: string;
  name?: string;
  rowVersion?: number;
}

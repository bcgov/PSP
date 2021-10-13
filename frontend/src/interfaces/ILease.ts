import { IOrganization, IPerson, IProperty } from '.';
export interface ILease {
  id?: number;
  lFileNo?: string;
  pidOrPin?: string;
  programName?: string;
  tenantName?: string;
  address?: string;
  expiryDate?: string;
  properties: IProperty[];
  persons: IPerson[];
  organizations: IOrganization[];
  paymentReceivableTypeId?: string;
}

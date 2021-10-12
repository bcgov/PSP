import { IProperty, ITenant } from '.';
export interface ILease {
  id?: number;
  lFileNo?: string;
  pidOrPin?: string;
  programName?: string;
  tenantName?: string;
  address?: string;
  expiryDate?: string;
  properties?: IProperty[];
  tenant?: ITenant;
}

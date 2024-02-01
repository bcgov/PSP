import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';

export interface ILeaseSearchResult {
  id: number;
  lFileNo: string;
  expiryDate?: string;
  programName?: string;
  tenantNames: string[];
  properties: ILeaseProperty[];
  statusType?: ApiGen_Base_CodeType<string>;
}

export interface ILeaseProperty {
  id: number;
  address?: string;
  pin?: string;
  pid?: string;
}

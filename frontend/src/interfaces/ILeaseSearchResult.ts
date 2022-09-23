import ITypeCode from './ITypeCode';

export interface ILeaseSearchResult {
  id: number;
  lFileNo: string;
  expiryDate?: string;
  programName?: string;
  tenantNames: string[];
  properties: ILeaseProperty[];
  statusType?: ITypeCode<string>;
}

export interface ILeaseProperty {
  id: number;
  address?: string;
  pin?: string;
  pid?: string;
}

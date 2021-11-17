export interface ILeaseSearchResult {
  id: number;
  lFileNo: string;
  programName?: string;
  tenantNames: string[];
  properties: ILeaseProperty[];
}

interface ILeaseProperty {
  id: number;
  address?: string;
  pin?: string;
  pid?: string;
}

export interface ILeaseFilter {
  pinOrPid: string;
  lFileNo: string;
  searchBy: string;
  leaseStatusTypes: string[];
  tenantName: string;
  programs: string[];
  expiryStartDate: string;
  expiryEndDate: string;
  regionType: string;
  details: string;
}

export interface ILeaseSearchBy {
  pinOrPid: string;
  address: string;
  lFileNo: string;
  historical: string;
}

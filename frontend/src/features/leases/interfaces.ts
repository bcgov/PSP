export interface ILeaseFilter {
  pinOrPid: string;
  lFileNo: string;
  searchBy: string;
  leaseStatusType: string;
  tenantName: string;
  programs: string[];
  expiryStartDate: string;
  expiryEndDate: string;
  regionType: string;
  details: string;
}

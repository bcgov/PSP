export interface ILeaseFilter {
  pid: string;
  pin: string;
  lFileNo: string;
  searchBy: string;
  leaseStatusTypes: string[];
  tenantName: string;
  programs: string[];
  expiryStartDate: string;
  expiryEndDate: string;
  regionType: string;
  details: string;
  leaseTeamPersonId: number | null;
  leaseTeamOrganizationId: number | null;
}

export interface ILeaseSearchBy {
  pin: string;
  pid: string;
  address: string;
  lFileNo: string;
  historical: string;
}

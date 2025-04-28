export interface Api_ManagementFilter {
  searchBy: string;
  pin: string;
  pid: string;
  address: string;
  fileNameOrNumberOrReference: string;
  managementFileStatusCode: string;
  managementFilePurposeCode: string;
  projectNameOrNumber: string;
  teamMemberPersonId: number | null;
  teamMemberOrganizationId: number | null;
}

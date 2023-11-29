export interface Api_DispositionFilter {
  searchBy: string;
  pin: string;
  pid: string;
  address: string;
  fileNameOrNumberOrReference: string;
  physicalFileStatusCode: string;
  dispositionStatusCode: string;
  dispositionTypeCode: string;
  teamMemberPersonId: number | null;
  teamMemberOrganizationId: number | null;
}

import { MultiSelectOption } from '@/interfaces/MultiSelectOption';

export interface Api_ManagementActivityFilter {
  searchBy: string;
  pin: string;
  pid: string;
  regionCodes: MultiSelectOption[];
  address: string;
  fileNameOrNumberOrReference: string;
  projectNameOrNumber: string;
  activityTypeCode: string;
  activityStatusCode: string;
  managementFileStatusCode: string;
  managementFilePurposeCode: string;
}

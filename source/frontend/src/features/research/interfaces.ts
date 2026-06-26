import { MultiSelectOption } from '@/interfaces/MultiSelectOption';

export interface IResearchFilter {
  pid: string;
  pin: string;
  regionCodes: string[];
  researchFileStatusTypeCode: string;
  name: string;
  roadOrAlias: string;
  rfileNumber: string;
  createOrUpdateRange: string;
  createOrUpdateBy: string;
  researchSearchBy: string;
  appCreateUserid: string;
  createdOnStartDate: string;
  createdOnEndDate: string;
  appLastUpdateUserid: string;
  updatedOnStartDate: string;
  updatedOnEndDate: string;
  selectedUser?: MultiSelectOption[];
}

import { PropertyTypes } from 'constants/propertyTypes';

export interface IProperty {
  id: number | '';
  propertyTypeId?: PropertyTypes;
  agencyId: number | '';
  agency: string;
  subAgency?: string;
  agencyFullName?: string;
  subAgencyFullName?: string;
  latitude: number | '';
  longitude: number | '';
  name?: string;
  description?: string;
  projectNumbers?: string[];
  projectStatus?: string;
  projectWorkflow?: string;
  isSensitive: boolean | '';
  createdOn?: string;
  updatedOn?: string;
  updatedByEmail?: string;
  updatedByName?: string;
}

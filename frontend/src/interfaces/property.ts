import { PropertyTypes } from 'constants/propertyTypes';

export interface IProperty {
  id: number | '';
  propertyTypeId?: PropertyTypes;
  organizationId: number | '';
  organization: string;
  subOrganization?: string;
  organizationFullName?: string;
  subOrganizationFullName?: string;
  latitude: number | '';
  longitude: number | '';
  name?: string;
  description?: string;
  isSensitive: boolean | '';
  createdOn?: string;
  updatedOn?: string;
  updatedByEmail?: string;
  updatedByName?: string;
}

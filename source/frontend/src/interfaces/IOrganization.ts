import { OrganizationIdentifierTypes, OrganizationTypes } from '@/constants/index';

import { IAddress } from './IAddress';

/**
 * An organization entity that owns property or has users.
 */
export interface IOrganization {
  id?: number;
  parentId?: any;
  parent?: string;
  name: string;
  addressId?: number;
  address?: IAddress;
  regionId?: number;
  districtId?: number;
  organizationTypeId?: OrganizationTypes;
  organizationType?: string;
  identifierTypeId?: OrganizationIdentifierTypes;
  identifierType?: string;
  identifier?: string;
  website?: string;
  isDisabled?: boolean;
  rowVersion?: number;
  landline?: string;
  mobile?: string;
  email?: string;
  contactName?: string;
}

// TODO: This whole file needs to be rethought out and refactored.
// Network URL's
import { PropertyClassificationTypes, PropertyTypes } from 'constants/index';

import { AccessRequestStatus } from './accessStatus';

// Generic Params
export interface IPaginateParams {
  page: number;
  quantity?: number;
  sort?: string | string[];
}

export interface IGetUsersParams extends IPaginateParams {
  businessIdentifier?: string;
  firstName?: string;
  surname?: string;
  email?: string;
  organization?: string;
  role?: string;
  isDisabled?: boolean;
  position?: string;
}

export interface IGetOrganizationsParams extends IPaginateParams {
  name?: string;
  description?: string;
  isDisabled?: boolean;
}

export interface IPaginateAccessRequests extends IPaginateParams {
  status?: AccessRequestStatus | null;
}

// Parcels
export interface IPropertySearchParams {
  pid?: string;
  neLatitude: number;
  neLongitude: number;
  swLatitude: number;
  swLongitude: number;
  address?: string;
  administrativeArea?: string;
  /** comma-separated list of organizations to filter by */
  organizations?: string;
  classificationId?: number;
  minLandArea?: number;
  maxLandArea?: number;
}

export interface IGeoSearchParams {
  bbox?: string;
  address?: string;
  municipality?: string;
  pid?: string;
  organizations?: string; // TODO: Switch to number[]
  classificationId?: PropertyClassificationTypes;
  minLandArea?: number;
  maxLandArea?: number;
  name?: string;
  propertyType?: PropertyTypes;
}

// Lookup Codes
export const ORGANIZATION_CODE_SET_NAME = 'Organization';
export const ROLE_CODE_SET_NAME = 'Role';
export const PROVINCE_CODE_SET_NAME = 'Province';
export const ADMINISTRATIVE_AREA_CODE_SET_NAME = 'AdministrativeArea';
export const PROPERTY_CLASSIFICATION_CODE_SET_NAME = 'PropertyClassification';
export const CONSTRUCTION_CODE_SET_NAME = 'BuildingConstructionType';
export const PREDOMINATE_USE_CODE_SET_NAME = 'BuildingPredominateUse';
export const OCCUPANT_TYPE_CODE_SET_NAME = 'BuildingOccupantType';

// TODO: This should all be removed from this and moved to the useApi* hooks.
// Organizations
export const POST_ORGANIZATIONS = () => `/admin/organizations/filter`; // get paged list of organizations

// TODO: This should all be removed from this and moved to the useApi* hooks.
// Auth Service
export const ACTIVATE_USER = () => `/auth/activate`; // get filtered properties or all if not specified.

// TODO: This should all be removed from this and moved to the useApi* hooks.
// User Service
export const POST_USERS = () => `/admin/users/my/organization`; // get paged list of users

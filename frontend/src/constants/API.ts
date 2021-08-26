// Network URL's
import queryString from 'query-string';

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
export const PROPERTIES = (params: IPropertySearchParams | null) =>
  `/properties/search?${params ? queryString.stringify(params) : ''}`; // get filtered properties or all if not specified.

export interface IGeoSearchParams {
  bbox?: string;
  address?: string;
  administrativeArea?: string;
  pid?: string;
  organizations?: string; // TODO: Switch to number[]
  classificationId?: number;
  minLandArea?: number;
  maxLandArea?: number;
  name?: string;
  bareLandOnly?: boolean;
  constructionTypeId?: number;
  predominateUseId?: number;
  floorCount?: number;
  rentableArea?: number;
  propertyType?: string;
  includeAllProperties?: boolean;
}
export const PARCELS_DETAIL = (params: IPropertySearchParams | null) => {
  return `/properties/parcels?${params ? queryString.stringify(params) : ''}`; // get filtered properties or all if not specified.
};
export interface IParcelDetailParams {
  id: number;
}

export const PARCEL_DETAIL = (params: IParcelDetailParams) => `/properties/parcels/${params.id}`;
export const PARCEL_ROOT = `/properties/parcels`;

export const BUILDING_ROOT = `/properties/buildings`;

export interface IBuildingDetailParams {
  id: number;
}
export const BUILDING_DETAIL = (params: IBuildingDetailParams) =>
  `/properties/buildings/${params.id}`;

// Lookup Codes
export const LOOKUP_CODE = () => `/lookup`;
export const LOOKUP_CODE_SET = (codeSetName: string) => `/lookup/${codeSetName}`; // get filtered properties or all if not specified.
export const ORGANIZATION_CODE_SET_NAME = 'Organization';
export const ROLE_CODE_SET_NAME = 'Role';
export const PROVINCE_CODE_SET_NAME = 'Province';
export const ADMINISTRATIVE_AREA_CODE_SET_NAME = 'AdministrativeArea';
export const PROPERTY_CLASSIFICATION_CODE_SET_NAME = 'PropertyClassification';
export const CONSTRUCTION_CODE_SET_NAME = 'BuildingConstructionType';
export const PREDOMINATE_USE_CODE_SET_NAME = 'BuildingPredominateUse';
export const OCCUPANT_TYPE_CODE_SET_NAME = 'BuildingOccupantType';

// Organizations
export const POST_ORGANIZATIONS = () => `/admin/organizations/filter`; // get paged list of organizations

// Auth Service
export const ACTIVATE_USER = () => `/auth/activate`; // get filtered properties or all if not specified.

// User Service
export const POST_USERS = () => `/admin/users/my/organization`; // get paged list of users

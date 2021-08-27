import {
  AccessRequestStatus,
  AddressTypes,
  OrganizationIdentifierTypes,
  OrganizationTypes,
  PropertyClassificationTypes,
  PropertyDataSourceTypes,
  PropertyStatusTypes,
  PropertyTenureTypes,
  PropertyTypes,
} from 'constants/index';
import { IAccessRequest, IAddress, IOrganization, IProperty, IUser } from 'interfaces';
import { ILookupCode } from 'store/slices/lookupCodes';
import { IPropertyDetail } from 'store/slices/properties';

// TODO: This needs to be removed as Administrative Areas no longer exist.
export const mockAdministrativeAreaLookups = [
  {
    code: '0',
    name: 'Victoria',
    id: 0,
    isDisabled: false,
    type: 'AdministrativeArea',
  },
  {
    code: '1',
    name: 'Royal Oak',
    id: 1,
    isDisabled: false,
    type: 'AdministrativeArea',
  },
] as ILookupCode[];

export const mockOrganizationLookups = [
  {
    id: 1,
    code: 'AEST',
    name: 'AEST',
    isDisabled: false,
    type: 'Organization',
  },
  {
    id: 2,
    code: 'HTLH',
    name: 'HTLH',
    isDisabled: false,
    type: 'Organization',
  },
  {
    id: 3,
    code: 'MOTI',
    name: 'MOTI',
    isDisabled: false,
    type: 'Organization',
  },
  {
    id: 4,
    code: 'FLNR',
    name: 'FLNR',
    isDisabled: false,
    type: 'Organization',
  },
  {
    id: 5,
    code: 'MAH',
    name: 'MAH',
    isDisabled: false,
    type: 'Organization',
  },
] as ILookupCode[];

export const mockAddress: IAddress = {
  id: 1,
  addressTypeId: AddressTypes.Physical,
  streetAddress1: '1234 mock Street',
  streetAddress2: 'N/A',
  municipality: 'Victoria',
  provinceId: 1,
  province: 'BC',
  postal: 'V1V1V1',
};

export const mockOrganization: IOrganization = {
  id: 1,
  name: 'Ministry of Transportation & Infrastructure',
  organizationTypeId: OrganizationTypes.BCMinistry,
  identifierTypeId: OrganizationIdentifierTypes.Government,
  identifier: 'I have no idea what this is',
  addressId: mockAddress.id,
  address: mockAddress,
};

export const mockUser: IUser = {
  id: 1,
  keycloakUserId: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
  businessIdentifier: 'admin',
  email: 'admin@pims.gov.bc.ca',
  displayName: 'User, Admin',
  firstName: 'Admin',
  surname: 'User',
  position: '',
  organizations: [],
  roles: [],
  createdOn: '2021-05-04T19:07:09.6920606',
  rowVersion: 1,
};

export const mockProperties = [
  {
    id: 1,
    pid: '000-000-000',
    pin: '',
    propertyTypeId: PropertyTypes.Land,
    statusId: PropertyStatusTypes.UnderAdmin,
    dataSourceId: PropertyDataSourceTypes.PAIMS,
    dataSourceEffectiveDate: new Date(),
    classificationId: PropertyClassificationTypes.CoreOperational,
    tenureId: PropertyTenureTypes.TitledLandMOTI,
    zoning: '',
    zoningPotential: '',
    encumbranceReason: '',
    isSensitive: false,
    latitude: 48,
    longitude: 123,
    name: 'test name',
    description: 'test',
    evaluations: [
      {
        evaluatedOn: '2021-05-14T17:15:58.193Z',
        key: 1,
        value: 10000,
      },
    ],
    addressId: mockAddress.id,
    address: mockAddress,
    landArea: 123,
    landLegalDescription: 'test description',
  },
  {
    id: 2,
    pid: '000-000-001',
    pin: '',
    propertyTypeId: PropertyTypes.Land,
    statusId: PropertyStatusTypes.UnderAdmin,
    dataSourceId: PropertyDataSourceTypes.PAIMS,
    dataSourceEffectiveDate: new Date(),
    classificationId: PropertyClassificationTypes.CoreOperational,
    tenureId: PropertyTenureTypes.TitledLandMOTI,
    zoning: '',
    zoningPotential: '',
    encumbranceReason: '',
    isSensitive: false,
    latitude: 49,
    longitude: 123,
    name: 'test name',
    description: 'test',
    evaluations: [
      {
        evaluatedOn: '2021-05-14T17:15:58.193Z',
        key: 1,
        value: 10000,
      },
    ],
    addressId: mockAddress.id,
    address: mockAddress,
    landArea: 123,
    landLegalDescription: 'test description',
  },
  {
    id: 100,
    pid: '000-000-000',
    pin: '',
    propertyTypeId: PropertyTypes.Building,
    statusId: PropertyStatusTypes.UnderAdmin,
    dataSourceId: PropertyDataSourceTypes.PAIMS,
    dataSourceEffectiveDate: new Date(),
    classificationId: PropertyClassificationTypes.CoreOperational,
    tenureId: PropertyTenureTypes.TitledLandMOTI,
    zoning: '',
    zoningPotential: '',
    encumbranceReason: '',
    isSensitive: false,
    latitude: 48,
    longitude: 123,
    name: 'test name',
    description: 'test',
    evaluations: [
      {
        evaluatedOn: '2021-05-14T17:15:58.193Z',
        key: 1,
        value: 10000,
      },
    ],
    addressId: mockAddress.id,
    address: mockAddress,
    landArea: 123,
    landLegalDescription: 'test description',
  },
] as IProperty[];

export const mockParcel = mockProperties[0];

export const mockBuilding = mockProperties[2];

export const mockBuildingDetail: IPropertyDetail = {
  propertyTypeId: PropertyTypes.Building,
  propertyDetail: mockBuilding,
};

export const mockParcelDetail: IPropertyDetail = {
  propertyTypeId: PropertyTypes.Land,
  propertyDetail: mockParcel,
};

export const mockAccessRequest: IAccessRequest = {
  id: 2,
  userId: 1,
  status: AccessRequestStatus.Received,
  note: '',
  user: {
    id: 1,
    keycloakUserId: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
    displayName: 'User, Admin',
    firstName: 'Admin',
    surname: 'User',
    email: 'admin@pims.gov.bc.ca',
    businessIdentifier: 'admin',
    position: '',
    createdOn: '2021-05-04T19:07:09.6920606',
  },
  organizationId: mockOrganization.id,
  organization: mockOrganization,
  role: {
    id: 1,
    name: 'Real Estate Manager',
  },

  createdOn: '2021-05-07T00:37:06.2457303',
};

export const mockParcelLayerResponse = {
  type: 'FeatureCollection',
  features: [
    {
      type: 'Feature',
      id: 'WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW.fid-666e6d6_17a1c384547_692b',
      geometry: {
        type: 'Polygon',
        coordinates: [
          [
            [-123.33988214, 48.42497172],
            [-123.3399637, 48.42525348],
            [-123.34015684, 48.4252027],
            [-123.34015485, 48.42519625],
            [-123.34002144, 48.4247626],
            [-123.33983527, 48.42480982],
            [-123.33987042, 48.42493125],
            [-123.33988214, 48.42497172],
          ],
        ],
      },
      geometry_name: 'SHAPE',
      properties: {
        PARCEL_FABRIC_POLY_ID: 4381977,
        PARCEL_NAME: '123456789',
        PLAN_NUMBER: 'VIP309',
        PIN: null,
        PID: '123456789',
        PID_NUMBER: 99996,
        PARCEL_STATUS: 'Active',
        PARCEL_CLASS: 'Subdivision',
        OWNER_TYPE: 'Private',
        PARCEL_START_DATE: null,
        MUNICIPALITY: 'Victoria, The Corporation of the City of',
        REGIONAL_DISTRICT: 'Capital Regional District',
        WHEN_UPDATED: '2021-05-10Z',
        FEATURE_AREA_SQM: 742.8769,
        FEATURE_LENGTH_M: 130.1602,
        OBJECTID: 462844205,
        SE_ANNO_CAD_DATA: null,
      },
    },
  ],
  totalFeatures: 1,
  numberMatched: 1,
  numberReturned: 1,
  timeStamp: '2021-06-17T23:04:51.421Z',
  crs: {
    type: 'name',
    properties: {
      name: 'urn:ogc:def:crs:EPSG::4326',
    },
  },
};

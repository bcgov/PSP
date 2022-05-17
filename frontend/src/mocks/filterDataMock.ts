import { ContactMethodTypes } from 'constants/contactMethodType';
import {
  AccessRequestStatus,
  OrganizationIdentifierTypes,
  OrganizationTypes,
  PropertyClassificationTypes,
  PropertyDataSourceTypes,
  PropertyStatusTypes,
  PropertyTenureTypes,
} from 'constants/index';
import { IAccessRequest, IAddress, IOrganization, IPerson, IProperty, IUser } from 'interfaces';
import { BillingInfo, LtsaOrders, OrderParent } from 'interfaces/ltsaModels';
import { Api_Person } from 'models/api/Person';
import { ILookupCode } from 'store/slices/lookupCodes';

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
  addressId: mockAddress.id ?? 0,
  address: mockAddress,
  landline: '2223334444',
  mobile: '5556667777',
};

export const mockUser: IUser = {
  id: 1,
  keycloakUserId: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
  businessIdentifierValue: 'admin',
  email: 'admin@pims.gov.bc.ca',
  displayName: 'User, Admin',
  firstName: 'Admin',
  surname: 'User',
  position: '',
  organizations: [],
  roles: [],
  appCreateTimestamp: '2021-05-04T19:07:09.6920606',
  rowVersion: 1,
  landline: '2223334444',
  mobile: '5556667777',
};

export const mockPerson: IPerson = {
  landline: '222-333-4444',
  mobile: '555-666-7777',
};

export const mockApiPerson: Api_Person = {
  id: 2,
  contactMethods: [
    { contactMethodType: { id: ContactMethodTypes.WorkPhone }, value: '222-333-4444' },
    { contactMethodType: { id: ContactMethodTypes.WorkMobile }, value: '555-666-7777' },
  ],
};

export const mockLtsaResponse: LtsaOrders = {
  parcelInfo: {
    productType: OrderParent.ProductTypeEnum.ParcelInfo,
    status: OrderParent.StatusEnum.Processing,
    fileReference: 'folio',
    orderId: '0b5b0203-6125-4edc-98c5-704481c5e3c4',
    orderedProduct: {
      fieldedData: {
        status: 'ACTIVE',
        parcelIdentifier: '009-212-434',
        registeredTitlesCount: 1,
        pendingApplicationCount: 0,
        miscellaneousNotes: 'SRW PLAN 33784\nSRW PLAN 41816\nSRW PLAN 50351\nDF L24824\n',
        tombstone: {
          taxAuthorities: [
            {
              authorityName: 'Delta, City of',
            },
          ],
        },
        legalDescription: {
          fullLegalDescription:
            'LOT 4 EXCEPT: FIRSTLY: PART ON STATUTORY RIGHT OF WAY PLAN 30557;\nSECONDLY: PART ON STATUTORY RIGHT OF WAY PLAN 45999A;\nDISTRICT LOT 26 GROUP 2 AND SECTION 11 TOWNSHIP 6 NEW WESTMINSTER DISTRICT\nPLAN 24843',
          subdividedShortLegals: [
            {
              planNumber1: '00000000000000024843',
              townshipOrTownSite2: '',
              range3: '',
              block4: '',
              subdivision5: '',
              lotOrDistrictLotOrSubLot6: '00004',
              subdivision7: '',
              lotOrParcel8: '',
              section9: '',
              quadrant10: '',
              blockOrLot11: '',
              lotOrParcel12: '',
              parcelOrBlock13: '',
              concatShortLegal: 'S/24843/////4',
              marginalNotes: '*REM',
            },
            {
              planNumber1: '00000000000000024843',
              townshipOrTownSite2: '',
              range3: '',
              block4: '',
              subdivision5: '',
              lotOrDistrictLotOrSubLot6: '00004',
              subdivision7: '',
              lotOrParcel8: '',
              section9: '',
              quadrant10: '',
              blockOrLot11: '',
              lotOrParcel12: '',
              parcelOrBlock13: '',
              concatShortLegal: 'S/24843/////4',
              marginalNotes: 'PART HIGHWAY SRW PLAN 30557',
            },
            {
              planNumber1: '00000000000000024843',
              townshipOrTownSite2: '',
              range3: '',
              block4: '',
              subdivision5: '',
              lotOrDistrictLotOrSubLot6: '00004',
              subdivision7: '',
              lotOrParcel8: '',
              section9: '',
              quadrant10: '',
              blockOrLot11: '',
              lotOrParcel12: '',
              parcelOrBlock13: '',
              concatShortLegal: 'S/24843/////4',
              marginalNotes: 'PART HIGHWAY SRW PLAN 45999A',
            },
          ],
          unsubdividedShortLegals: [],
        },
        associatedPlans: [
          {
            planType: 'SUBDIVISIONPLAN',
            planNumber: 'NWP24843',
          },
          {
            planType: 'STATUTORYRIGHTOFWAYPLAN',
            planNumber: 'NWP30557',
          },
          {
            planType: 'STATUTORYRIGHTOFWAYPLAN',
            planNumber: 'NWP33784',
          },
          {
            planType: 'STATUTORYRIGHTOFWAYPLAN',
            planNumber: 'NWP50351',
          },
          {
            planType: 'PLAN',
            planNumber: 'NWP41618',
          },
        ],
      },
    },
  },
  titleOrders: [
    {
      productType: OrderParent.ProductTypeEnum.Title,
      status: OrderParent.StatusEnum.Processing,
      fileReference: 'folio',
      orderId: '5bbb421e-b7ba-42a0-a3d7-830fcd5c0941',
      billingInfo: {
        billingModel: BillingInfo.BillingModelEnum.PROV,
        productName: 'Searches',
        productCode: 'Search',
        feeExempted: true,
        productFee: 0,
        serviceCharge: 0,
        subtotalFee: 0,
        productFeeTax: 0,
        serviceChargeTax: 0,
        totalTax: 0,
        totalFee: 0,
      },
      orderedProduct: {
        fieldedData: {
          titleStatus: 'REGISTERED',
          titleIdentifier: {
            titleNumber: 'BL264819',
            landLandDistrict: 0,
          },
          tombstone: {
            applicationReceivedDate: '1997-08-18T18:30:00Z',
            enteredDate: '1997-08-18T18:46:34Z',
            titleRemarks: '',
            marketValueAmount: '',
            fromTitles: [
              {
                titleNumber: 'BK211498',
                landLandDistrict: 0,
              },
            ],
            natureOfTransfers: [
              {
                transferReason: 'SECTION 185 LAND TITLE ACT',
              },
            ],
          },
          ownershipGroups: [
            {
              interestFractionNumerator: '1',
              interestFractionDenominator: '1',
              ownershipRemarks: '',
              titleOwners: [
                {
                  lastNameOrCorpName1: 'BAYKEY',
                  givenName: 'MORRIS GRAVES',
                  incorporationNumber: '',
                  occupationDescription: 'RETIRED',
                  address: {
                    addressLine1: '',
                    addressLine2: '',
                    city: '',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: '',
                  },
                },
                {
                  lastNameOrCorpName1: 'BAYKEY',
                  givenName: 'KATHLEEN LOIS',
                  incorporationNumber: '',
                  occupationDescription: 'RETIRED',
                  address: {
                    addressLine1: '4431 SPANTON DRIVE',
                    addressLine2: '',
                    city: 'DELTA',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: 'V4K 2W4',
                  },
                },
                {
                  lastNameOrCorpName1: 'BAYKEY',
                  givenName: 'ROCK TYLER',
                  incorporationNumber: '',
                  occupationDescription: 'CARE TAKER',
                  address: {
                    addressLine1: '',
                    addressLine2: '',
                    city: '',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: '',
                  },
                },
                {
                  lastNameOrCorpName1: 'BAYKEY',
                  givenName: 'PATRICIA GAIL',
                  incorporationNumber: '',
                  occupationDescription: 'SELF EMPLOYED',
                  address: {
                    addressLine1: '4629 RIVER ROAD WEST',
                    addressLine2: '',
                    city: 'DELTA',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: 'V4K 1R9',
                  },
                },
                {
                  lastNameOrCorpName1: 'LIETZ',
                  givenName: 'CLIFFORD',
                  incorporationNumber: '',
                  occupationDescription: 'SHOP FOREMAN',
                  address: {
                    addressLine1: '',
                    addressLine2: '',
                    city: '',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: '',
                  },
                },
                {
                  lastNameOrCorpName1: 'LIETZ',
                  givenName: 'LEILAH MARIA',
                  incorporationNumber: '',
                  occupationDescription: 'HOMEMAKER',
                  address: {
                    addressLine1: '4369 - 41B STREET',
                    addressLine2: '',
                    city: 'DELTA',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: 'V4K 2K8',
                  },
                },
              ],
            },
          ],
          taxAuthorities: [
            {
              authorityName: 'Delta, City of',
            },
          ],
          descriptionsOfLand: [
            {
              parcelStatus: 'AActive',
              parcelIdentifier: '009-212-434',
              fullLegalDescription:
                'LOT 4 EXCEPT: FIRSTLY: PART ON STATUTORY RIGHT OF WAY PLAN 30557;\nSECONDLY: PART ON STATUTORY RIGHT OF WAY PLAN 45999A;\nDISTRICT LOT 26 GROUP 2 AND SECTION 11 TOWNSHIP 6 NEW WESTMINSTER DISTRICT\nPLAN 24843',
            },
          ],
          legalNotationsOnTitle: [
            {
              legalNotationNumber: 'CV1442411',
              status: 'ACTIVE',
              legalNotation: {
                originalLegalNotationNumber: 'CV1442411',
                legalNotationText:
                  'THIS CERTIFICATE OF TITLE MAY BE AFFECTED BY THE AGRICULTURAL LAND\nCOMMISSION ACT, SEE AGRICULTURAL LAND RESERVE PLAN NO. 2\nDEPOSITED JULY 30TH, 1974.',
              },
            },
            {
              legalNotationNumber: 'CV1442412',
              status: 'ACTIVE',
              legalNotation: {
                originalLegalNotationNumber: 'CV1442412',
                legalNotationText:
                  'ZONING REGULATION AND PLAN UNDER THE AERONAUTICS ACT (CANADA) FILED\n10.2.1981 UNDER NO. T17084 PLAN NO. 61216',
              },
            },
            {
              legalNotationNumber: 'CV1442414',
              status: 'ACTIVE',
              legalNotation: {
                originalLegalNotationNumber: 'CV1442414',
                legalNotationText:
                  'LAND HEREIN CHARGED UNDER THE\nMUNICIPAL ACT, PART 25\nD.F. D21890',
              },
            },
          ],
          chargesOnTitle: [
            {
              status: 'REGISTERED',
              interAlia: 'NO',
              chargeNumber: 'H107642',
              enteredDate: '1997-08-18T18:46:34Z',
              chargeRemarks: '',
              chargeRelease: {},
              charge: {
                chargeNumber: 'H107642',
                transactionType: 'STATUTORY RIGHT-OF-WAY',
                applicationReceivedDate: '1972-10-23T21:38:00Z',
                chargeOwnershipGroups: [
                  {
                    jointTenancyIndication: false,
                    interestFractionNumerator: '1',
                    interestFractionDenominator: '1',
                    ownershipRemarks: '',
                    chargeOwners: [
                      {
                        lastNameOrCorpName1: 'BRITISH COLUMBIA HYDRO AND POWER AUTHORITY',
                        incorporationNumber: '',
                      },
                    ],
                  },
                ],
                certificatesOfCharge: [],
                correctionsAltos1: [],
                corrections: [],
              },
            },
            {
              status: 'REGISTERED',
              interAlia: 'NO',
              chargeNumber: 'N35326',
              enteredDate: '1997-08-18T18:46:34Z',
              chargeRemarks: 'PLAN 50351\n',
              chargeRelease: {},
              charge: {
                chargeNumber: 'N35326',
                transactionType: 'STATUTORY RIGHT-OF-WAY',
                applicationReceivedDate: '1977-04-14T22:32:00Z',
                chargeOwnershipGroups: [
                  {
                    jointTenancyIndication: false,
                    interestFractionNumerator: '1',
                    interestFractionDenominator: '1',
                    ownershipRemarks: '',
                    chargeOwners: [
                      {
                        lastNameOrCorpName1: 'THE CORPORATION OF DELTA',
                        incorporationNumber: '',
                      },
                    ],
                  },
                ],
                certificatesOfCharge: [],
                correctionsAltos1: [],
                corrections: [],
              },
            },
            {
              status: 'REGISTERED',
              interAlia: 'NO',
              chargeNumber: 'BL158885',
              enteredDate: '1997-08-29T19:39:37Z',
              chargeRemarks: 'MODIFIED BY BR248606\n',
              chargeRelease: {},
              charge: {
                chargeNumber: 'BL158885',
                transactionType: 'MORTGAGE',
                applicationReceivedDate: '1997-05-07T16:51:00Z',
                chargeOwnershipGroups: [
                  {
                    jointTenancyIndication: false,
                    interestFractionNumerator: '1',
                    interestFractionDenominator: '1',
                    ownershipRemarks: '',
                    chargeOwners: [
                      {
                        lastNameOrCorpName1: 'RICHMOND SAVINGS CREDIT UNION',
                        incorporationNumber: '',
                      },
                    ],
                  },
                ],
                certificatesOfCharge: [],
                correctionsAltos1: [],
                corrections: [],
              },
            },
            {
              status: 'REGISTERED',
              interAlia: 'NO',
              chargeNumber: 'BR248606',
              enteredDate: '2001-09-27T16:28:50Z',
              chargeRemarks: 'MODIFICATION OF BL158885\n',
              chargeRelease: {},
              charge: {
                chargeNumber: 'BR248606',
                transactionType: 'MORTGAGE',
                applicationReceivedDate: '2001-09-25T18:11:00Z',
                chargeOwnershipGroups: [],
                certificatesOfCharge: [],
                correctionsAltos1: [],
                corrections: [],
              },
            },
            {
              status: 'REGISTERED',
              interAlia: 'NO',
              chargeNumber: 'CA5338545',
              enteredDate: '2016-07-21T22:16:04Z',
              chargeRemarks: '',
              chargeRelease: {},
              charge: {
                chargeNumber: 'CA5338545',
                transactionType: 'STATUTORY RIGHT OF WAY',
                applicationReceivedDate: '2016-07-13T21:01:28Z',
                chargeOwnershipGroups: [
                  {
                    jointTenancyIndication: false,
                    interestFractionNumerator: '1',
                    interestFractionDenominator: '1',
                    ownershipRemarks: '',
                    chargeOwners: [
                      {
                        lastNameOrCorpName1: 'ROGERS COMMUNICATIONS INC.',
                        incorporationNumber: 'BC0921753',
                      },
                    ],
                  },
                ],
                certificatesOfCharge: [],
                correctionsAltos1: [],
                corrections: [],
              },
            },
          ],
          duplicateCertificatesOfTitle: [
            {
              certificateIdentifier: {
                documentNumber: '12345',
                documentDistrictCode: 'test district code',
              },
              certificateDelivery: {
                certificateText: 'certificate text',
                intendedRecipientLastName: 'last',
                intendedRecipientGivenName: 'given',
              },
            },
          ],
          titleTransfersOrDispositions: [
            {
              disposition: 'disposition text',
              disposationDate: '2020-01-01',
              titleNumber: '54321',
              landLandDistrict: 'district',
            },
          ],
        },
      },
    },
  ],
};

export const mockProperties = [
  {
    id: 1,
    pid: '000-000-000',
    pin: '',
    statusId: PropertyStatusTypes.UnderAdmin,
    dataSourceId: PropertyDataSourceTypes.PAIMS,
    dataSourceEffectiveDate: '2021-08-30T17:28:17.655Z',
    classificationId: PropertyClassificationTypes.CoreOperational,
    tenureId: PropertyTenureTypes.HighwayRoad,
    zoning: '',
    zoningPotential: '',
    encumbranceReason: '',
    isSensitive: false,
    latitude: 48,
    longitude: 123,
    name: 'test name',
    description: 'test',
    addressId: mockAddress.id,
    address: mockAddress,
    landArea: 123,
    landLegalDescription: 'test description',
  },
  {
    id: 2,
    pid: '000-000-001',
    pin: '',
    statusId: PropertyStatusTypes.UnderAdmin,
    dataSourceId: PropertyDataSourceTypes.PAIMS,
    dataSourceEffectiveDate: '2021-08-30T18:14:13.170Z',
    classificationId: PropertyClassificationTypes.CoreOperational,
    tenureId: PropertyTenureTypes.HighwayRoad,
    zoning: '',
    zoningPotential: '',
    encumbranceReason: '',
    isSensitive: false,
    latitude: 49,
    longitude: 123,
    name: 'test name',
    description: 'test',
    addressId: mockAddress.id,
    address: mockAddress,
    landArea: 123,
    landLegalDescription: 'test description',
  },
  {
    id: 100,
    pid: '000-000-000',
    pin: '',
    statusId: PropertyStatusTypes.UnderAdmin,
    dataSourceId: PropertyDataSourceTypes.PAIMS,
    dataSourceEffectiveDate: '2021-08-30T18:14:13.170Z',
    classificationId: PropertyClassificationTypes.CoreOperational,
    tenureId: PropertyTenureTypes.HighwayRoad,
    zoning: '',
    zoningPotential: '',
    encumbranceReason: '',
    isSensitive: false,
    latitude: 48,
    longitude: 123,
    name: 'test name',
    description: 'test',
    addressId: mockAddress.id,
    address: mockAddress,
    landArea: 123,
    landLegalDescription: 'test description',
  },
] as IProperty[];

export const mockParcel = mockProperties[0];

export const mockParcelDetail = {
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
    businessIdentifierValue: 'admin',
    position: '',
    appCreateTimestamp: '2021-05-04T19:07:09.6920606',
  },
  organizationId: mockOrganization.id,
  organization: mockOrganization,
  role: {
    id: 1,
    name: 'Real Estate Manager',
  },

  appCreateTimestamp: '2021-05-07T00:37:06.2457303',
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

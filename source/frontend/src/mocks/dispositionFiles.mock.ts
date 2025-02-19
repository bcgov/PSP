import { ApiGen_CodeTypes_DispositionFileStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_DispositionFileStatusTypes';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_DispositionFileAppraisal } from '@/models/api/generated/ApiGen_Concepts_DispositionFileAppraisal';
import { ApiGen_Concepts_DispositionFileOffer } from '@/models/api/generated/ApiGen_Concepts_DispositionFileOffer';
import { ApiGen_Concepts_DispositionFileSale } from '@/models/api/generated/ApiGen_Concepts_DispositionFileSale';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { toTypeCode } from '@/utils/formUtils';

import { getEmptyPerson } from './contacts.mock';
import { getEmptyOrganization } from './organization.mock';

export const mockDispositionFileResponse = (
  id = 1,
  name = 'Test Disposition File',
  rowVersion = 1,
): ApiGen_Concepts_DispositionFile => ({
  fileReference: 'FILE_REFERENCE 8128827 3EAD56A',
  initiatingDocumentDate: '1917-06-29T00:00:00',
  assignedDate: '2025-04-26T00:00:00',
  completionDate: '1956-05-28T00:00:00',
  projectId: 1,
  project: {
    id: 1,
    projectStatusTypeCode: null,
    businessFunctionCode: null,
    costTypeCode: null,
    workActivityCode: null,
    regionCode: null,
    code: '00048',
    description: 'CLAIMS',
    note: null,
    projectPersons: [],
    projectProducts: [],
    appCreateTimestamp: '2024-02-06T20:56:46.47',
    appLastUpdateTimestamp: '2024-02-06T20:56:46.47',
    appLastUpdateUserid: 'dbo',
    appCreateUserid: 'dbo',
    appLastUpdateUserGuid: null,
    appCreateUserGuid: null,
    rowVersion: 1,
  },
  productId: 10,
  product: {
    id: 10,
    projectProducts: [],
    acquisitionFiles: [],
    code: '00055',
    description: 'AVALANCHE  \u0026  PROGRAM REVIEW',
    startDate: null,
    costEstimate: null,
    costEstimateDate: null,
    objective: null,
    scope: null,
    appCreateTimestamp: '2024-02-06T20:56:46.58',
    appLastUpdateTimestamp: '2024-02-06T20:56:46.58',
    appLastUpdateUserid: 'dbo',
    appCreateUserid: 'dbo',
    appLastUpdateUserGuid: null,
    appCreateUserGuid: null,
    rowVersion: 1,
  },
  dispositionTypeCode: {
    id: 'CLOSURE',
    description: 'Road Closure',
    isDisabled: false,
    displayOrder: 10,
  },
  dispositionStatusTypeCode: toTypeCode(ApiGen_CodeTypes_DispositionFileStatusTypes.ACTIVE),
  initiatingBranchTypeCode: {
    id: 'PLMB',
    description: 'PLMB',
    isDisabled: false,
    displayOrder: 10,
  },
  physicalFileStatusTypeCode: {
    id: 'PENDING',
    description: 'Pending Litigation',
    isDisabled: false,
    displayOrder: 10,
  },
  fundingTypeCode: null,
  initiatingDocumentTypeCode: {
    id: 'SURPLUS',
    description: 'Surplus Declaration',
    isDisabled: false,
    displayOrder: 10,
  },
  dispositionTypeOther: null,
  initiatingDocumentTypeOther: null,
  regionCode: {
    id: 4,
    description: 'Cannot determine',
    isDisabled: false,
    displayOrder: 10,
  },
  fileProperties: [],
  dispositionTeam: [
    {
      id: 9,
      dispositionFileId: 120,
      personId: 6,
      person: {
        ...getEmptyPerson(),
        id: 6,
        isDisabled: false,
        surname: 'Sanchez',
        middleNames: '',
        firstName: 'Alejandro',
        preferredName: 'Alex',
        personOrganizations: [],
        personAddresses: [],
        contactMethods: [],
        comment: null,
        rowVersion: 1,
      },
      organizationId: null,
      organization: null,
      primaryContactId: null,
      primaryContact: null,
      teamProfileTypeCode: 'MOTILAWYER',
      teamProfileType: {
        id: 'MOTILAWYER',
        description: 'MoTI Solicitor',
        isDisabled: false,
        displayOrder: 1,
      },
      rowVersion: 1,
    },
    {
      id: 10,
      dispositionFileId: 120,
      personId: 9,
      person: {
        ...getEmptyPerson(),
        id: 9,
        isDisabled: false,
        surname: 'Monga',
        firstName: 'Aman',
        middleNames: '',
        preferredName: 'Aman',
        personOrganizations: [],
        personAddresses: [],
        contactMethods: [],
        comment: null,
        rowVersion: 1,
      },
      organizationId: null,
      organization: null,
      primaryContactId: null,
      primaryContact: null,
      teamProfileTypeCode: 'NEGOTAGENT',
      teamProfileType: {
        id: 'NEGOTAGENT',
        description: 'Negotiation agent',
        isDisabled: false,
        displayOrder: 10,
      },
      rowVersion: 1,
    },
  ],
  id: id,
  fileName: name ?? 'FILE_NAME B8233BF E8C7408',
  fileNumber: 'FILE_NUMBER 3A8F46B',
  fileStatusTypeCode: {
    id: 'DRAFT',
    description: 'Draft',
    isDisabled: false,
    displayOrder: 10,
  },
  dispositionAppraisal: {
    id: 250,
    dispositionFileId: id,
    appraisedAmount: 550000,
    appraisalDate: '2023-12-28T00:00:00',
    bcaValueAmount: 600000,
    bcaRollYear: '2023',
    listPriceAmount: 590000,
    ...getEmptyBaseAudit(1),
  },
  dispositionOffers: [],
  dispositionSale: null,
  totalAllowableCompensation: null,
  appCreateTimestamp: '2023-11-25T20:48:26.693',
  appLastUpdateTimestamp: '2023-11-24T20:48:26.693',
  appLastUpdateUserid: 'FOUGSTER',
  appCreateUserid: 'FOUGSTER',
  appLastUpdateUserGuid: '672bef2d-f689-4ad0-8201-1b6a40665e07',
  appCreateUserGuid: '5c01a037-8595-4f9c-b2d3-7d26b0095d36',
  rowVersion: rowVersion,
  fileChecklistItems: [],
});

export const mockDispositionFilePropertyResponse = () => [
  {
    id: 1,
    propertyName: null,
    displayOrder: null,
    property: {
      id: 227,
      propertyType: null,
      anomalies: [],
      tenures: [],
      roadTypes: [],
      status: null,
      dataSource: null,
      region: {
        id: 1,
        description: 'South Coast Region',
        isDisabled: false,
        displayOrder: null,
      },
      district: {
        id: 1,
        description: 'Lower Mainland District',
        isDisabled: false,
        displayOrder: null,
      },
      dataSourceEffectiveDate: '2021-08-31T00:00:00',
      latitude: 49.239094925567755,
      longitude: -122.8796576490368,
      name: null,
      description: null,
      isSensitive: false,
      isProvincialPublicHwy: null,
      pphStatusUpdateUserid: null,
      pphStatusUpdateTimestamp: null,
      pphStatusUpdateUserGuid: null,
      isRwyBeltDomPatent: false,
      pphStatusTypeCode: null,
      address: {
        id: 1,
        streetAddress1: '45 - 904 Hollywood Crescent',
        streetAddress2: 'Living in a van',
        streetAddress3: 'Down by the River',
        municipality: 'Hollywood North',
        provinceStateId: 1,
        province: {
          id: 1,
          code: 'BC',
          description: 'British Columbia',
          displayOrder: 10,
        },
        countryId: 1,
        country: {
          id: 1,
          code: 'CA',
          description: 'Canada',
          displayOrder: 1,
        },
        district: null,
        region: null,
        countryOther: null,
        postal: 'V6Z 5G7',
        latitude: null,
        longitude: null,
        comment: null,
        rowVersion: 2,
      },
      pid: 13852752,
      pin: null,
      planNumber: null,
      isOwned: false,
      isPropertyOfInterest: false,
      isVisibleToOtherAgencies: false,
      areaUnit: null,
      landArea: 1,
      isVolumetricParcel: false,
      volumetricMeasurement: null,
      volumetricUnit: null,
      volumetricType: null,
      landLegalDescription: null,
      municipalZoning: null,
      zoning: null,
      zoningPotential: null,
      location: {
        coordinate: {
          x: -122.8796576490368,
          y: 49.239094925567755,
        },
      },
      boundary: {
        coordinates: [
          {
            x: 1227371.213199999,
            y: 474215.6899999976,
          },
          {
            x: 1227382.344799999,
            y: 474194.5275999997,
          },
          {
            x: 1227356.9580000008,
            y: 474208.35579999816,
          },
          {
            x: 1227371.213199999,
            y: 474215.6899999976,
          },
        ],
      },
      generalLocation: null,
      propertyContacts: null,
      notes: null,
      surplusDeclarationType: null,
      surplusDeclarationComment: null,
      surplusDeclarationDate: '0001-01-01T00:00:00',
      rowVersion: 3,
    },
    propertyId: 227,
    file: null,
    fileId: 4,
    rowVersion: 1,
  },
];

export const mockDispositionFileOfferApi = (
  id = 0,
  dispositionFileId = 1,
): ApiGen_Concepts_DispositionFileOffer => ({
  id: id,
  dispositionFileId: dispositionFileId,
  dispositionOfferStatusTypeCode: 'OPEN',
  dispositionOfferStatusType: {
    id: 'OPEN',
    description: 'Open',
    isDisabled: false,
    displayOrder: null,
  },
  offerName: 'TEST OFFER NAME',
  offerDate: '2023-12-25T00:00:00',
  offerExpiryDate: '2024-12-25T00:00:00',
  offerAmount: 1500000.99,
  offerNote: 'MY OFFER NOTES',
  rowVersion: 0,
});

export const mockDispositionFileSaleApi = (
  id = 0,
  dispositionFileId = 1,
): ApiGen_Concepts_DispositionFileSale => ({
  id: id,
  dispositionFileId: dispositionFileId,
  finalConditionRemovalDate: '2022-01-30T00:00:00',
  saleCompletionDate: '2024-01-30T00:00:00',
  saleFiscalYear: '2023',
  finalSaleAmount: 746325.23,
  realtorCommissionAmount: 12500.27,
  isGstRequired: true,
  gstCollectedAmount: 36489.36,
  netBookAmount: 246.2,
  totalCostAmount: 856320.36,
  sppAmount: 1000.0,
  remediationAmount: 1.0,
  purchaserAgentId: 100,
  dispositionPurchaserAgent: {
    id: 100,
    personId: 1000,
    person: {
      ...getEmptyPerson(),
      id: 1000,
      isDisabled: false,
      surname: 'DOE',
      firstName: 'JOHN',
      middleNames: '',
      preferredName: 'Johny Boy',
      personOrganizations: [],
      personAddresses: [],
      contactMethods: [],
      comment: '',
      rowVersion: 1,
    },
    organizationId: null,
    organization: null,
    primaryContactId: null,
    primaryContact: null,
    rowVersion: 1,
  },
  dispositionPurchasers: [
    {
      id: 2,
      dispositionSaleId: 1,
      personId: 12,
      person: {
        ...getEmptyPerson(),
        id: 12,
        isDisabled: false,
        surname: 'Cheese',
        firstName: 'Stinky',
        middleNames: '',
        preferredName: 'Cheesy',
        personOrganizations: [],
        personAddresses: [],
        contactMethods: [],
        comment: '',
        rowVersion: 1,
      },
      organizationId: null,
      organization: null,
      primaryContactId: null,
      primaryContact: null,
      rowVersion: 1,
    },
    {
      id: 5,
      dispositionSaleId: 1,
      personId: 15,
      person: {
        ...getEmptyPerson(),
        id: 15,
        isDisabled: false,
        surname: 'Sanchez',
        firstName: 'Alejandro',
        middleNames: null,
        preferredName: '',
        personOrganizations: [],
        personAddresses: [],
        contactMethods: [],
        comment: '',
        rowVersion: 1,
      },
      organizationId: null,
      organization: null,
      primaryContactId: null,
      primaryContact: null,
      rowVersion: 1,
    },
    {
      id: 12,
      dispositionSaleId: 1,
      personId: null,
      person: null,
      organizationId: 1,
      organization: {
        ...getEmptyOrganization(),
        id: 1,
        isDisabled: false,
        name: 'Ministry of Transportation and Infrastructure',
        alias: 'MOTI',
        incorporationNumber: '1234',
        organizationPersons: [],
        organizationAddresses: [],
        contactMethods: [],
        comment: 'This is a test comment',
        rowVersion: 2,
      },
      primaryContactId: 16,
      primaryContact: {
        ...getEmptyPerson(),
        id: 16,
        isDisabled: false,
        surname: 'Rodriguez',
        firstName: 'Manuel',
        middleNames: '',
        preferredName: '',
        personOrganizations: [],
        personAddresses: [],
        contactMethods: [],
        comment: '',
        rowVersion: 1,
      },
      rowVersion: 1,
    },
  ],
  purchaserSolicitorId: 101,
  dispositionPurchaserSolicitor: {
    id: 101,
    personId: 1001,
    person: {
      ...getEmptyPerson(),
      id: 1001,
      isDisabled: false,
      surname: 'DOE',
      firstName: 'JANE',
      middleNames: '',
      preferredName: 'JANEY',
      personOrganizations: [],
      personAddresses: [],
      contactMethods: [],
      comment: '',
      rowVersion: 1,
    },
    organizationId: null,
    organization: null,
    primaryContactId: null,
    primaryContact: null,
    rowVersion: 1,
  },
  rowVersion: 1,
});

export const mockDispositionAppraisalApi = (
  id = 10,
  dispositionFileId = 1,
): ApiGen_Concepts_DispositionFileAppraisal => ({
  id: id,
  dispositionFileId: dispositionFileId,
  appraisedAmount: 20000.0,
  appraisalDate: '2024-01-18',
  bcaValueAmount: 350000.0,
  bcaRollYear: '2024',
  listPriceAmount: 500000.0,
  rowVersion: 1,
});

export const mockDispositionSaleApi = (
  id = 10,
  dispositionFileId = 1,
): ApiGen_Concepts_DispositionFileSale => ({
  id: id,
  dispositionFileId: dispositionFileId,
  finalConditionRemovalDate: '2024-01-26',
  saleCompletionDate: '2024-01-27',
  saleFiscalYear: '2023',
  finalSaleAmount: 2500000.0,
  realtorCommissionAmount: 1000.0,
  isGstRequired: true,
  gstCollectedAmount: 125000.0,
  netBookAmount: 2000.0,
  totalCostAmount: 3000.0,
  sppAmount: 4000.0,
  remediationAmount: 5000.0,
  dispositionPurchasers: [
    {
      id: 200,
      dispositionSaleId: id,
      personId: null,
      person: null,
      organizationId: 2,
      organization: {
        ...getEmptyOrganization(),
        id: 2,
        isDisabled: false,
        incorporationNumber: '123456',
        name: 'French Mouse Property Management',
        alias: 'FMPM',
        organizationPersons: [],
        organizationAddresses: [],
        contactMethods: [],
        comment: null,
        rowVersion: 1,
      },
      primaryContactId: 2,
      primaryContact: {
        ...getEmptyPerson(),
        id: 2,
        isDisabled: false,
        surname: 'Wilson',
        firstName: 'Volley',
        middleNames: 'Ball',
        preferredName: 'WILLY',
        personOrganizations: [],
        personAddresses: [],
        contactMethods: [],
        comment: null,
        rowVersion: 1,
      },
      rowVersion: 5,
    },
    {
      id: 201,
      dispositionSaleId: id,
      personId: null,
      person: null,
      organizationId: 3,
      organization: {
        ...getEmptyOrganization(),
        id: 3,
        isDisabled: false,
        incorporationNumber: '123456',
        name: 'Dairy Queen Forever! Property Management',
        alias: 'DQ',
        organizationPersons: [],
        organizationAddresses: [],
        contactMethods: [],
        comment: null,
        rowVersion: 1,
      },
      primaryContactId: 3,
      primaryContact: {
        ...getEmptyPerson(),
        id: 3,
        isDisabled: false,
        surname: 'Cheese',
        firstName: 'Stinky',
        middleNames: '',
        preferredName: 'Roquefort',
        personOrganizations: [],
        personAddresses: [],
        contactMethods: [],
        comment: null,
        rowVersion: 1,
      },
      rowVersion: 4,
    },
    {
      id: 202,
      dispositionSaleId: id,
      personId: 15,
      person: {
        ...getEmptyPerson(),
        id: 15,
        isDisabled: false,
        surname: 'Sanchez',
        middleNames: null,
        firstName: 'Alejandro',
        preferredName: 'Alex',
        personOrganizations: [],
        personAddresses: [],
        contactMethods: [],
        comment: null,
        rowVersion: 1,
      },
      organizationId: null,
      organization: null,
      primaryContactId: null,
      primaryContact: null,
      rowVersion: 4,
    },
  ],
  purchaserAgentId: 300,
  dispositionPurchaserAgent: {
    id: 300,
    personId: null,
    person: null,
    organizationId: 3,
    organization: {
      ...getEmptyOrganization(),
      id: 3,
      isDisabled: false,
      incorporationNumber: '123456',
      name: 'Dairy Queen Forever! Property Management',
      alias: 'DQ',
      organizationPersons: [],
      organizationAddresses: [],
      contactMethods: [],
      comment: null,
      rowVersion: 1,
    },
    primaryContactId: 3,
    primaryContact: {
      ...getEmptyPerson(),
      id: 3,
      isDisabled: false,
      surname: 'Cheese',
      firstName: 'Stinky',
      preferredName: 'Roquerfort',
      middleNames: '',
      personOrganizations: [],
      personAddresses: [],
      contactMethods: [],
      comment: null,
      rowVersion: 1,
    },
    rowVersion: 1,
  },
  purchaserSolicitorId: 21,
  dispositionPurchaserSolicitor: {
    id: 21,
    personId: null,
    person: null,
    organizationId: 2,
    organization: {
      ...getEmptyOrganization(),
      id: 2,
      isDisabled: false,
      incorporationNumber: '5678',
      name: 'French Mouse Property Management',
      alias: 'FMPM',
      organizationPersons: [],
      organizationAddresses: [],
      contactMethods: [],
      comment: null,
      rowVersion: 1,
    },
    primaryContactId: 2,
    primaryContact: {
      ...getEmptyPerson(),
      id: 2,
      isDisabled: false,
      surname: 'Wilson',
      firstName: 'Volley',
      middleNames: 'Ball',
      preferredName: 'Round',
      personOrganizations: [],
      personAddresses: [],
      contactMethods: [],
      comment: null,
      rowVersion: 1,
    },
    rowVersion: 2,
  },
  rowVersion: 1,
});

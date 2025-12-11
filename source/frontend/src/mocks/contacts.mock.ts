import { ApiGen_Concepts_ContactMethod } from '@/models/api/generated/ApiGen_Concepts_ContactMethod';
import { ApiGen_Concepts_ContactSummary } from '@/models/api/generated/ApiGen_Concepts_ContactSummary';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';

import { getEmptyOrganization } from './organization.mock';

export const getMockContactOrganizationWithOnePerson = (): ApiGen_Concepts_ContactSummary => ({
  ...getEmptyContactSummary(),
  id: 'O3',
  organizationId: 3,
  organization: {
    ...getEmptyOrganization(),
    id: 3,
    isDisabled: false,
    name: 'Dairy Queen Forever! Property Management',
    alias: 'DQ',
    incorporationNumber: '56789',
    organizationPersons: [
      {
        id: 1,
        personId: 3,
        person: null,
        organizationId: 3,
        organization: null,
        rowVersion: 1,
      },
    ],
    organizationAddresses: [
      {
        id: 4,
        organizationId: 1,
        address: {
          id: 3,
          streetAddress1: '1012 Douglas',
          streetAddress2: 'Above Freshi',
          streetAddress3: 'PO BOX 456',
          municipality: 'Victoria',
          province: {
            id: 1,
            code: 'BC',
            description: 'British Columbia',
            displayOrder: 10,
          },
          country: {
            id: 1,
            code: 'CA',
            description: 'Canada',
            displayOrder: 1,
          },
          postal: 'V9B 000',

          rowVersion: 1,
          comment: null,
          countryId: null,
          countryOther: null,
          district: null,
          latitude: null,
          longitude: null,
          provinceStateId: null,
          region: null,
          districtCode: null,
          regionCode: null,
        },
        addressUsageType: {
          id: 'MAILING',
          description: 'Mailing address',
          isDisabled: false,
          displayOrder: null,
        },
        rowVersion: 1,
      },
    ],
    contactMethods: [],
    rowVersion: 1,
    comment: null,
  },
  summary: 'Dairy Queen Forever! Property Management',
  organizationName: 'Dairy Queen Forever! Property Management',
  isDisabled: false,
});

export const getMockContactOrganizationWithMultiplePeople = (): ApiGen_Concepts_ContactSummary => ({
  ...getEmptyContactSummary(),
  id: 'O2',
  organizationId: 2,
  organization: {
    ...getEmptyOrganization(),
    id: 2,
    isDisabled: false,
    name: 'French Mouse Property Management',
    organizationPersons: [
      {
        id: 1,
        personId: 1,
        person: null,
        organizationId: 2,
        organization: null,
        rowVersion: 1,
      },
      {
        id: 2,
        personId: 3,
        person: null,
        organizationId: 2,
        organization: null,
        rowVersion: 1,
      },
    ],
    organizationAddresses: [],
    contactMethods: [],
    rowVersion: 1,
    alias: null,
    comment: null,
    incorporationNumber: null,
  },
  summary: 'French Mouse Property Management',
  organizationName: 'French Mouse Property Management',
  isDisabled: false,
});

export const getMockContactPerson = (): ApiGen_Concepts_ContactSummary => ({
  ...getEmptyContactSummary(),
  id: 'P1',
  personId: 1,
  person: { ...getEmptyPerson(), firstName: 'test', surname: 'person' },
});

export const getMockPerson = ({
  id,
  surname,
  firstName,
}: {
  id: number;
  surname: string;
  firstName: string;
}): ApiGen_Concepts_Person => ({
  ...getEmptyPerson(),
  id: id,
  isDisabled: false,
  surname: surname,
  firstName: firstName,
  middleNames: '',
  comment: null,
  preferredName: 'Preferred',
  personOrganizations: [
    {
      person: null,
      personId: 3,
      organization: {
        ...getEmptyOrganization(),
        id: 3,
        isDisabled: false,
        name: 'Dairy Queen Forever! Property Management',
        organizationPersons: [
          {
            id: 1,
            personId: 3,
            person: null,
            organizationId: 3,
            organization: null,
            rowVersion: 1,
          },
        ],
        organizationAddresses: [],
        contactMethods: [],
        rowVersion: 1,
        alias: null,
        comment: null,
        incorporationNumber: null,
      },
      rowVersion: 1,
      id: 0,
      organizationId: null,
    },
  ],
  personAddresses: [
    {
      id: 3,
      personId: 2,
      address: {
        id: 3,
        streetAddress1: '123 Main Street',
        streetAddress2: 'PO Box 123',
        streetAddress3: 'Next to the Dairy Queen',
        municipality: 'East Podunk',
        province: {
          id: 1,
          code: 'BC',
          description: 'British Columbia',
          displayOrder: 10,
        },
        country: {
          id: 1,
          code: 'CA',
          description: 'Canada',
          displayOrder: 1,
        },
        postal: 'I4M B0B',
        rowVersion: 1,
        comment: null,
        countryId: null,
        countryOther: null,
        district: null,
        latitude: null,
        longitude: null,
        provinceStateId: null,
        region: null,
        regionCode: null,
        districtCode: null,
      },
      addressUsageType: {
        id: 'MAILADDR',
        description: 'Mailing address',
        isDisabled: true,
        displayOrder: null,
      },
      rowVersion: 1,
    },
    {
      id: 4,
      personId: 2,
      address: {
        id: 3,
        streetAddress1: '123 Main Street',
        streetAddress2: 'PO Box 456',
        streetAddress3: 'Across Dairy Queen',
        municipality: 'West Podunk',
        province: {
          id: 1,
          code: 'BC',
          description: 'British Columbia',
          displayOrder: 10,
        },
        country: {
          id: 1,
          code: 'CA',
          description: 'Canada',
          displayOrder: 1,
        },
        postal: 'V9B B0B',
        rowVersion: 1,
        comment: null,
        countryId: null,
        countryOther: null,
        district: null,
        latitude: null,
        longitude: null,
        provinceStateId: null,
        region: null,
        districtCode: null,
        regionCode: null,
      },
      addressUsageType: {
        id: 'MAILING',
        description: 'Mailing address',
        isDisabled: false,
        displayOrder: null,
      },
      rowVersion: 1,
    },
  ],
  contactMethods: [
    {
      id: 3,
      contactMethodType: {
        id: 'PERSPHONE',
        description: 'Personal phone',
        isDisabled: false,
        displayOrder: null,
      },
      personId: 2,
      organizationId: null,
      value: '6049983251',
      rowVersion: 1,
    },
  ],
  rowVersion: 1,
});

export const getEmptyContactSummary = (): ApiGen_Concepts_ContactSummary => {
  return {
    id: null,
    personId: null,
    person: null,
    organizationId: null,
    organization: null,
    summary: null,
    surname: null,
    firstName: null,
    middleNames: null,
    organizationName: null,
    email: null,
    mailingAddress: null,
    municipalityName: null,
    provinceState: null,
    isDisabled: false,
  };
};

export const getEmptyPerson = (): ApiGen_Concepts_Person => {
  return {
    id: 0,
    surname: null,
    firstName: null,
    middleNames: null,
    nameSuffix: null,
    preferredName: null,
    birthDate: null,
    comment: null,
    addressComment: null,
    useOrganizationAddress: null,
    isDisabled: false,
    managementActivityId: null,
    contactMethods: null,
    personAddresses: null,
    personOrganizations: null,
    rowVersion: null,
  };
};

export const getEmptyContactMethod = (): ApiGen_Concepts_ContactMethod => {
  return {
    id: 0,
    contactMethodType: null,
    value: null,
    rowVersion: null,
    personId: null,
    organizationId: null,
  };
};

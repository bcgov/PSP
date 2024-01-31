import { IContactSearchResult } from '@/interfaces/IContactSearchResult';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';

export const getMockContactOrganizationWithOnePerson = (): IContactSearchResult => ({
  id: 'O3',
  organizationId: 3,
  organization: {
    id: 3,
    isDisabled: false,
    name: 'Dairy Queen Forever! Property Management',
    alias: 'DQ',
    incorporationNumber: '56789',
    organizationPersons: [
      {
        personId: 3,
        person: null,
        organizationId: 3,
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

export const getMockContactOrganizationWithMultiplePeople = (): IContactSearchResult => ({
  id: 'O2',
  organizationId: 2,
  organization: {
    id: 2,
    isDisabled: false,
    name: 'French Mouse Property Management',
    organizationPersons: [
      {
        personId: 1,
        person: null,
        organizationId: 2,
        rowVersion: 1,
      },
      {
        personId: 3,
        person: null,
        organizationId: 2,
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

export const getMockContactPerson = (): IContactSearchResult => ({
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
  id: id,
  isDisabled: false,
  surname: surname,
  firstName: firstName,
  middleNames: '',
  comment: null,
  preferredName: null,
  personOrganizations: [
    {
      personId: 3,
      organization: {
        id: 3,
        isDisabled: false,
        name: 'Dairy Queen Forever! Property Management',
        organizationPersons: [
          {
            personId: 3,
            person: null,
            organizationId: 3,
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
      value: '6049983251',
      rowVersion: 1,
    },
  ],
  rowVersion: 1,
});

export const getEmptyPerson = (): ApiGen_Concepts_Person => {
  return {
    id: 0,
    isDisabled: false,
    surname: null,
    firstName: null,
    middleNames: null,
    preferredName: null,
    personOrganizations: null,
    personAddresses: null,
    contactMethods: null,
    comment: null,
    rowVersion: null,
  };
};

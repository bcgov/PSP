import { IContactSearchResult } from 'interfaces/IContactSearchResult';
export const getMockContactOrganizationWithOnePerson = (): IContactSearchResult => ({
  id: 'O3',
  organizationId: 3,
  organization: {
    id: 3,
    isDisabled: false,
    name: 'Dairy Queen Forever! Property Management',
    organizationPersons: [
      {
        personId: 3,
        organizationId: 3,
        isDisabled: false,
        rowVersion: 1,
      },
    ],
    organizationAddresses: [],
    contactMethods: [],
    rowVersion: 1,
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
        organizationId: 2,
        isDisabled: false,
        rowVersion: 1,
      },
      {
        personId: 3,
        organizationId: 2,
        isDisabled: false,
        rowVersion: 1,
      },
    ],
    organizationAddresses: [],
    contactMethods: [],
    rowVersion: 1,
  },
  summary: 'French Mouse Property Management',
  organizationName: 'French Mouse Property Management',
  isDisabled: false,
});

export const getMockContactPerson = () => ({
  id: 'P1',
  personId: 1,
  person: { firstName: 'test', surname: 'person' },
});

export const getMockPerson = ({
  id,
  surname,
  firstName,
}: {
  id: number;
  surname: string;
  firstName: string;
}) => ({
  id: id,
  isDisabled: false,
  surname: surname,
  firstName: firstName,
  middleNames: '',
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
            organizationId: 3,
            isDisabled: false,
            rowVersion: 1,
          },
        ],
        organizationAddresses: [],
        contactMethods: [],
        rowVersion: 1,
      },
      isDisabled: false,
      rowVersion: 1,
    },
  ],
  personAddresses: [
    {
      id: 3,
      isDisabled: false,
      personId: 3,
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
      },
      addressUsageType: {
        id: 'MAILADDR',
        description: 'Mailing address',
        isDisabled: true,
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
      },
      value: '6049983251',
      rowVersion: 1,
    },
  ],
  rowVersion: 1,
});

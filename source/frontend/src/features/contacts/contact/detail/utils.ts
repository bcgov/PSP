import { AddressTypes } from '@/constants/addressTypes';
import { ContactInfoField } from '@/features/contacts/interfaces';
import { Dictionary } from '@/interfaces/Dictionary';
import { IContactAddress, IContactMethod } from '@/interfaces/IContact';

// the order of this array corresponds to the expected display order
const addressSortOrder = [AddressTypes.Mailing, AddressTypes.Residential, AddressTypes.Billing];

export const sortAddresses = (a1: IContactAddress, a2: IContactAddress) => {
  if (a2.addressType.id === a1.addressType.id) {
    return 0;
  }
  const a2Index = addressSortOrder.indexOf(a2.addressType.id as AddressTypes);
  if (a2Index !== -1 && a2Index < addressSortOrder.indexOf(a1.addressType.id as AddressTypes)) {
    return 1;
  }

  return -1;
};

export function getContactInfo(
  validTypes: Dictionary<string>,
  contactMethods?: IContactMethod[],
): ContactInfoField[] {
  if (contactMethods === undefined) {
    return [];
  }
  // Get only the valid types
  let filteredFields = contactMethods.reduce(
    (accumulator: ContactInfoField[], method: IContactMethod) => {
      if (Object.keys(validTypes).includes(method.contactMethodType.id)) {
        accumulator.push({
          info: method.value,
          label: validTypes[method.contactMethodType.id],
        });
      }
      return accumulator;
    },
    [],
  );

  // Sort according to the dictionary order
  return filteredFields.sort((a, b) => {
    return Object.values(validTypes).indexOf(a.label) - Object.values(validTypes).indexOf(b.label);
  });
}

export const fakeAddresses = [
  {
    id: 1,
    rowVersion: 3,
    addressType: {
      id: 'BILLING',
      description: 'Billing address',
      isDisabled: false,
    },
    streetAddress1: 'Billing address',
    municipality: 'Hollywood North',
    province: {
      provinceStateId: 1,
      provinceStateCode: 'BC',
      description: 'British Columbia',
    },
    country: {
      countryId: 1,
      countryCode: 'CA',
      description: 'Canada',
    },
    postal: 'V6A 5G7',
  },
  {
    id: 1,
    rowVersion: 3,
    addressType: {
      id: 'MAILING',
      description: 'Mailing address',
      isDisabled: false,
    },
    streetAddress1: 'Mailing address',
    streetAddress2: 'Living in a van',
    streetAddress3: 'Down by the River',
    municipality: 'Hollywood North',
    province: {
      provinceStateId: 1,
      provinceStateCode: 'BC',
      description: 'British Columbia',
    },
    country: {
      countryId: 1,
      countryCode: 'CA',
      description: 'Canada',
    },
    postal: 'V6Z 5G7',
  },
  {
    id: 5,
    rowVersion: 1,
    addressType: {
      id: 'RESIDNT',
      description: 'Property address',
      isDisabled: false,
    },
    streetAddress1: 'Property address',
    streetAddress2: '',
    streetAddress3: '',
    municipality: 'vic',
    province: {
      provinceStateId: 1,
      provinceStateCode: 'BC',
      description: 'British Columbia',
    },
    country: {
      countryId: 1,
      countryCode: 'CA',
      description: 'Canada',
    },
    postal: 'V9A 0H6',
  },
];

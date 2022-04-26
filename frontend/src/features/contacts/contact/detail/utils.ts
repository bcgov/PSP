import { IContactAddress } from 'interfaces/IContact';

export const sortAddresses = (a1: IContactAddress, a2: IContactAddress) => {
  if (a2.addressType.id === a1.addressType.id) {
    return 0;
  }

  if (a2.addressType.id === 'MAILING') {
    return 1;
  }

  if (a2.addressType.id === 'RESIDNT' && a1.addressType.id !== 'MAILING') {
    return 1;
  }
  return -1;
};

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

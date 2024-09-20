import { ContactInfoField } from '@/features/contacts/interfaces';
import { Dictionary } from '@/interfaces/Dictionary';
import { ApiGen_CodeTypes_AddressUsageTypes } from '@/models/api/generated/ApiGen_CodeTypes_AddressUsageTypes';
import { ApiGen_Concepts_ContactMethod } from '@/models/api/generated/ApiGen_Concepts_ContactMethod';
import { ApiGen_Concepts_OrganizationAddress } from '@/models/api/generated/ApiGen_Concepts_OrganizationAddress';
import { ApiGen_Concepts_PersonAddress } from '@/models/api/generated/ApiGen_Concepts_PersonAddress';
import { exists } from '@/utils';

// the order of this array corresponds to the expected display order
const addressSortOrder = [
  ApiGen_CodeTypes_AddressUsageTypes.MAILING,
  ApiGen_CodeTypes_AddressUsageTypes.RESIDNT,
  ApiGen_CodeTypes_AddressUsageTypes.BILLING,
];

export const sortAddresses = (
  a1: ApiGen_Concepts_PersonAddress | ApiGen_Concepts_OrganizationAddress,
  a2: ApiGen_Concepts_PersonAddress | ApiGen_Concepts_OrganizationAddress,
) => {
  if (a2?.addressUsageType?.id === a1?.addressUsageType?.id) {
    return 0;
  }
  const a2Index = addressSortOrder.indexOf(
    a2?.addressUsageType?.id as ApiGen_CodeTypes_AddressUsageTypes,
  );
  if (
    a2Index !== -1 &&
    a2Index <
      addressSortOrder.indexOf(a1?.addressUsageType?.id as ApiGen_CodeTypes_AddressUsageTypes)
  ) {
    return 1;
  }

  return -1;
};

export function getContactInfo(
  validTypes: Dictionary<string>,
  contactMethods?: ApiGen_Concepts_ContactMethod[],
): ContactInfoField[] {
  if (contactMethods === undefined) {
    return [];
  }
  // Get only the valid types
  const filteredFields = contactMethods.reduce(
    (accumulator: ContactInfoField[], method: ApiGen_Concepts_ContactMethod) => {
      if (
        exists(method.contactMethodType?.id) &&
        Object.keys(validTypes).includes(method.contactMethodType!.id)
      ) {
        accumulator.push({
          info: method.value,
          label: validTypes[method.contactMethodType!.id],
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

export const fakeAddresses: ApiGen_Concepts_PersonAddress[] = [
  {
    id: 1,
    personId: 1,
    rowVersion: 3,
    addressUsageType: {
      id: 'BILLING',
      description: 'Billing address',
      isDisabled: false,
      displayOrder: null,
    },
    address: {
      id: 1,
      districtCode: 1,
      district: {
        id: 1,
        code: 'GVRD',
        description: 'Greater Vancouver Regional District',
        displayOrder: 1,
      },
      regionCode: 1,
      region: { id: 1, code: 'LM', description: 'Lower Mainland', displayOrder: 1 },
      streetAddress1: 'Billing address',
      streetAddress2: '',
      streetAddress3: '',
      municipality: 'Hollywood North',
      provinceStateId: 1,
      province: {
        id: 1,
        code: 'BC',
        description: 'British Columbia',
        displayOrder: 1,
      },
      countryId: 1,
      country: {
        id: 1,
        code: 'CA',
        description: 'Canada',
        displayOrder: 1,
      },
      postal: 'V6A 5G7',
      latitude: 49.2827,
      longitude: -123.1207,
      countryOther: '',
      comment: '',
      rowVersion: 1,
    },
  },
  {
    id: 1,
    personId: 1,
    rowVersion: 3,
    addressUsageType: {
      id: 'MAILING',
      description: 'Mailing address',
      isDisabled: false,
      displayOrder: null,
    },
    address: {
      id: 1,
      districtCode: 1,
      district: {
        id: 1,
        code: 'GVRD',
        description: 'Greater Vancouver Regional District',
        displayOrder: 1,
      },
      regionCode: 1,
      region: { id: 1, code: 'LM', description: 'Lower Mainland', displayOrder: 1 },
      streetAddress1: 'Mailing address',
      streetAddress2: 'Living in a van',
      streetAddress3: 'Down by the River',
      municipality: 'Hollywood North',
      provinceStateId: 1,
      province: {
        id: 1,
        code: 'BC',
        description: 'British Columbia',
        displayOrder: 1,
      },
      countryId: 1,
      country: {
        id: 1,
        code: 'CA',
        description: 'Canada',
        displayOrder: 1,
      },
      postal: 'V6A 5G7',
      latitude: 49.2827,
      longitude: -123.1207,
      countryOther: '',
      comment: '',
      rowVersion: 1,
    },
  },
  {
    personId: 1,
    id: 5,
    rowVersion: 1,
    addressUsageType: {
      id: 'RESIDNT',
      description: 'Property address',
      isDisabled: false,
      displayOrder: null,
    },
    address: {
      id: 1,
      districtCode: 1,
      district: {
        id: 1,
        code: 'GVRD',
        description: 'Greater Vancouver Regional District',
        displayOrder: 1,
      },
      regionCode: 1,
      region: { id: 1, code: 'LM', description: 'Lower Mainland', displayOrder: 1 },
      streetAddress1: 'Property address',
      streetAddress2: 'Living in a van',
      streetAddress3: 'Down by the River',
      municipality: 'vic',
      provinceStateId: 1,
      province: {
        id: 1,
        code: 'BC',
        description: 'British Columbia',
        displayOrder: 1,
      },
      countryId: 1,
      country: {
        id: 1,
        code: 'CA',
        description: 'Canada',
        displayOrder: 1,
      },
      postal: 'V9A 0H6',
      latitude: 49.2827,
      longitude: -123.1207,
      countryOther: '',
      comment: '',
      rowVersion: 1,
    },
  },
];

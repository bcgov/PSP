import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';

export const getEmptyAddress: () => ApiGen_Concepts_Address = () => ({
  id: null,
  streetAddress1: null,
  streetAddress2: null,
  streetAddress3: null,
  municipality: null,
  provinceStateId: null,
  province: null,
  countryId: null,
  country: null,
  district: null,
  region: null,
  countryOther: null,
  postal: null,
  latitude: null,
  longitude: null,
  comment: null,
  rowVersion: null,
});

export const getMockApiAddress: () => ApiGen_Concepts_Address = () => ({
  ...getEmptyAddress(),
  id: 1,
  streetAddress1: '1234 mock Street',
  streetAddress2: 'N/A',
  municipality: 'Victoria',
  province: {
    id: 1,
    code: 'BC',
    description: 'British Columbia',
    displayOrder: null,
  },
  postal: 'V1V1V1',
  country: { id: 1, code: 'CA', description: 'Canada', displayOrder: null },
});

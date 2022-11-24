import { Api_Address } from 'models/api/Address';

export const getMockApiAddress: () => Api_Address = () => ({
  id: 1,
  streetAddress1: '1234 mock Street',
  streetAddress2: 'N/A',
  municipality: 'Victoria',
  province: { id: 1, description: 'British Columbia' },
  postal: 'V1V1V1',
});

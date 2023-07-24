import { getMockApiAddress } from '@/mocks/address.mock';

import { Api_GenerateAddress } from './GenerateAddress';

describe('GenerateAddress tests', () => {
  it('Can Generate an empty address without throwing an error', () => {
    const address = new Api_GenerateAddress(null);
    expect(address.address_string).toBe('');
  });

  it('Can Generate an address string in the expected format', () => {
    const address = new Api_GenerateAddress(getMockApiAddress());
    expect(address.address_string).toBe(`1234 mock Street
N/A
Victoria
British Columbia
V1V1V1
Canada`);
  });
});

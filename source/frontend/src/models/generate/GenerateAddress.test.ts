import { getMockApiAddress } from 'mocks/mockAddress';

import { GenerateAddress } from './GenerateAddress';

describe('GenerateAddress tests', () => {
  it('Can Generate an empty address without throwing an error', () => {
    const address = new GenerateAddress(null);
    expect(address.address_string).toBe('');
  });
  it('Can Generate an address string in the expected format', () => {
    const address = new GenerateAddress(getMockApiAddress());
    expect(address.address_string).toBe(`1234 mock Street
N/A
Victoria
British Columbia
V1V1V1`);
  });
});

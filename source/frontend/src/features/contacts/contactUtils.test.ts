import { formAddressToApiAddress } from './contactUtils';

describe('contact utilities', () => {
  it('saves a province if set to an int value', () => {
    const actualApiAddress = formAddressToApiAddress({
      provinceId: 1,
      streetAddress1: '',
      countryId: 1,
      addressTypeId: '',
    });

    expect(actualApiAddress.provinceId).toBe(1);
  });
});

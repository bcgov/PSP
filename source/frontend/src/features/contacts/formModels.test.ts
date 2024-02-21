import { AddressTypes } from '@/constants';

import { IEditablePersonAddressForm } from './formModels';

describe('contact utilities', () => {
  it('saves a province if set to an int value', () => {
    const testForm = new IEditablePersonAddressForm(AddressTypes.Mailing);
    testForm.provinceId = 1;
    testForm.streetAddress1 = '';
    testForm.countryId = 1;
    testForm.addressTypeId = '';
    const actualApiAddress = testForm.formAddressToApiAddress();

    expect(actualApiAddress.address?.provinceStateId).toBe(1);
  });
});

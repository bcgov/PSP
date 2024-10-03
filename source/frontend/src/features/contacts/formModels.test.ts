import { IEditablePersonAddressForm } from './formModels';
import { ApiGen_CodeTypes_AddressUsageTypes } from '@/models/api/generated/ApiGen_CodeTypes_AddressUsageTypes';

describe('contact utilities', () => {
  it('saves a province if set to an int value', () => {
    const testForm = new IEditablePersonAddressForm(ApiGen_CodeTypes_AddressUsageTypes.MAILING);
    testForm.provinceId = 1;
    testForm.streetAddress1 = '';
    testForm.countryId = 1;
    testForm.addressTypeId = '';
    const actualApiAddress = testForm.formAddressToApiAddress();

    expect(actualApiAddress.address?.provinceStateId).toBe(1);
  });
});

import { IEditableOrganizationAddress } from '@/interfaces/editable-contact';
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

  it('Can create a model from IEditableOrganizationAddress', () => {
    const conceptModel = {
      id: 1,
      addressTypeId: {
        id: 'MAILING',
        description: 'MAILING ADDRESS',
        isDisabled: false,
        displayOrder: 100,
      },
      streetAddress1: '1012 Douglas',
      streetAddress2: 'LINE2',
      streetAddress3: '',
      municipality: 'Victoria',
      regionId: 10,
      districtId: 20,
      provinceId: 30,
      province: 'British Columbia',
      countryId: 40,
      country: 'Canada',
      countryOther: undefined,
      postal: 'V1V1V1',
      rowVersion: 1,
      organizationId: 1,
      organizationAddressId: 1,
      organizationAddressRowVersion: 1,
    } as IEditableOrganizationAddress;

    const model = Api_GenerateAddress.fromIEditableOrgAddress(conceptModel);

    expect(model.line_1).toBe('1012 Douglas');
    expect(model.line_2).toBe('LINE2');
    expect(model.line_3).toBe('');
    expect(model.city).toBe('Victoria');
    expect(model.province).toBe('British Columbia');
    expect(model.postal).toBe('V1V1V1');
    expect(model.country).toBe('Canada');
    expect(model.address_string).toBe(`1012 Douglas
LINE2
Victoria
British Columbia
V1V1V1
Canada`);
  });
});

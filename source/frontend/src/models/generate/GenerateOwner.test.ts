import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { getMockContactOrganizationWithOnePerson, getMockPerson } from '@/mocks/contacts.mock';

import { Api_GenerateOwner } from './GenerateOwner';

describe('GenerateOwner tests', () => {
  it('Can Generate an empty owner without throwing an error', () => {
    const owner = new Api_GenerateOwner(null);
    expect(owner.owner_string).toBe('');
  });

  it('Can Generate an owner person string in the expected format', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const owner = new Api_GenerateOwner(
      acqFile?.acquisitionFileOwners ? acqFile?.acquisitionFileOwners[0] : null,
    );
    expect(owner.owner_string).toBe(`John Doe (Jr.)`);
  });

  it('Can Generate an owner organization string in the expected format', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const owner = new Api_GenerateOwner(
      acqFile?.acquisitionFileOwners ? acqFile?.acquisitionFileOwners[1] : null,
    );
    expect(owner.owner_string).toBe(`FORTIS BC, Inc. No. 9999 (OR Reg. No. 12345)`);
  });

  it('Can Generate an owner phone number', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const owner = new Api_GenerateOwner(
      acqFile?.acquisitionFileOwners ? acqFile?.acquisitionFileOwners[1] : null,
    );
    expect(owner.phone).toBe(`775-111-1111`);
  });

  it('Can generate owner from Api Person', () => {
    const mockConceptModel = getMockPerson({ id: 1, surname: 'last', firstName: 'first' });

    const model = Api_GenerateOwner.fromApiPerson(mockConceptModel);

    expect(model.given_name).toBe('first');
    expect(model.last_name_or_corp_name).toBe('last');
    expect(model.other_name).toBe('');
    expect(model.incorporation_number).toBe('');
    expect(model.registration_number).toBe('');
    expect(model.is_corporation).toBe(false);
    expect(model.owner_string).toBe('first last');

    expect(model.address.line_1).toBe('123 Main Street');
    expect(model.address.line_2).toBe('PO Box 456');
    expect(model.address.line_3).toBe('Across Dairy Queen');
    expect(model.address.city).toBe('West Podunk');
    expect(model.address.province).toBe('British Columbia');
    expect(model.address.postal).toBe('V9B B0B');
    expect(model.address.country).toBe('Canada');
    expect(model.address.address_string).toBe(`123 Main Street
PO Box 456
Across Dairy Queen
West Podunk
British Columbia
V9B B0B
Canada`);
    expect(model.address.address_single_line_string).toBe(
      `123 Main Street, PO Box 456, Across Dairy Queen, West Podunk, British Columbia, V9B B0B, Canada`,
    );
  });

  it('Can generate owner from Api Organization', () => {
    const mockConceptModel = getMockContactOrganizationWithOnePerson().organization;

    const model = Api_GenerateOwner.fromApiOrganization(mockConceptModel!);

    expect(model.given_name).toBe('');
    expect(model.last_name_or_corp_name).toBe('Dairy Queen Forever! Property Management');
    expect(model.other_name).toBe('DQ');
    expect(model.incorporation_number).toBe('56789');
    expect(model.registration_number).toBe('');
    expect(model.is_corporation).toBe(true);
    expect(model.owner_string).toBe('Dairy Queen Forever! Property Management, Inc. No. 56789');

    expect(model.address.line_1).toBe('1012 Douglas');
    expect(model.address.line_2).toBe('Above Freshi');
    expect(model.address.line_3).toBe('PO BOX 456');
    expect(model.address.city).toBe('Victoria');
    expect(model.address.province).toBe('British Columbia');
    expect(model.address.postal).toBe('V9B 000');
    expect(model.address.country).toBe('Canada');
    expect(model.address.address_string).toBe(`1012 Douglas
Above Freshi
PO BOX 456
Victoria
British Columbia
V9B 000
Canada`);
    expect(model.address.address_single_line_string).toBe(
      `1012 Douglas, Above Freshi, PO BOX 456, Victoria, British Columbia, V9B 000, Canada`,
    );
  });
});

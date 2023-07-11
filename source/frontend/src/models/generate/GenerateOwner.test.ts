import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { getMockPerson } from '@/mocks/contacts.mock';

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
    expect(owner.owner_string).toBe(`John Doe Jr.`);
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
  });
});

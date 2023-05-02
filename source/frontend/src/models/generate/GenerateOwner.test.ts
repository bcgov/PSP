import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';

import { GenerateOwner } from './GenerateOwner';

describe('GenerateOwner tests', () => {
  it('Can Generate an empty owner without throwing an error', () => {
    const owner = new GenerateOwner(null);
    expect(owner.owner_string).toBe('');
  });
  it('Can Generate an owner person string in the expected format', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const owner = new GenerateOwner(
      acqFile?.acquisitionFileOwners ? acqFile?.acquisitionFileOwners[0] : null,
    );
    expect(owner.owner_string).toBe(`John Doe Jr.`);
  });

  it('Can Generate an owner organization string in the expected format', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const owner = new GenerateOwner(
      acqFile?.acquisitionFileOwners ? acqFile?.acquisitionFileOwners[1] : null,
    );
    expect(owner.owner_string).toBe(`FORTIS BC, Inc. No. 9999 (OR Reg. No. 12345)`);
  });

  it('Can Generate an owner phone number', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const owner = new GenerateOwner(
      acqFile?.acquisitionFileOwners ? acqFile?.acquisitionFileOwners[1] : null,
    );
    expect(owner.phone).toBe(`775-111-1111`);
  });
});

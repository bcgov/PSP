import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';

import { Api_GenerateFile } from './GenerateFile';

describe('GenerateOwner tests', () => {
  it('Can Generate an empty owner without throwing an error', () => {
    const file = new Api_GenerateFile(null);
    expect(file.file_name).toBe('');
    expect(file.file_number).toBe('');
  });
  it('Can generate a file with no primary owner', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    acqFile.acquisitionFileOwners = acqFile?.acquisitionFileOwners
      ? [acqFile?.acquisitionFileOwners[1]]
      : [];
    const file = new Api_GenerateFile(acqFile);
    expect(file.primary_owner?.owner_string).toBe('');
  });

  it('Can generate a file with a primary owner', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateFile(acqFile);
    expect(file.primary_owner).not.toBeNull();
    expect(file.primary_owner?.owner_string).toBe('John Doe Jr.');
  });

  it('saves a list of the person owners on the file', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateFile(acqFile);
    expect(file.person_owners).toHaveLength(1);
    expect(file.person_owners[0].owner_string).toBe('John Doe Jr.');
  });

  it('saves a list of the organization owners on the file', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateFile(acqFile);
    expect(file.organization_owners).toHaveLength(1);
    expect(file.organization_owners[0].owner_string).toBe(
      'FORTIS BC, Inc. No. 9999 (OR Reg. No. 12345)',
    );
  });
});

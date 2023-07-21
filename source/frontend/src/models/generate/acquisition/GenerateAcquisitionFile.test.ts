import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { emptyApiInterestHolder, emptyInterestHolderProperty } from '@/mocks/interestHolder.mock';

import { Api_GenerateAcquisitionFile } from './GenerateAcquisitionFile';

describe('GenerateFile tests', () => {
  it('Can Generate an empty file without throwing an error', () => {
    const file = new Api_GenerateAcquisitionFile({
      file: null,
      interestHolders: [],
    });
    expect(file.file_name).toBe('');
    expect(file.file_number).toBe('');
  });
  it('Can generate a file with no primary owner', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    acqFile.acquisitionFileOwners = acqFile?.acquisitionFileOwners
      ? [acqFile?.acquisitionFileOwners[1]]
      : [];
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      interestHolders: [],
    });
    expect(file.primary_owner?.owner_string).toBe('');
  });

  it('Can generate a file with a primary owner', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      interestHolders: [],
    });
    expect(file.primary_owner).not.toBeNull();
    expect(file.primary_owner?.owner_string).toBe('John Doe (Jr.)');
  });

  it('saves a list of the person owners on the file', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      interestHolders: [],
    });
    expect(file.person_owners).toHaveLength(1);
    expect(file.person_owners[0].owner_string).toBe('John Doe (Jr.)');
  });

  it('saves a list of the organization owners on the file', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      interestHolders: [],
    });
    expect(file.organization_owners).toHaveLength(1);
    expect(file.organization_owners[0].owner_string).toBe(
      'FORTIS BC, Inc. No. 9999 (OR Reg. No. 12345)',
    );
  });

  it('saves an interest holder with the associated file properties', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      interestHolders: [
        {
          ...emptyApiInterestHolder,
          interestHolderId: 1,
          acquisitionFileId: acqFile.id ?? null,
          person: { firstName: 'first', middleNames: 'middle', surname: 'last' },
          interestHolderProperties: [
            {
              ...emptyInterestHolderProperty,
              propertyInterestTypes: [{ description: 'interest' }],
              interestHolderId: 1,
              acquisitionFilePropertyId: 1,
            },
          ],
        },
      ],
    });
    expect(file.properties[0].interest_holders_string).toBe('first middle last: interest');
  });

  it('saves multiple interest holders with the associated file properties', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      interestHolders: [
        {
          ...emptyApiInterestHolder,
          interestHolderId: 1,
          acquisitionFileId: acqFile.id ?? null,
          person: { firstName: 'first', middleNames: 'middle', surname: 'last' },
          interestHolderProperties: [
            {
              ...emptyInterestHolderProperty,
              propertyInterestTypes: [{ description: 'interest' }],
              interestHolderId: 1,
              acquisitionFilePropertyId: 1,
            },
          ],
        },
        {
          ...emptyApiInterestHolder,
          interestHolderId: 2,
          acquisitionFileId: acqFile.id ?? null,
          person: { firstName: 'another', middleNames: 'middle', surname: 'person' },
          interestHolderProperties: [
            {
              ...emptyInterestHolderProperty,
              propertyInterestTypes: [{ description: 'interest 2' }],
              interestHolderId: 2,
              acquisitionFilePropertyId: 1,
            },
          ],
        },
      ],
    });
    expect(file.properties[0].interest_holders_string).toBe(
      'first middle last: interest, another middle person: interest 2',
    );
  });

  it('saves a single interest holder with multiple impacted properties with the associated file properties', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      interestHolders: [
        {
          ...emptyApiInterestHolder,
          interestHolderId: 1,
          acquisitionFileId: acqFile.id ?? null,
          person: { firstName: 'first', middleNames: 'middle', surname: 'last' },
          interestHolderProperties: [
            {
              ...emptyInterestHolderProperty,
              propertyInterestTypes: [{ description: 'interest' }],
              interestHolderId: 1,
              acquisitionFilePropertyId: 1,
            },
            {
              ...emptyInterestHolderProperty,
              propertyInterestTypes: [{ description: 'interest 2' }],
              interestHolderId: 1,
              acquisitionFilePropertyId: 1,
            },
          ],
        },
      ],
    });
    expect(file.properties[0].interest_holders_string).toBe(
      'first middle last: interest, first middle last: interest 2',
    );
  });

  it('ingores nip interest holders on an h120', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      interestHolders: [
        {
          ...emptyApiInterestHolder,
          interestHolderId: 1,
          acquisitionFileId: acqFile.id ?? null,
          person: { firstName: 'first', middleNames: 'middle', surname: 'last' },
          interestHolderProperties: [
            {
              ...emptyInterestHolderProperty,
              propertyInterestTypes: [{ description: 'interest', id: 'NIP' }],
              interestHolderId: 1,
              acquisitionFilePropertyId: 1,
            },
          ],
        },
      ],
    });
    expect(file.properties[0].interest_holders_string).toBe('');
  });
});

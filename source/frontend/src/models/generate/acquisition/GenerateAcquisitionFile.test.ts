import { AddressTypes } from '@/constants';
import { ContactMethodTypes } from '@/constants/contactMethodType';
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

  it('generates a file with org neg agent that has org name but primary contacts contact info and address', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      negotiatingAgent: {
        acquisitionFileId: acqFile.id ?? 0,
        organization: {
          name: 'testOrg',
          organizationAddresses: [
            {
              address: { streetAddress1: 'orgaddress' },
              addressUsageType: { id: AddressTypes.Mailing },
            },
          ],
        },
        primaryContact: {
          contactMethods: [
            { contactMethodType: { id: ContactMethodTypes.WorkEmail }, value: 'primaryworkemail' },
            { contactMethodType: { id: ContactMethodTypes.WorkPhone }, value: 'primaryworkphone' },
          ],
          personAddresses: [
            {
              address: { streetAddress1: 'primaryaddress' },
              addressUsageType: { id: AddressTypes.Mailing },
            },
          ],
        },
      },
    });
    expect(file.neg_agent?.full_name_string).toBe('testOrg (Inc. No. )');
    expect(file.neg_agent?.email).toBe('primaryworkemail');
    expect(file.neg_agent?.phone).toBe('primaryworkphone');
    expect(file.neg_agent?.address?.address_string).toBe('primaryaddress');
  });

  it('generates a file with org neg agent that has org name but null contact if primary contact empty', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      negotiatingAgent: {
        acquisitionFileId: acqFile.id ?? 0,
        organization: {
          name: 'testOrg',
          organizationAddresses: [
            {
              address: { streetAddress1: 'orgaddress' },
              addressUsageType: { id: AddressTypes.Mailing },
            },
          ],
        },
      },
    });
    expect(file.neg_agent?.full_name_string).toBe('testOrg (Inc. No. )');
    expect(file.neg_agent?.email).toBe('');
    expect(file.neg_agent?.phone).toBe('');
    expect(file.neg_agent?.address).toBeNull();
  });

  it('generates a file with org coord agent that has org name but primary contacts contact info', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      coordinatorContact: {
        acquisitionFileId: acqFile.id ?? 0,
        organization: {
          name: 'testOrg',
          organizationAddresses: [
            {
              address: { streetAddress1: 'orgaddress' },
              addressUsageType: { id: AddressTypes.Mailing },
            },
          ],
        },
        primaryContact: {
          contactMethods: [
            { contactMethodType: { id: ContactMethodTypes.WorkEmail }, value: 'workemail' },
            { contactMethodType: { id: ContactMethodTypes.WorkPhone }, value: 'workphone' },
          ],
        },
      },
    });
    expect(file.property_coordinator?.full_name_string).toBe('testOrg (Inc. No. )');
    expect(file.property_coordinator?.email).toBe('workemail');
    expect(file.property_coordinator?.phone).toBe('workphone');
    expect(file.property_coordinator?.address?.address_string).toBe('orgaddress');
  });

  it('generates a file with org coord agent that has org name but empty primary contact', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      coordinatorContact: {
        acquisitionFileId: acqFile.id ?? 0,
        organization: {
          name: 'testOrg',
          organizationAddresses: [
            {
              address: { streetAddress1: 'orgaddress' },
              addressUsageType: { id: AddressTypes.Mailing },
            },
          ],
        },
      },
    });
    expect(file.property_coordinator?.full_name_string).toBe('testOrg (Inc. No. )');
    expect(file.property_coordinator?.email).toBe('');
    expect(file.property_coordinator?.phone).toBe('');
    expect(file.property_coordinator?.address?.address_string).toBe('orgaddress');
  });

  it('generates a file with org provincial solicitor that has org address and name, primary contacts info, and separate attn field', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      provincialSolicitor: {
        acquisitionFileId: acqFile.id ?? 0,
        organization: {
          name: 'testOrg',
          organizationAddresses: [
            {
              address: { streetAddress1: 'orgaddress' },
              addressUsageType: { id: AddressTypes.Mailing },
            },
          ],
        },
        primaryContact: {
          firstName: 'testfirst',
          contactMethods: [
            { contactMethodType: { id: ContactMethodTypes.WorkEmail }, value: 'workemail' },
            { contactMethodType: { id: ContactMethodTypes.WorkPhone }, value: 'workphone' },
          ],
        },
      },
    });
    expect(file.prov_solicitor?.full_name_string).toBe('testOrg (Inc. No. )');
    expect(file.prov_solicitor?.email).toBe('workemail');
    expect(file.prov_solicitor?.phone).toBe('workphone');
    expect(file.prov_solicitor_attn?.full_name_string).toBe('testfirst');
    expect(file.prov_solicitor?.address?.address_string).toBe('orgaddress');
  });

  it('generates a file with org provincial solicitor that has org address and name, and empty attn and info if primary contact empty', () => {
    const acqFile = mockAcquisitionFileResponse(1, 'test', 1);
    const file = new Api_GenerateAcquisitionFile({
      file: acqFile,
      provincialSolicitor: {
        acquisitionFileId: acqFile.id ?? 0,
        organization: {
          name: 'testOrg',
          organizationAddresses: [
            {
              address: { streetAddress1: 'orgaddress' },
              addressUsageType: { id: AddressTypes.Mailing },
            },
          ],
        },
      },
    });
    expect(file.prov_solicitor?.full_name_string).toBe('testOrg (Inc. No. )');
    expect(file.prov_solicitor?.email).toBe('');
    expect(file.prov_solicitor?.phone).toBe('');
    expect(file.prov_solicitor_attn?.full_name_string).toBe('');
    expect(file.prov_solicitor?.address?.address_string).toBe('orgaddress');
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
});

import { ApiGen_Concepts_Association } from '@/models/api/generated/ApiGen_Concepts_Association';
import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';

export const getMockApiAssociation = (fileId = 1): ApiGen_Concepts_Association => ({
  id: fileId,
  fileNumber: '02-100885-01',
  fileName: 'test file',
  createdDateTime: '2023-11-03T01:02:05.203',
  createdBy: 'PAIMS_PIMS_ACQUISITION',
  createdByGuid: null,
  status: 'Active',
  statusCode: 'ACTIVE',
});

export const getMockPropertyAssociations = (
  id = 1,
  pid = null,
): ApiGen_Concepts_PropertyAssociations => ({
  id,
  pid,
  acquisitionAssociations: [],
  dispositionAssociations: [],
  leaseAssociations: [],
  managementAssociations: [],
  researchAssociations: [],
});

import { ApiGen_Concepts_FormDocumentFile } from '@/models/api/generated/ApiGen_Concepts_FormDocumentFile';
import { ApiGen_Concepts_FormDocumentType } from '@/models/api/generated/ApiGen_Concepts_FormDocumentType';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
export const getMockApiFormDocumentType = (): ApiGen_Concepts_FormDocumentType => ({
  formTypeCode: 'H120',
  documentId: null,
  description: '',
  displayOrder: 1,
  ...getEmptyBaseAudit(),
});
export const getMockApiFileForms = (): ApiGen_Concepts_FormDocumentFile[] => [
  {
    id: 2,
    fileId: 1,
    formDocumentType: {
      formTypeCode: 'H179T',
      description: 'Offer agreement - Total (H179 T)',
      documentId: null,
      displayOrder: 0,
      ...getEmptyBaseAudit(),
    },
    ...getEmptyBaseAudit(1),
  },
  {
    id: 3,
    fileId: 1,
    formDocumentType: {
      formTypeCode: 'H179A',
      description: 'Offer agreement - Section 3 (H179 A)',
      documentId: null,
      displayOrder: 0,
      ...getEmptyBaseAudit(),
    },
    ...getEmptyBaseAudit(1),
  },
  {
    id: 4,
    fileId: 1,
    formDocumentType: {
      formTypeCode: 'H120',
      description: 'Payment requisition (H120)',
      documentId: null,
      displayOrder: 0,
      ...getEmptyBaseAudit(),
    },
    ...getEmptyBaseAudit(1),
  },
];

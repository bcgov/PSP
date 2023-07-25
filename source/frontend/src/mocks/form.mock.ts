import { Api_FormDocumentFile, Api_FormDocumentType } from '@/models/api/FormDocument';
export const getMockApiFormDocumentType = (): Api_FormDocumentType => ({
  formTypeCode: 'H120',
  documentId: null,
  description: '',
  displayOrder: 1,
});
export const getMockApiFileForms = (): Api_FormDocumentFile[] => [
  {
    id: 2,
    fileId: 1,
    formDocumentType: {
      formTypeCode: 'H179T',
      description: 'Offer agreement - Total (H179 T)',
      documentId: null,
      displayOrder: 0,
    },
    rowVersion: 1,
  },
  {
    id: 3,
    fileId: 1,
    formDocumentType: {
      formTypeCode: 'H179A',
      description: 'Offer agreement - Section 3 (H179 A)',
      documentId: null,
      displayOrder: 0,
    },
    rowVersion: 1,
  },
  {
    id: 4,
    fileId: 1,
    formDocumentType: {
      formTypeCode: 'H120',
      description: 'Payment requisition (H120)',
      documentId: null,
      displayOrder: 0,
    },
    rowVersion: 1,
  },
];

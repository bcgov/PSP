import { Api_Document, Api_DocumentType } from 'models/api/Document';

export const mockDocumentsResponse = (): Api_Document[] => [
  {
    mayanDocumentId: 13,
    documentType: { documentType: 'Survey', id: 1 },
    fileName: 'Survey.pdf',
    id: 1,
    statusTypeCode: {
      id: 'DRAFT',
      description: 'Draft',
    },
    appCreateUserid: 'James Bond',
    appCreateTimestamp: '10-Jan-2022',
  },
  {
    mayanDocumentId: 13,
    documentType: { documentType: 'Photo', id: 2 },
    fileName: 'Photo_Rest.pdf',
    id: 1,
    statusTypeCode: {
      id: 'AMENDD',
      description: 'Amended',
    },
    appCreateUserid: 'Amelia Bond',
    appCreateTimestamp: '11-Jan-2022',
  },
  {
    mayanDocumentId: 13,
    documentType: { documentType: 'Correspondence', id: 3 },
    fileName: 'Test Correspondence.pdf',
    id: 1,
    statusTypeCode: {
      id: 'RGSTRD',
      description: 'Registered',
    },
    appCreateUserid: 'Jerry J',
    appCreateTimestamp: '13-Jan-2022',
  },
  {
    mayanDocumentId: 13,
    documentType: { documentType: 'Correspondence', id: 3 },
    fileName: 'New Correspondence.pdf',
    id: 1,
    statusTypeCode: {
      id: 'SENT',
      description: 'Sent',
    },
    appCreateUserid: 'Tom Hank',
    appCreateTimestamp: '14-Jan-2022',
  },
];

export const mockDocumentTypesResponse = (): Api_DocumentType[] => [
  {
    documentType: 'Survey',
    id: 1,
    appCreateUserid: 'James Bond',
    appCreateTimestamp: '10-Jan-2022',
  },
];

import { Api_Document, Api_Document_Type } from 'models/api/Document';

export const mockDocumentsResponse = (): Api_Document[] => [
  {
    documentTypeId: 1,
    documentType: 'Survey',
    fileName: 'Survey.pdf',
    id: 1,
    statusCode: 'DRAFT',
    status: 'Draft',
    appCreateUserid: 'James Bond',
    appCreateTimestamp: '10-Jan-2022',
  },
  {
    documentTypeId: 2,
    documentType: 'Photo',
    fileName: 'Photo_Rest.pdf',
    id: 1,
    statusCode: 'AMENDD',
    status: 'Amended',
    appCreateUserid: 'Amelia Bond',
    appCreateTimestamp: '11-Jan-2022',
  },
  {
    documentTypeId: 3,
    documentType: 'Correspondence',
    fileName: 'Test Correspondence.pdf',
    id: 1,
    statusCode: 'RGSTRD',
    status: 'Registered',
    appCreateUserid: 'Jerry J',
    appCreateTimestamp: '13-Jan-2022',
  },
  {
    documentTypeId: 3,
    documentType: 'Correspondence',
    fileName: 'New Correspondence.pdf',
    id: 1,
    statusCode: 'SENT',
    status: 'Sent',
    appCreateUserid: 'Tom Hank',
    appCreateTimestamp: '14-Jan-2022',
  },
];

export const mockDocumentTypesResponse = (): Api_Document_Type[] => [
  {
    documentType: 'Survey',
    id: 1,
    appCreateUserid: 'James Bond',
    appCreateTimestamp: '10-Jan-2022',
  },
];

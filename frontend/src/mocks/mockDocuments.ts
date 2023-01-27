import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { Api_DocumentRelationship, Api_DocumentType } from 'models/api/Document';

export const mockDocumentsResponse = (): Api_DocumentRelationship[] => [
  {
    id: 1,
    parentId: 1,
    isDisabled: false,
    document: {
      id: 1,
      mayanDocumentId: 13,
      documentType: {
        id: 12,
        documentType: 'Gazette',
        appCreateTimestamp: '2022-07-27T16:06:42.42',
        appLastUpdateTimestamp: '2022-07-27T16:06:42.42',
        appLastUpdateUserid: 'service',
        appCreateUserid: 'service',
        appLastUpdateUserGuid: '00000000-0000-0000-0000-000000000000',
        appCreateUserGuid: '00000000-0000-0000-0000-000000000000',
        rowVersion: 1,
      },
      statusTypeCode: {
        id: 'SIGND',
        description: 'Signed',
        isDisabled: false,
      },
      fileName: 'DocTest.docx',
      appCreateTimestamp: '0001-01-01T00:00:00',
      appLastUpdateTimestamp: '0001-01-01T00:00:00',
      rowVersion: 1,
    },
    relationshipType: DocumentRelationshipType.ACTIVITIES,
    appCreateTimestamp: '2022-07-27T16:10:01.82',
    appLastUpdateTimestamp: '2022-07-27T16:10:01.82',
    appLastUpdateUserid: 'dbo',
    appCreateUserid: 'dbo',
    rowVersion: 1,
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

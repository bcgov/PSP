import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { Api_DocumentRelationship, Api_DocumentType } from 'models/api/Document';
import {
  Api_Storage_DocumentMetadata,
  Api_Storage_DocumentTypeMetadataType,
} from 'models/api/DocumentStorage';

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
        mayanId: undefined,
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
  {
    id: 2,
    parentId: 1,
    isDisabled: false,
    document: {
      id: 2,
      mayanDocumentId: 33,
      documentType: {
        id: 8,
        documentType: 'MoTI Plan',
        mayanId: 24,
        appCreateTimestamp: '2022-09-08T21:18:09.01',
        appLastUpdateTimestamp: '2022-09-08T21:18:09.01',
        appLastUpdateUserid: 'admin',
        appCreateUserid: 'admin',
        appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
        appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
        rowVersion: 1,
      },
      statusTypeCode: {
        id: 'AMENDD',
        description: 'Amended',
        isDisabled: false,
      },
      fileName: 'moti_plan.txt',
      appCreateTimestamp: '2022-09-08T21:18:54.057',
      appLastUpdateTimestamp: '2022-09-08T21:18:54.057',
      appLastUpdateUserid: 'admin',
      appCreateUserid: 'admin',
      appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
      appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
      rowVersion: 1,
    },
    relationshipType: DocumentRelationshipType.ACTIVITIES,
    appCreateTimestamp: '2022-09-08T21:18:54.057',
    appLastUpdateTimestamp: '2022-09-08T21:18:54.057',
    appLastUpdateUserid: 'admin',
    appCreateUserid: 'admin',
    appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
    appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
    rowVersion: 1,
  },
];

export const mockDocumentTypesResponse = (): Api_DocumentType[] => [
  {
    documentType: 'Survey',
    id: 1,
    mayanId: 8,
    appCreateUserid: 'James Bond',
    appCreateTimestamp: '10-Jan-2022',
  },
  {
    id: 2,
    documentType: 'Privy Council',
    mayanId: 7,
    appCreateUserid: 'James Bond',
    appCreateTimestamp: '10-Jan-2022',
  },
];

export const mockDocumentTypeMetadata = (): Api_Storage_DocumentTypeMetadataType[] => [
  {
    id: 1,
    document_type: {
      id: 1,
      label: 'Survey',
    },
    metadata_type: {
      default: '',
      id: 1,
      label: 'Tag',
      lookup: '',
      name: 'Tag',
      parser: '',
      parser_arguments: '',
      url: '',
      validation: '',
      validation_arguments: '',
    },
    required: true,
  },
];

export const mockDocumentMetadata = (): Api_Storage_DocumentMetadata[] => [
  {
    document: {
      label: '',
      datetime_created: '2022-07-27T16:06:42.42',
      description: '',
      file_latest: {
        id: 2,
        document_id: 1,
        comment: '',
        encoding: '',
        fileName: '',
        mimetype: '',
        size: 12,
        timeStamp: '',
      },
      id: 1,
      document_type: { id: 1, label: 'Survey' },
    },
    id: 1,
    metadata_type: { id: 1, label: 'Tag' },
    url: '',
    value: 'Tag1234',
  },
];

import { SelectOption } from 'components/common/form';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { useApiDocuments } from 'hooks/pims-api/useApiDocuments';
import useIsMounted from 'hooks/useIsMounted';
import { Api_Document, Api_DocumentType, Api_UploadRequest } from 'models/api/Document';
import { Api_Storage_DocumentTypeMetadataType } from 'models/api/DocumentStorage';
import { ExternalResultStatus } from 'models/api/ExternalResult';
import { useEffect, useState } from 'react';

import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import { ComposedDocument } from './ComposedDocument';
import DocumentUploadView from './DocumentUploadView';
import DocumentDetailView from './DocumentUploadView';

export interface IDocumentUploadContainerProps {
  parentId: number;
  relationshipType: DocumentRelationshipType;
  onUploadSuccess: () => void;
}

export const DocumentUploadContainer: React.FunctionComponent<IDocumentUploadContainerProps> = props => {
  const isMounted = useIsMounted();
  const { getDocumentTypes } = useApiDocuments();
  const { retrieveDocumentMetadata, retrieveDocumentMetadataLoading } = useDocumentProvider();

  const { uploadDocument, uploadDocumentLoading } = useDocumentRelationshipProvider();

  const [documentTypes, setDocumentTypes] = useState<Api_DocumentType[]>([]);

  useEffect(() => {
    const fetch = async () => {
      const axiosResponse = await getDocumentTypes();
      if (axiosResponse && isMounted()) {
        setDocumentTypes(axiosResponse.data);
      }
    };

    fetch();
  }, [retrieveDocumentMetadata]);

  const onDocumentTypeChange = (param: any) => {
    console.log(param);
  };

  const onUploadDocument = async (uploadRequest: Api_UploadRequest) => {
    var result = await uploadDocument(props.relationshipType, props.parentId, uploadRequest);
    console.log(result);
    props.onUploadSuccess();
  };

  return (
    <DocumentUploadView
      documentTypes={documentTypes}
      isLoading={retrieveDocumentMetadataLoading}
      mayanMetadata={lele}
      onDocumentTypeChange={onDocumentTypeChange}
      onUploadDocument={onUploadDocument}
    />
  );
};

const lele: Api_Storage_DocumentTypeMetadataType[] = [
  {
    document_type: {
      delete_time_period: 30,
      delete_time_unit: 'days',
      filename_generator_backend: 'uuid',
      filename_generator_backend_arguments: '',
      id: 22,
      label: 'Gazette',
      quick_label_list_url: 'http://localhost:7080/api/v4/document_types/22/quick_labels/',
      trash_time_period: undefined,
      trash_time_unit: undefined,
      url: 'http://localhost:7080/api/v4/document_types/22/',
    },
    id: 114,
    metadata_type: {
      default: undefined,
      id: 115,
      label: 'Gazette Date',
      lookup: undefined,
      name: 'Gazette_Date',
      parser: '',
      parser_arguments: '',
      url: 'http://localhost:7080/api/v4/metadata_types/115/',
      validation: '',
      validation_arguments: '',
    },
    required: false,
    url: 'http://localhost:7080/api/v4/document_types/22/metadata_types/114/',
  },
  {
    document_type: {
      delete_time_period: 30,
      delete_time_unit: 'days',
      filename_generator_backend: 'uuid',
      filename_generator_backend_arguments: '',
      id: 22,
      label: 'Gazette',
      quick_label_list_url: 'http://localhost:7080/api/v4/document_types/22/quick_labels/',
      trash_time_period: undefined,
      trash_time_unit: undefined,
      url: 'http://localhost:7080/api/v4/document_types/22/',
    },
    id: 116,
    metadata_type: {
      default: undefined,
      id: 116,
      label: 'Gazette Page #',
      lookup: undefined,
      name: 'Gazette_Page_#',
      parser: '',
      parser_arguments: '',
      url: 'http://localhost:7080/api/v4/metadata_types/116/',
      validation: '',
      validation_arguments: '',
    },
    required: false,
    url: 'http://localhost:7080/api/v4/document_types/22/metadata_types/116/',
  },
  {
    document_type: {
      delete_time_period: 30,
      delete_time_unit: 'days',
      filename_generator_backend: 'uuid',
      filename_generator_backend_arguments: '',
      id: 22,
      label: 'Gazette',
      quick_label_list_url: 'http://localhost:7080/api/v4/document_types/22/quick_labels/',
      trash_time_period: undefined,
      trash_time_unit: undefined,
      url: 'http://localhost:7080/api/v4/document_types/22/',
    },
    id: 111,
    metadata_type: {
      default: undefined,
      id: 114,
      label: 'Gazette Published Date',
      lookup: undefined,
      name: 'Gazette_Published_Date',
      parser: '',
      parser_arguments: '',
      url: 'http://localhost:7080/api/v4/metadata_types/114/',
      validation: '',
      validation_arguments: '',
    },
    required: true,
    url: 'http://localhost:7080/api/v4/document_types/22/metadata_types/111/',
  },
  {
    document_type: {
      delete_time_period: 30,
      delete_time_unit: 'days',
      filename_generator_backend: 'uuid',
      filename_generator_backend_arguments: '',
      id: 22,
      label: 'Gazette',
      quick_label_list_url: 'http://localhost:7080/api/v4/document_types/22/quick_labels/',
      trash_time_period: undefined,
      trash_time_unit: undefined,
      url: 'http://localhost:7080/api/v4/document_types/22/',
    },
    id: 120,
    metadata_type: {
      default: undefined,
      id: 117,
      label: 'Gazette Type',
      lookup: undefined,
      name: 'Gazette_Type',
      parser: '',
      parser_arguments: '',
      url: 'http://localhost:7080/api/v4/metadata_types/117/',
      validation: '',
      validation_arguments: '',
    },
    required: true,
    url: 'http://localhost:7080/api/v4/document_types/22/metadata_types/120/',
  },
  {
    document_type: {
      delete_time_period: 30,
      delete_time_unit: 'days',
      filename_generator_backend: 'uuid',
      filename_generator_backend_arguments: '',
      id: 22,
      label: 'Gazette',
      quick_label_list_url: 'http://localhost:7080/api/v4/document_types/22/quick_labels/',
      trash_time_period: undefined,
      trash_time_unit: undefined,
      url: 'http://localhost:7080/api/v4/document_types/22/',
    },
    id: 123,
    metadata_type: {
      default: undefined,
      id: 120,
      label: 'LTSA schedule filing #',
      lookup: undefined,
      name: 'LTSA_schedule_filing_#',
      parser: '',
      parser_arguments: '',
      url: 'http://localhost:7080/api/v4/metadata_types/120/',
      validation: '',
      validation_arguments: '',
    },
    required: false,
    url: 'http://localhost:7080/api/v4/document_types/22/metadata_types/123/',
  },
  {
    document_type: {
      delete_time_period: 30,
      delete_time_unit: 'days',
      filename_generator_backend: 'uuid',
      filename_generator_backend_arguments: '',
      id: 22,
      label: 'Gazette',
      quick_label_list_url: 'http://localhost:7080/api/v4/document_types/22/quick_labels/',
      trash_time_period: undefined,
      trash_time_unit: undefined,
      url: 'http://localhost:7080/api/v4/document_types/22/',
    },
    id: 122,
    metadata_type: {
      default: undefined,
      id: 119,
      label: 'Legal Survey Plan #',
      lookup: undefined,
      name: 'Legal_Survey_Plan_#',
      parser: '',
      parser_arguments: '',
      url: 'http://localhost:7080/api/v4/metadata_types/119/',
      validation: '',
      validation_arguments: '',
    },
    required: false,
    url: 'http://localhost:7080/api/v4/document_types/22/metadata_types/122/',
  },
  {
    document_type: {
      delete_time_period: 30,
      delete_time_unit: 'days',
      filename_generator_backend: 'uuid',
      filename_generator_backend_arguments: '',
      id: 22,
      label: 'Gazette',
      quick_label_list_url: 'http://localhost:7080/api/v4/document_types/22/quick_labels/',
      trash_time_period: undefined,
      trash_time_unit: undefined,
      url: 'http://localhost:7080/api/v4/document_types/22/',
    },
    id: 121,
    metadata_type: {
      default: undefined,
      id: 118,
      label: 'MoTI Plan #',
      lookup: undefined,
      name: 'MoTI_Plan_#',
      parser: '',
      parser_arguments: '',
      url: 'http://localhost:7080/api/v4/metadata_types/118/',
      validation: '',
      validation_arguments: '',
    },
    required: false,
    url: 'http://localhost:7080/api/v4/document_types/22/metadata_types/121/',
  },
  {
    document_type: {
      delete_time_period: 30,
      delete_time_unit: 'days',
      filename_generator_backend: 'uuid',
      filename_generator_backend_arguments: '',
      id: 22,
      label: 'Gazette',
      quick_label_list_url: 'http://localhost:7080/api/v4/document_types/22/quick_labels/',
      trash_time_period: undefined,
      trash_time_unit: undefined,
      url: 'http://localhost:7080/api/v4/document_types/22/',
    },
    id: 108,
    metadata_type: {
      default: undefined,
      id: 113,
      label: 'Road Name',
      lookup: undefined,
      name: 'Road_Name',
      parser: '',
      parser_arguments: '',
      url: 'http://localhost:7080/api/v4/metadata_types/113/',
      validation: '',
      validation_arguments: '',
    },
    required: true,
    url: 'http://localhost:7080/api/v4/document_types/22/metadata_types/108/',
  },
];

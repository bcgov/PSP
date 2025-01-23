import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { Claims } from '@/constants/claims';
import { mockDocumentTypesAcquisition } from '@/mocks/documents.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Mayan_DocumentMetadata } from '@/models/api/generated/ApiGen_Mayan_DocumentMetadata';
import { ApiGen_Mayan_DocumentType } from '@/models/api/generated/ApiGen_Mayan_DocumentType';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { ApiGen_Mayan_MetadataType } from '@/models/api/generated/ApiGen_Mayan_MetadataType';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockKeycloak, render, RenderOptions } from '@/utils/test-utils';

import { ComposedDocument, DocumentUpdateFormData } from '../ComposedDocument';
import { DocumentDetailForm, IDocumentDetailFormProps } from './DocumentDetailForm';

// mock auth library

const history = createMemoryHistory();

const documentTypes: ApiGen_Mayan_DocumentType[] = [
  {
    id: 1,
    label: 'BC Assessment Search',

    delete_time_period: null,
    delete_time_unit: null,
    trash_time_period: null,
    trash_time_unit: null,

    document_stub_expiration_interval: null,
    document_stub_pruning_enabled: null,

    filename_generator_backend: '',
    filename_generator_backend_arguments: '',
  },
];

const metadataTypes: ApiGen_Mayan_MetadataType[] = [
  {
    id: 1,
    label: 'Tag Foo',
    name: 'tag-foo',
    default: null,
    lookup: null,
    parser: null,
    parser_arguments: null,
    url: null,
    validation: null,
    validation_arguments: null,
  },
  {
    id: 2,
    label: 'Tag Bar',
    name: 'tag-bar',
    default: null,
    lookup: null,
    parser: null,
    parser_arguments: null,
    url: null,
    validation: null,
    validation_arguments: null,
  },
];

const documentMetadata: ApiGen_Mayan_DocumentMetadata[] = [
  {
    document: {
      label: '',
      datetime_created: '2022-07-27T16:06:42.42',
      description: '',
      language: '',
      uuid: '',
      file_latest: {
        id: 2,
        comment: '',
        encoding: '',
        mimetype: '',
        size: 12,
        filename: null,
        timestamp: '',
        checksum: '',
        file: '',
      },
      id: 1,
      document_type: documentTypes[0],
    },
    id: 1,
    metadata_type: metadataTypes[0],
    url: '',
    value: 'Tag1234',
  },
];

const documentTypeMetadataType: ApiGen_Mayan_DocumentTypeMetadataType[] = [
  {
    id: 2,
    document_type: documentTypes[0],
    metadata_type: metadataTypes[0],
    url: '',
    required: false,
  },
];

const mockDocument: ComposedDocument = {
  mayanMetadata: documentMetadata,
  pimsDocumentRelationship: {
    id: 1,
    document: {
      mayanDocumentId: 15,
      documentType: {
        id: 36,
        documentType: 'AFFISERV',
        documentTypeDescription: 'Affidavit of service',
        mayanId: 272,
        isDisabled: false,
        appCreateTimestamp: '2024-10-15T19:44:11.713',
        appLastUpdateTimestamp: '2024-10-15T19:46:55.25',
        appLastUpdateUserid: 'EHERRERA',
        appCreateUserid: 'EHERRERA',
        appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
        appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
        rowVersion: 2,
        documentTypePurpose: 'Test document purpouse',
      },
      statusTypeCode: {
        id: 'AMEND',
        description: 'Amended',
        isDisabled: false,
        displayOrder: null,
      },
      fileName: 'NewFile.doc',
      id: 0,
      rowVersion: 1,
      appCreateTimestamp: EpochIsoDateTime,
      appLastUpdateTimestamp: EpochIsoDateTime,
      appLastUpdateUserid: null,
      appCreateUserid: null,
      appLastUpdateUserGuid: null,
      appCreateUserGuid: null,
      documentQueueStatusTypeCode: null,
    },
    parentId: null,
    relationshipType: ApiGen_CodeTypes_DocumentRelationType.AcquisitionFiles,
    appCreateTimestamp: EpochIsoDateTime,
    appLastUpdateTimestamp: EpochIsoDateTime,
    appLastUpdateUserid: null,
    appCreateUserid: null,
    appLastUpdateUserGuid: null,
    appCreateUserGuid: null,
    rowVersion: null,
  },
  mayanFileId: 2,
};

const onUpdate = vi.fn();
const onCancel = vi.fn();
const onDocumentTypeChange = vi.fn();

describe('DocumentDetailForm component', () => {
  // render component under test
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IDocumentDetailFormProps> },
  ) => {
    const formikRef = createRef<FormikProps<DocumentUpdateFormData>>();

    const utils = render(
      <DocumentDetailForm
        {...renderOptions.props}
        formikRef={formikRef}
        document={renderOptions.props?.document ?? mockDocument}
        mayanMetadataTypes={renderOptions.props?.mayanMetadataTypes ?? documentTypeMetadataType}
        documentTypes={renderOptions.props?.documentTypes ?? mockDocumentTypesAcquisition()}
        isLoading={renderOptions.props?.isLoading ?? false}
        relationshipType={
          renderOptions.props?.relationshipType ??
          ApiGen_CodeTypes_DocumentRelationType.AcquisitionFiles
        }
        documentTypeUpdated={renderOptions.props?.documentTypeUpdated ?? false}
        onUpdate={onUpdate}
        onCancel={onCancel}
        onDocumentTypeChange={onDocumentTypeChange}
      />,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        history,
      },
    );

    return {
      ...utils,
      formikRef,
      useMockAuthentication: true,
      getDocumentTypeSelect: () => {
        return utils.container.querySelector(`#input-documentTypeId`) as HTMLElement;
      },
    };
  };

  beforeEach(() => {
    mockKeycloak({ claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_EDIT] });
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    setup({});
    expect(document.body).toMatchSnapshot();
  });

  it('renders the file name', async () => {
    const { getAllByText } = await setup({});
    const textarea = getAllByText('NewFile.doc')[0];

    expect(textarea).toBeVisible();
  });

  it('renders the document type', async () => {
    const { getAllByText } = await setup({});
    const textarea = getAllByText('Affidavit of service')[0];

    expect(textarea).toBeVisible();
  });

  it('displays field for metadata types', async () => {
    const { getAllByDisplayValue } = await setup({});
    const textarea = getAllByDisplayValue('Tag1234')[0];

    expect(textarea).toBeVisible();
  });

  it('disables document type select for templates', async () => {
    const { getDocumentTypeSelect } = await setup({
      props: { relationshipType: ApiGen_CodeTypes_DocumentRelationType.Templates },
    });

    const select = getDocumentTypeSelect();
    expect(select).toBeDisabled();
  });

  it('displays a warning legend when the document type has changed.', async () => {
    const { getByText } = await setup({
      props: { documentTypeUpdated: true },
    });

    const warningLegend = getByText(
      'Some associated metadata may be lost if the document type is changed.',
    );
    expect(warningLegend).toBeInTheDocument();
  });
});

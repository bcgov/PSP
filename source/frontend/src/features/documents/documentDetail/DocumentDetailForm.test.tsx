import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/claims';
import { mockDocumentTypesResponse } from '@/mocks/documents.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Mayan_DocumentMetadata } from '@/models/api/generated/ApiGen_Mayan_DocumentMetadata';
import { ApiGen_Mayan_DocumentType } from '@/models/api/generated/ApiGen_Mayan_DocumentType';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { ApiGen_Mayan_MetadataType } from '@/models/api/generated/ApiGen_Mayan_MetadataType';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockKeycloak, render, RenderOptions } from '@/utils/test-utils';

import { ComposedDocument } from '../ComposedDocument';
import { DocumentDetailForm } from './DocumentDetailForm';

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
      file_latest: {
        id: 2,

        comment: '',
        encoding: '',

        mimetype: '',
        size: 12,

        filename: null,
        timestamp: '',
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
      documentType: mockDocumentTypesResponse()[0],
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
describe('DocumentDetailForm component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions) => {
    const utils = render(
      <DocumentDetailForm
        onUpdate={vi.fn()}
        document={mockDocument}
        isLoading={false}
        mayanMetadataTypes={documentTypeMetadataType}
        onCancel={vi.fn()}
        formikRef={{ current: { submitForm: vi.fn() } } as any}
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
      useMockAuthentication: true,
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

  it('renders the file name', () => {
    const { getAllByText } = setup({});
    const textarea = getAllByText('NewFile.doc')[0];

    expect(textarea).toBeVisible();
  });

  it('renders the document type', () => {
    const { getAllByText } = setup({});
    const textarea = getAllByText('Survey')[0];

    expect(textarea).toBeVisible();
  });

  it('displays field for metadata types', async () => {
    const { getAllByDisplayValue } = setup({});
    const textarea = getAllByDisplayValue('Tag1234')[0];

    expect(textarea).toBeVisible();
  });
});

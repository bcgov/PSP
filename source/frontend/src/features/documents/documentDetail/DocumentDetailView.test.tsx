import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/claims';
import { mockDocumentTypesResponse } from '@/mocks/documents.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Mayan_DocumentMetadata } from '@/models/api/generated/ApiGen_Mayan_DocumentMetadata';
import { ApiGen_Mayan_MetadataType } from '@/models/api/generated/ApiGen_Mayan_MetadataType';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockKeycloak, render, RenderOptions } from '@/utils/test-utils';

import { ComposedDocument } from '../ComposedDocument';
import { DocumentDetailView } from './DocumentDetailView';

// mock auth library

const history = createMemoryHistory();

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

const documentTypeMetadata: ApiGen_Mayan_DocumentMetadata[] = [
  {
    document: {
      label: '',
      datetime_created: '2022-07-27T16:06:42.42',
      description: '',
      file_latest: {
        id: 2,
        comment: '',
        encoding: '',
        filename: '',
        mimetype: '',
        size: 12,
        timestamp: '',
      },
      id: 1,
      document_type: {
        id: 1,
        label: 'BC Assessment Search',
        delete_time_period: null,
        delete_time_unit: null,
        trash_time_period: null,
        trash_time_unit: null,
      },
    },
    id: 1,
    metadata_type: metadataTypes[0],
    url: '',
    value: 'Tag1234',
  },
];
const mockDocument: ComposedDocument = {
  mayanMetadata: documentTypeMetadata,
  pimsDocumentRelationship: {
    id: 1,
    document: {
      id: 1,
      mayanDocumentId: 15,
      documentType: mockDocumentTypesResponse()[0],
      statusTypeCode: {
        id: 'AMEND',
        description: 'Amended',
        isDisabled: false,
        displayOrder: null,
      },
      fileName: 'NewFile.doc',
      rowVersion: 0,
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
describe('DocumentDetailView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { document?: ComposedDocument }) => {
    const utils = render(
      <DocumentDetailView
        document={renderOptions.document ?? mockDocument}
        isLoading={false}
        setIsEditable={vi.fn()}
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
  it('displays label for metadata types', async () => {
    const { getAllByText } = setup({});
    const textarea = getAllByText('Tag1234')[0];

    expect(textarea).toBeVisible();
  });

  it('displays tooltip if file not available', async () => {
    const { getAllByTestId } = setup({});
    const downloadButtonTooltip = await getAllByTestId(
      'tooltip-icon-document-not-available-tooltip',
    );

    expect(downloadButtonTooltip[0]).toBeVisible();
  });

  it('displays tooltip if file available', async () => {
    const { getAllByTestId } = setup({
      document: { ...mockDocument, documentDetail: { file_latest: { id: 1 } } as any },
    });
    const downloadButtonTooltip = await getAllByTestId('document-download-button');

    expect(downloadButtonTooltip[0]).toBeVisible();
  });
});

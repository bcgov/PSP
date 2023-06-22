import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/claims';
import { mockDocumentTypesResponse } from '@/mocks/documents.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import {
  Api_Storage_DocumentMetadata,
  Api_Storage_MetadataType,
} from '@/models/api/DocumentStorage';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockKeycloak, render, RenderOptions } from '@/utils/test-utils';

import { ComposedDocument } from '../ComposedDocument';
import { DocumentDetailForm } from './DocumentDetailForm';

// mock auth library
jest.mock('@react-keycloak/web');

const history = createMemoryHistory();

const metadataTypes: Api_Storage_MetadataType[] = [
  { id: 1, label: 'Tag Foo', name: 'tag-foo' },
  { id: 2, label: 'Tag Bar', name: 'tag-bar' },
];
const documentTypeMetadata: Api_Storage_DocumentMetadata[] = [
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
      document_type: { id: 1, label: 'BC Assessment Search' },
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
      mayanDocumentId: 15,
      documentType: mockDocumentTypesResponse()[0],
      statusTypeCode: { id: 'AMEND', description: 'Amended' },
      fileName: 'NewFile.doc',
    },
    parentId: null,
    relationshipType: null,
  },
  mayanFileId: 2,
};
describe('DocumentDetailForm component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions) => {
    const utils = render(
      <DocumentDetailForm
        onUpdate={jest.fn()}
        document={mockDocument}
        isLoading={false}
        mayanMetadataTypes={documentTypeMetadata}
        onCancel={jest.fn()}
        formikRef={{ current: { submitForm: jest.fn() } } as any}
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
    jest.clearAllMocks();
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

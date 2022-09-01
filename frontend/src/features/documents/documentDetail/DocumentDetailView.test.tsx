import { Claims } from 'constants/claims';
import { createMemoryHistory } from 'history';
import { render, RenderOptions } from 'utils/test-utils';
import { ComposedDocument } from '../ComposedDocument';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { DocumentDetailView } from './DocumentDetailView';
import { mockDocumentMetadata, mockDocumentTypesResponse } from 'mocks/mockDocuments';
import { mockLookups } from 'mocks';

// mock auth library
jest.mock('@react-keycloak/web');

const history = createMemoryHistory();

const mockDocument: ComposedDocument = {
  mayanMetadata: mockDocumentMetadata(),
  pimsDocument: {
    id: 1,
    mayanDocumentId: 15,
    documentType: mockDocumentTypesResponse()[0],
    statusTypeCode: { id: 'AMEND', description: 'Amended' },
    fileName: 'NewFile.doc',
  },
};
describe('DocumentDetailView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions) => {
    const utils = render(
      <DocumentDetailView onUpdate={jest.fn()} document={mockDocument} isLoading={false} />,
      {
        ...renderOptions,
        useMockAuthentication: true,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        history,
        claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_EDIT],
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {});

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
  it('displays label for metadata types', async () => {
    const { getAllByText } = setup({});
    const textarea = getAllByText('Tag1234')[0];

    expect(textarea).toBeVisible();
  });
});

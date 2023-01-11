import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import Claims from 'constants/claims';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { noop } from 'lodash';
import { mockLookups } from 'mocks';
import { mockDocumentDetailResponse } from 'mocks/mockDocumentDetail';
import { mockDocumentsResponse, mockDocumentTypesResponse } from 'mocks/mockDocuments';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { cleanup, render, RenderOptions, waitFor } from 'utils/test-utils';

import { DocumentRow } from '../ComposedDocument';
import { DocumentListView, IDocumentListViewProps } from './DocumentListView';

const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const deleteMock = jest.fn().mockResolvedValue(true);

const mockDocumentRowResponse = () =>
  mockDocumentsResponse().map(x =>
    x?.document ? DocumentRow.fromApi(x.document) : new DocumentRow(),
  );

jest.mock('@react-keycloak/web');
(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    userInfo: {
      organizations: [1],
      roles: [],
    },
    subject: 'test',
  },
});

describe('Document List View', () => {
  const setup = (renderOptions?: RenderOptions & IDocumentListViewProps) => {
    // render component under test
    const component = render(
      <DocumentListView
        isLoading={false}
        parentId={renderOptions?.parentId || 0}
        relationshipType={
          renderOptions?.relationshipType || DocumentRelationshipType.RESEARCH_FILES
        }
        documentResults={
          renderOptions?.documentResults ||
          mockDocumentsResponse().map(x =>
            x?.document ? DocumentRow.fromApi(x.document) : new DocumentRow(),
          )
        }
        onDelete={renderOptions?.onDelete || deleteMock}
        onSuccess={renderOptions?.onSuccess || noop}
      />,
      {
        ...renderOptions,
        store: storeState,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    mockAxios.reset();
    mockAxios.onGet(`documents/types`).reply(200, mockDocumentTypesResponse());
  });
  afterEach(() => {
    mockAxios.reset();
    cleanup();
  });
  afterAll(() => {
    jest.restoreAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('should have the Documents type in the component', async () => {
    const { getByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: 0,
      relationshipType: DocumentRelationshipType.RESEARCH_FILES,
      documentResults: mockDocumentRowResponse(),
      onDelete: deleteMock,
      onSuccess: noop,
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    expect(getByTestId('document-type')).toBeInTheDocument();
  });

  it('should have the Documents filename in the component', async () => {
    const { getByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: 0,
      relationshipType: DocumentRelationshipType.RESEARCH_FILES,
      documentResults: mockDocumentRowResponse(),
      onDelete: deleteMock,
      onSuccess: noop,
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    expect(getByTestId('document-filename')).toBeInTheDocument();
  });

  it('should have the Documents add button in the component', async () => {
    const { getByText } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: 0,
      relationshipType: DocumentRelationshipType.RESEARCH_FILES,
      documentResults: mockDocumentRowResponse(),
      onDelete: deleteMock,
      onSuccess: noop,
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    expect(getByText('Add a Document')).toBeInTheDocument();
  });

  it('should display the warning tooltip instead of the download icon', async () => {
    const { findAllByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: 0,
      relationshipType: DocumentRelationshipType.RESEARCH_FILES,
      documentResults: mockDocumentRowResponse(),
      onDelete: deleteMock,
      onSuccess: noop,
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    const downloadButtonTooltip = await findAllByTestId(
      'tooltip-icon-document-not-available-tooltip',
    );
    expect(downloadButtonTooltip[0]).toBeInTheDocument();
  });

  it('should display the download icon if download is available', async () => {
    mockAxios.onGet().reply(200, mockDocumentDetailResponse());
    const documentRows = mockDocumentRowResponse();
    documentRows[0].isFileAvailable = true;
    const { findByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: 0,
      relationshipType: DocumentRelationshipType.RESEARCH_FILES,
      documentResults: documentRows,
      onDelete: deleteMock,
      onSuccess: noop,
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    const downloadButtonTooltip = await findByTestId('document-download-button');
    expect(downloadButtonTooltip).toBeInTheDocument();
  });
});

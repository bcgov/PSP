import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import Claims from 'constants/claims';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { noop } from 'lodash';
import { mockLookups } from 'mocks';
import { mockDocumentsResponse, mockDocumentTypesResponse } from 'mocks/mockDocuments';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { cleanup, mockKeycloak, render, RenderOptions, waitFor } from 'utils/test-utils';

import { DocumentListView, IDocumentListViewProps } from './DocumentListView';

const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const deleteMock = jest.fn().mockResolvedValue(true);

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
        relationshipType={renderOptions?.relationshipType || DocumentRelationshipType.FILES}
        documentResults={renderOptions?.documentResults || mockDocumentsResponse()}
        onDelete={renderOptions?.onDelete || deleteMock}
        refreshDocumentList={renderOptions?.refreshDocumentList || noop}
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

  it('renders as expected', async () => {
    const { asFragment } = setup();
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('should have the Documents type in the component', () => {
    const { getByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: 0,
      relationshipType: DocumentRelationshipType.FILES,
      documentResults: mockDocumentsResponse(),
      onDelete: deleteMock,
      refreshDocumentList: noop,
    });
    expect(getByTestId('document-type')).toBeInTheDocument();
  });

  it('should have the Documents add button in the component', () => {
    mockKeycloak({ claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE] });
    const { getByText } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: 0,
      relationshipType: DocumentRelationshipType.FILES,
      documentResults: mockDocumentsResponse(),
      onDelete: deleteMock,
      refreshDocumentList: noop,
    });
    expect(getByText('Add a Document')).toBeInTheDocument();
  });
});

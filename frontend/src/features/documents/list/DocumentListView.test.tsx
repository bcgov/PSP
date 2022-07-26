import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { DocumentTypes } from 'constants/documentTypes';
import { mockLookups } from 'mocks';
import { mockDocumentsResponse, mockDocumentTypesResponse } from 'mocks/mockDocuments';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { cleanup, render, RenderOptions, waitFor } from 'utils/test-utils';

import { DocumentListView, IDocumentListViewProps } from './DocumentListView';

const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

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
        documentType={DocumentTypes.FILE}
        entityId={0}
        documentResults={mockDocumentsResponse()}
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
    mockAxios.onGet(`documents/document-types`).reply(200, mockDocumentTypesResponse());
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

  it('should have the Documents type in the component', () => {
    const { getByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      documentType: DocumentTypes.FILE,
      entityId: 0,
      documentResults: mockDocumentsResponse(),
    });
    expect(getByTestId('document-type')).toBeInTheDocument();
  });

  it('should not have the Documents status in the component', () => {
    const { getByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      documentType: DocumentTypes.FILE,
      entityId: 0,
      documentResults: mockDocumentsResponse(),
    });
    expect(getByTestId('document-type')).toBeInTheDocument();
  });
});

import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import noop from 'lodash/noop';

import Claims from '@/constants/claims';
import { mockDocumentsResponse, mockDocumentTypesResponse } from '@/mocks/documents.mock';
import { mockLookups } from '@/mocks/index.mock';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  cleanup,
  render,
  RenderOptions,
  screen,
  userEvent,
  waitFor,
} from '@/utils/test-utils';

import { DocumentRow } from '../ComposedDocument';
import { DocumentListView, IDocumentListViewProps } from './DocumentListView';

const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const deleteMock = vi.fn().mockResolvedValue(true);

const mockDocumentRowResponse = () =>
  mockDocumentsResponse().map(x => (x?.document ? DocumentRow.fromApi(x) : new DocumentRow()));

describe('Document List View', () => {
  const setup = (renderOptions?: RenderOptions & IDocumentListViewProps) => {
    // render component under test
    const component = render(
      <DocumentListView
        isLoading={false}
        parentId={renderOptions?.parentId.toString() || '0'}
        relationshipType={
          renderOptions?.relationshipType || ApiGen_CodeTypes_DocumentRelationType.ResearchFiles
        }
        documentResults={
          renderOptions?.documentResults ||
          mockDocumentsResponse().map(x =>
            x?.document ? DocumentRow.fromApiDocument(x.document) : new DocumentRow(),
          )
        }
        onDelete={renderOptions?.onDelete || deleteMock}
        onSuccess={renderOptions?.onSuccess || noop}
      />,
      {
        ...renderOptions,
        store: storeState,
        useMockAuthentication: true,
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
    vi.restoreAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    const fragment = await waitFor(() => asFragment());
    await act(async () => expect(fragment).toMatchSnapshot());
  });

  it('should have the Documents type in the component', async () => {
    const { getByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: '0',
      relationshipType: ApiGen_CodeTypes_DocumentRelationType.ResearchFiles,
      documentResults: mockDocumentRowResponse(),
      onDelete: deleteMock,
      onSuccess: noop,
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    await act(async () => expect(getByTestId('document-type')).toBeInTheDocument());
  });

  it('should have the Documents filename in the component', async () => {
    const { getByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: '0',
      relationshipType: ApiGen_CodeTypes_DocumentRelationType.ResearchFiles,
      documentResults: mockDocumentRowResponse(),
      onDelete: deleteMock,
      onSuccess: noop,
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    await act(async () => expect(getByTestId('document-filename')).toBeInTheDocument());
  });

  it('should have the Documents add button in the component', async () => {
    const { getByText } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: '0',
      relationshipType: ApiGen_CodeTypes_DocumentRelationType.ResearchFiles,
      documentResults: mockDocumentRowResponse(),
      onDelete: deleteMock,
      onSuccess: noop,
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    await act(async () => expect(getByText('Add a Document')).toBeInTheDocument());
  });

  it('should not display the download icon on the listview', async () => {
    mockAxios.onGet().reply(200, []);
    const documentRows = mockDocumentRowResponse();
    documentRows[0].isFileAvailable = true;
    const { queryByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: '0',
      relationshipType: ApiGen_CodeTypes_DocumentRelationType.ResearchFiles,
      documentResults: documentRows,
      onDelete: deleteMock,
      onSuccess: noop,
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    const downloadButtonTooltip = await queryByTestId('document-download-button');
    await act(async () => expect(downloadButtonTooltip).toBeNull());
  });

  it('should call on delete for a document when the document id does not equal the document relationship id', async () => {
    const documentRows = mockDocumentRowResponse();
    documentRows[0].isFileAvailable = true;
    const { findAllByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: '0',
      relationshipType: ApiGen_CodeTypes_DocumentRelationType.ResearchFiles,
      documentResults: documentRows,
      onDelete: deleteMock,
      onSuccess: noop,
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    const deleteButtonTooltip = await findAllByTestId('document-delete-button');
    await act(async () => userEvent.click(deleteButtonTooltip[0]));

    await waitFor(() => screen.getByText('Yes'));
    const continueButton = screen.getByText('Yes');
    await act(async () => userEvent.click(continueButton));

    expect(deleteMock).toHaveBeenCalledWith(DocumentRow.toApi(documentRows[0]));
  });
});

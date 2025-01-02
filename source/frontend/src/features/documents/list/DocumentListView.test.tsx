import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import Claims from '@/constants/claims';
import {
  mockDocumentBatchUploadResponse,
  mockDocumentsResponse,
  mockDocumentTypesResponse,
} from '@/mocks/documents.mock';
import { mockLookups } from '@/mocks/index.mock';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  cleanup,
  getByName,
  render,
  RenderOptions,
  screen,
  userEvent,
} from '@/utils/test-utils';

import { DocumentRow } from '../ComposedDocument';
import { DocumentListView, IDocumentListViewProps } from './DocumentListView';

const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onDelete = vi.fn().mockResolvedValue(true);
const onSuccess = vi.fn();
const onRefresh = vi.fn();

const mockDocumentRowResponse = () =>
  mockDocumentsResponse().map(x => (x?.document ? DocumentRow.fromApi(x) : new DocumentRow()));

describe('Document List View', () => {
  // render component under test
  const setup = (renderOptions?: RenderOptions & Partial<IDocumentListViewProps>) => {
    const utils = render(
      <DocumentListView
        isLoading={false}
        parentId={renderOptions?.parentId?.toString() || '1'}
        relationshipType={
          renderOptions?.relationshipType || ApiGen_CodeTypes_DocumentRelationType.ResearchFiles
        }
        documentResults={
          renderOptions?.documentResults ||
          mockDocumentsResponse().map(x =>
            x?.document ? DocumentRow.fromApiDocument(x.document) : new DocumentRow(),
          )
        }
        onDelete={renderOptions?.onDelete || onDelete}
        onSuccess={renderOptions?.onSuccess || onSuccess}
        onRefresh={renderOptions?.onRefresh || onRefresh}
      />,
      {
        ...renderOptions,
        store: storeState,
        useMockAuthentication: true,
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    mockAxios.reset();
    mockAxios.onGet(`documents/types`).reply(200, mockDocumentTypesResponse());
    mockAxios
      .onGet(`documents/categories/${ApiGen_CodeTypes_DocumentRelationType.ResearchFiles}`)
      .reply(200, mockDocumentTypesResponse());
    mockAxios
      .onPost(`documents/upload/${ApiGen_CodeTypes_DocumentRelationType.ResearchFiles}/1`)
      .reply(200, mockDocumentBatchUploadResponse());
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
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('should have the Documents type in the component', async () => {
    const { getByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: '1',
      relationshipType: ApiGen_CodeTypes_DocumentRelationType.ResearchFiles,
      documentResults: mockDocumentRowResponse(),
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    expect(getByTestId('document-type')).toBeInTheDocument();
  });

  it('should have the Documents filename in the component', async () => {
    const { getByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: '1',
      relationshipType: ApiGen_CodeTypes_DocumentRelationType.ResearchFiles,
      documentResults: mockDocumentRowResponse(),
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    expect(getByTestId('document-filename')).toBeInTheDocument();
  });

  it('should have the Documents "Add button" in the component', async () => {
    const { getByText } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: '1',
      relationshipType: ApiGen_CodeTypes_DocumentRelationType.ResearchFiles,
      documentResults: mockDocumentRowResponse(),
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    expect(getByText('Add Document')).toBeInTheDocument();
  });

  it('should have the Documents "Refresh button" in the component', async () => {
    const { getByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: '1',
      relationshipType: ApiGen_CodeTypes_DocumentRelationType.ResearchFiles,
      documentResults: mockDocumentRowResponse(),
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    expect(getByTestId('refresh-button')).toBeInTheDocument();
  });

  it('should not display the download icon on the listview', async () => {
    mockAxios.onGet().reply(200, []);
    const documentRows = mockDocumentRowResponse();
    documentRows[0].isFileAvailable = true;

    const { queryByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: '1',
      relationshipType: ApiGen_CodeTypes_DocumentRelationType.ResearchFiles,
      documentResults: documentRows,
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });

    const downloadButtonTooltip = await queryByTestId('document-download-button');
    expect(downloadButtonTooltip).toBeNull();
  });

  it('should call onDelete for a document when the document id does not equal the document relationship id', async () => {
    const documentRows = mockDocumentRowResponse();
    documentRows[0].isFileAvailable = true;
    const { findAllByTestId } = setup({
      hideFilters: false,
      isLoading: false,
      parentId: '1',
      relationshipType: ApiGen_CodeTypes_DocumentRelationType.ResearchFiles,
      documentResults: documentRows,
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });
    const deleteButtonTooltip = await findAllByTestId('document-delete-button');
    await act(async () => userEvent.click(deleteButtonTooltip[0]));

    const continueButton = await screen.findByText('Yes');
    expect(continueButton).toBeVisible();
    await act(async () => userEvent.click(continueButton));

    expect(onDelete).toHaveBeenCalledWith(DocumentRow.toApi(documentRows[0]));
  });

  it('should call onSuccess when the upload modal is closed to refresh the list of documents', async () => {
    const documentRows = mockDocumentRowResponse();
    documentRows[0].isFileAvailable = true;

    setup({
      hideFilters: false,
      isLoading: false,
      parentId: '1',
      relationshipType: ApiGen_CodeTypes_DocumentRelationType.ResearchFiles,
      documentResults: documentRows,
      claims: [Claims.DOCUMENT_ADD, Claims.DOCUMENT_DELETE, Claims.DOCUMENT_VIEW],
    });

    // launch the document upload modal
    const addButton = await screen.findByText('Add Document');
    await act(async () => userEvent.click(addButton));

    expect(await screen.findByText(/Drag files here to attach/i)).toBeVisible();

    // get the upload button
    const uploader = screen.getByTestId('upload-input');

    // simulate upload event and wait until it finishes
    const file = new File(['hello'], 'hello.png', { type: 'image/png' });
    await act(async () => {
      userEvent.upload(uploader, [file]);
    });

    expect(
      await screen.findByText(/You have attached 1 files. Do you want to proceed/i),
    ).toBeVisible();

    await act(async () => {
      userEvent.selectOptions(getByName('documents.0.documentTypeId'), '1');
    });

    const continueButton = await screen.findByText('Yes');
    expect(continueButton).toBeVisible();
    await act(async () => userEvent.click(continueButton));

    const closeButton = await screen.findByText('Close');
    expect(closeButton).toBeVisible();
    await act(async () => userEvent.click(closeButton));

    expect(onSuccess).toHaveBeenCalled();
  });
});

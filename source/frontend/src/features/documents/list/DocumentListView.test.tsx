import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import Claims from '@/constants/claims';
import { mockDocumentBatchUploadResponse, mockDocumentsResponse } from '@/mocks/documents.mock';
import { mockDocumentTypesResponse } from '@/mocks/documentTypes.mock';
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
  waitForEffects,
} from '@/utils/test-utils';

import { DocumentRow } from '../models/DocumentRow';
import { DocumentListView, IDocumentListViewProps } from './DocumentListView';
import { ApiGen_CodeTypes_DocumentQueueStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_DocumentQueueStatusTypes';

const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onDelete = vi.fn().mockResolvedValue(true);
const onSuccess = vi.fn();
const onRefresh = vi.fn();
const onViewParent = vi.fn();

const mockDocumentRowResponse = () =>
  mockDocumentsResponse().map(x =>
    x?.document
      ? DocumentRow.fromApi(x, ApiGen_CodeTypes_DocumentRelationType.Leases)
      : new DocumentRow(),
  );

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
        relationshipTypes={[]}
        showParentInformation={false}
        onViewParent={renderOptions?.onViewParent || onViewParent}
        canEditDocuments={renderOptions?.canEditDocuments ?? true}
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
    const deleteButtonTooltip = await findAllByTestId('document-delete-button-0');
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
    await waitForEffects();

    await act(async () => {
      userEvent.selectOptions(getByName('documents.0.documentTypeId'), '1');
    });

    expect(
      await screen.findByText(/You have attached 1 files. Do you want to continue/i),
    ).toBeVisible();

    const continueButton = screen.getByTitle('ok-modal');
    expect(continueButton).toBeVisible();
    await act(async () => userEvent.click(continueButton));

    const closeButton = await screen.findByText('Close');
    expect(closeButton).toBeVisible();
    await act(async () => userEvent.click(closeButton));

    expect(onSuccess).toHaveBeenCalled();
  });


it('does not allow editing document details when canEditDocuments is false', async () => {
  setup({
    relationshipType: ApiGen_CodeTypes_DocumentRelationType.DispositionFiles,
    documentResults: mockDocumentRowResponse(),
    canEditDocuments: false,
    claims: [
      Claims.DOCUMENT_VIEW,
      Claims.DOCUMENT_EDIT,
      Claims.DOCUMENT_ADD,
      Claims.DOCUMENT_DELETE,
    ],
  });

  // Cannot add documents
  expect(screen.queryByTestId('add-document-btn')).not.toBeInTheDocument();

  // Cannot delete documents
  expect(screen.queryByTestId('document-delete-button-0')).not.toBeInTheDocument();

  // Can still view document details
  expect(screen.getByTestId('document-view-button-0')).toBeInTheDocument();
});


it('hides Delete for documents with upload errors when editing is disabled', () => {
  const documentRows = mockDocumentRowResponse();

  // Use the actual error queue status code from your generated enum.
  documentRows[0].queueStatusTypeCode = {
    id: ApiGen_CodeTypes_DocumentQueueStatusTypes.PIMS_ERROR,
    description: 'Error',
    isDisabled: false,
    displayOrder: null,
  };

  setup({
    relationshipType: ApiGen_CodeTypes_DocumentRelationType.DispositionFiles,
    documentResults: documentRows,
    canEditDocuments: false,
    claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_DELETE],
  });

  expect(screen.queryByTestId('document-delete-button-0')).not.toBeInTheDocument();
});


it('shows document editing controls when editing is enabled', () => {
  const documentRows = mockDocumentRowResponse();

  setup({
    relationshipType: ApiGen_CodeTypes_DocumentRelationType.AcquisitionFiles,
    documentResults: documentRows,
    canEditDocuments: true,
    claims: [
      Claims.DOCUMENT_VIEW,
      Claims.DOCUMENT_ADD,
      Claims.DOCUMENT_EDIT,
      Claims.DOCUMENT_DELETE,
    ],
  });

  expect(screen.getByTestId('add-document-btn')).toBeInTheDocument();
  expect(screen.getByTestId('document-delete-button-0')).toBeInTheDocument();
});


it('allows viewing document details without allowing edits', async () => {
  const documentRows = mockDocumentRowResponse();
  documentRows[0].documentType = {
  ...documentRows[0].documentType,
  mayanId: 123,
};

  setup({
    relationshipType: ApiGen_CodeTypes_DocumentRelationType.DispositionFiles,
    documentResults: documentRows,
    canEditDocuments: false,
    claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_EDIT],
  });

  await act(async () => {
    await userEvent.click(screen.getByTestId('document-view-button-0'));
  });

  expect(await screen.findByText('View Document Information')).toBeVisible();

  expect(screen.queryByTitle('Edit document information')).not.toBeInTheDocument();
});


});

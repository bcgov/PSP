import {
  mockDocumentBatchUploadResponse,
  mockDocumentTypesAcquisition,
} from '@/mocks/documents.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, getByName, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import { DocumentUploadModal, IDocumentUploadModalProps } from './DocumentUploadModal';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onUploadSuccess = vi.fn();
const onClose = vi.fn();

const mockDocumentApi = {
  retrieveDocumentMetadata: vi.fn(),
  retrieveDocumentMetadataLoading: false,
  downloadWrappedDocumentFile: vi.fn(),
  downloadWrappedDocumentFileLoading: false,
  downloadWrappedDocumentFileLatest: vi.fn(),
  downloadWrappedDocumentFileLatestLoading: false,
  downloadWrappedDocumentFileLatestResponse: null,
  streamDocumentFile: vi.fn(),
  streamDocumentFileLoading: false,
  streamDocumentFileLatest: vi.fn(),
  streamDocumentFileLatestLoading: false,
  streamDocumentFileLatestResponse: null,
  retrieveDocumentTypeMetadata: vi.fn(),
  retrieveDocumentTypeMetadataLoading: false,
  getDocumentTypes: vi.fn(),
  getDocumentTypesLoading: false,
  getDocumentRelationshipTypes: vi.fn(),
  getDocumentRelationshipTypesLoading: false,
  retrieveDocumentDetail: vi.fn(),
  retrieveDocumentDetailLoading: false,
  updateDocument: vi.fn(),
  updateDocumentLoading: false,
  downloadDocumentFilePageImage: vi.fn(),
  downloadDocumentFilePageImageLoading: false,
  getDocumentFilePageList: vi.fn(),
  getDocumentFilePageListLoading: false,
};

vi.mock('@/features/documents/hooks/useDocumentProvider');
vi.mocked(useDocumentProvider).mockReturnValue(mockDocumentApi);

const mockDocumentRelationshipApi = {
  deleteDocumentRelationship: vi.fn(),
  deleteDocumentRelationshipLoading: false,
  retrieveDocumentRelationship: vi.fn(),
  retrieveDocumentRelationshipLoading: false,
  uploadDocument: vi.fn(),
  uploadDocumentLoading: false,
};

vi.mock('@/features/documents/hooks/useDocumentRelationshipProvider');
vi.mocked(useDocumentRelationshipProvider).mockReturnValue(mockDocumentRelationshipApi);

describe('DocumentUploadModal component', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IDocumentUploadModalProps> } = {},
  ) => {
    const utils = render(
      <DocumentUploadModal
        parentId="1"
        relationshipType={
          renderOptions?.props?.relationshipType ??
          ApiGen_CodeTypes_DocumentRelationType.AcquisitionFiles
        }
        onUploadSuccess={renderOptions?.props?.onUploadSuccess ?? onUploadSuccess}
        onClose={renderOptions?.props?.onClose ?? onClose}
        maxDocumentCount={renderOptions?.props?.maxDocumentCount ?? 10}
        display={renderOptions?.props?.display ?? true}
        setDisplay={vi.fn()}
      />,
      {
        ...renderOptions,
        store: storeState,
      },
    );

    return { ...utils };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders correctly', async () => {
    setup();
    await act(async () => {});
    expect(document.body).toMatchSnapshot();
  });

  it(`closes the document upload modal when there are no changes`, async () => {
    setup();
    await act(async () => {});

    const cancelButton = screen.getByTitle('cancel-modal');
    await act(async () => userEvent.click(cancelButton));

    expect(screen.queryByText(/Unsaved updates will be lost/i)).toBeNull();
    expect(onClose).toHaveBeenCalled();
  });

  it(`doesn't close the upload modal when there are unsaved changes, showing a warning message instead`, async () => {
    setup();
    await act(async () => {});

    const files = [
      new File(['hello'], 'hello.png', { type: 'image/png' }),
      new File(['there'], 'there.png', { type: 'image/png' }),
    ];

    // get the upload button - simulate upload event and wait for it to finish
    const uploader = screen.getByTestId('upload-input');
    await act(async () => userEvent.upload(uploader, files));

    const cancelButton = screen.getByTitle('cancel-modal');
    await act(async () => userEvent.click(cancelButton));

    expect(await screen.findByText(/Unsaved updates will be lost/i)).toBeVisible();
    expect(onClose).not.toHaveBeenCalled();
  });

  it(`closes the upload modal after user clicks CANCEL a second time to confirm loosing unsaved changes`, async () => {
    setup();
    await act(async () => {});

    const files = [
      new File(['hello'], 'hello.png', { type: 'image/png' }),
      new File(['there'], 'there.png', { type: 'image/png' }),
    ];

    // get the upload button - simulate upload event and wait for it to finish
    const uploader = screen.getByTestId('upload-input');
    await act(async () => userEvent.upload(uploader, files));

    const cancelButton = screen.getByTitle('cancel-modal');
    await act(async () => userEvent.click(cancelButton));

    expect(await screen.findByText(/Unsaved updates will be lost/i)).toBeVisible();
    expect(onClose).not.toHaveBeenCalled();

    // click CANCEL again to confirm
    await act(async () => userEvent.click(cancelButton));
    expect(onClose).toHaveBeenCalled();
  });

  it('shows file upload results after successfully uploading files to mayan', async () => {
    mockDocumentApi.getDocumentRelationshipTypes.mockResolvedValue(mockDocumentTypesAcquisition());
    mockDocumentRelationshipApi.uploadDocument.mockResolvedValue(
      mockDocumentBatchUploadResponse()[0],
    );
    setup();
    await act(async () => {});

    const files = [new File(['hello'], 'hello.png', { type: 'image/png' })];

    // get the upload button - simulate upload event and wait for it to finish
    const uploader = screen.getByTestId('upload-input');
    await act(async () => userEvent.upload(uploader, files));

    const documentTypeDropdown = getByName('documents.0.documentTypeId') as HTMLSelectElement;
    await act(async () => userEvent.selectOptions(documentTypeDropdown, 'Correspondence'));

    const okButton = screen.getByTitle('ok-modal');
    await act(async () => userEvent.click(okButton));

    expect(await screen.findByText(/1 files successfully uploaded/i)).toBeVisible();
    expect(await screen.findByText('hello.png')).toBeVisible();
    expect(onClose).not.toHaveBeenCalled();
  });
});

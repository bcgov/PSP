import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, waitFor, screen, userEvent, act } from '@/utils/test-utils';
import { vi } from 'vitest';
import {
  IDocumentPreviewContainerProps,
  DocumentPreviewContainer,
} from './DocumentPreviewContainer';
import { IDocumentPreviewViewProps } from './DocumentPreviewView';
import { useDocumentProvider } from './hooks/useDocumentProvider';
import { DocumentViewerContextProvider } from './context/DocumentViewerContext';
import moment from 'moment';
import { HttpStatusCode } from 'axios';
import { ApiGen_System_HttpStatusCode } from '@/models/api/generated/ApiGen_System_HttpStatusCode';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const getDocumentFilePageList = vi.fn();
const retrieveDocumentDetail = vi.fn();
const downloadDocumentFilePageImage = vi.fn();

vi.mock('@/features/documents/hooks/useDocumentProvider');
vi.mocked(useDocumentProvider).mockImplementation(() => ({
  getDocumentFilePageList: getDocumentFilePageList,
  retrieveDocumentMetadata: vi.fn(),
  retrieveDocumentMetadataLoading: false,
  downloadWrappedDocumentFile: vi.fn(),
  downloadWrappedDocumentFileLoading: false,
  downloadWrappedDocumentFileLatest: vi.fn(),
  downloadWrappedDocumentFileLatestLoading: false,
  streamWrappedDocumentFile: vi.fn(),
  streamWrappedDocumentFileLoading: false,
  streamWrappedDocumentFileLatest: vi.fn(),
  streamWrappedDocumentFileLatestLoading: false,
  streamDocumentFile: vi.fn(),
  streamDocumentFileLoading: false,
  streamDocumentFileLatest: vi.fn(),
  streamDocumentFileLatestLoading: false,
  streamDocumentFileLatestResponse: null,
  retrieveDocumentTypeMetadata: vi.fn(),
  retrieveDocumentTypeMetadataLoading: false,
  getDocumentTypes: vi.fn(),
  getDocumentTypesLoading: false,
  downloadWrappedDocumentFileLatestResponse: null,
  retrieveDocumentTypeMetadataResponse: null,
  getDocumentTypesResponse: null,
  getDocumentFilePageListLoading: false,
  getDocumentRelationshipTypes: vi.fn(),
  getDocumentRelationshipTypesLoading: false,
  retrieveDocumentDetail: retrieveDocumentDetail,
  retrieveDocumentDetailLoading: false,
  updateDocument: vi.fn(),
  updateDocumentLoading: false,
  downloadDocumentFilePageImage: downloadDocumentFilePageImage,
  downloadDocumentFilePageImageLoading: false,
}));

const createFileUrl = vi.fn();

describe('DocumentPreviewContainer component', () => {
  // render component under test

  let viewProps: IDocumentPreviewViewProps;
  const View: React.FC<IDocumentPreviewViewProps> = (props, ref) => {
    viewProps = props;
    return <></>;
  };

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IDocumentPreviewContainerProps> },
  ) => {
    const utils = render(
      <DocumentViewerContextProvider
        {...{
          showDocumentPreview: true,
          previewDocumentId: 1,
          setShowDocumentPreview: vi.fn(),
          setPreviewDocumentId: vi.fn(),
        }}
      >
        <DocumentPreviewContainer
          {...renderOptions.props}
          View={View}
          createFileUrl={createFileUrl}
        />
      </DocumentViewerContextProvider>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {});

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('Displays an error if the document detail request fails', async () => {
    setup({});

    await screen.findByText('Failed to retrieve document details.', { exact: false });

    const ok = screen.getByText('Ok');

    await waitFor(async () => {
      userEvent.click(ok);
      expect(viewProps.showDocumentPreview).toBe(false);
    });
  });

  it('Displays a warning if the document was recently uploaded', async () => {
    retrieveDocumentDetail.mockResolvedValue({
      payload: { datetime_created: new Date().toISOString() },
    });
    setup({});
    await waitFor(async () => {
      await screen.findByText('Document Still Processing');
    });
    const ok = screen.getByText('Ok');

    await waitFor(async () => {
      userEvent.click(ok);
      expect(viewProps.showDocumentPreview).toBe(false);
    });
  });

  it('Displays an error if the document page list cannot be retrieved', async () => {
    retrieveDocumentDetail.mockResolvedValue({
      payload: {
        datetime_created: moment().subtract(2, 'minutes').toISOString(),
        file_latest: { id: 1 },
      },
      httpStatusCode: ApiGen_System_HttpStatusCode.OK,
    });
    setup({});
    await waitFor(async () => {
      await screen.findByText('Unable to retrieve preview pages for this document.', {
        exact: false,
      });
    });

    const ok = screen.getByText('Ok');

    await waitFor(async () => {
      userEvent.click(ok);
      expect(viewProps.showDocumentPreview).toBe(false);
    });
  });

  it('Passes the list of pages', async () => {
    retrieveDocumentDetail.mockResolvedValue({
      payload: {
        datetime_created: moment().subtract(2, 'minutes').toISOString(),
        file_latest: { id: 1 },
      },
      httpStatusCode: ApiGen_System_HttpStatusCode.OK,
    });
    getDocumentFilePageList.mockResolvedValue([
      {
        id: 1,
        document_file_id: 1,
        document_file_url: 'url',
        image_url: 'url',
        page_number: '1',
        url: 'url',
      },
    ]);
    downloadDocumentFilePageImage.mockResolvedValue('url');
    createFileUrl.mockReturnValue('http://example.com');
    setup({});
    await waitFor(async () => {
      expect(viewProps.pages).toHaveLength(1);
    });
  });

  it('Can change the page number', async () => {
    retrieveDocumentDetail.mockResolvedValue({
      payload: {
        datetime_created: moment().subtract(2, 'minutes').toISOString(),
        file_latest: { id: 1 },
      },
      httpStatusCode: ApiGen_System_HttpStatusCode.OK,
    });
    getDocumentFilePageList.mockResolvedValue([
      {
        id: 1,
        document_file_id: 1,
        document_file_url: 'url',
        image_url: 'url',
        page_number: '1',
        url: 'url',
      },
      {
        id: 2,
        document_file_id: 2,
        document_file_url: 'url',
        image_url: 'url',
        page_number: '2',
        url: 'url',
      },
    ]);
    downloadDocumentFilePageImage.mockResolvedValue('url');
    createFileUrl.mockReturnValue('http://example.com');
    setup({});
    await waitFor(async () => {
      expect(viewProps.pages).toHaveLength(2);
    });
    await act(async () => {
      viewProps.setCurrentPage(1);
    });
    await waitFor(async () => {
      expect(viewProps.currentPage).toBe(1);
      expect(viewProps.pages[1].loadedDocumentImageDataUrl).toBe('http://example.com');
    });
  });
});

import { createMemoryHistory } from 'history';
import React, { createRef } from 'react';

import { useDocumentProvider } from '@/features/documents/hooks/useDocumentProvider';
import { useDocumentRelationshipProvider } from '@/features/documents/hooks/useDocumentRelationshipProvider';
import {
  mockDocumentBatchUploadResponse,
  mockDocumentTypesAcquisition,
  mockDocumentTypesAll,
} from '@/mocks/documents.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen } from '@/utils/test-utils';

import DocumentUploadContainer, {
  IDocumentUploadContainerProps,
  IDocumentUploadContainerRef,
} from './DocumentUploadContainer';
import { IDocumentUploadFormProps } from './DocumentUploadForm';
import { BatchUploadFormModel, DocumentUploadFormData } from '../ComposedDocument';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onUploadSuccess = vi.fn();
const onCancel = vi.fn();
const setCanUpload = vi.fn();

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

describe('DocumentUploadContainer component', () => {
  // render component under test

  let viewProps: IDocumentUploadFormProps | undefined;
  const View: React.FC<IDocumentUploadFormProps> = props => {
    viewProps = props;
    return <>Content Rendered</>;
  };

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IDocumentUploadContainerProps> } = {},
  ) => {
    const uploadContainerRef = createRef<IDocumentUploadContainerRef>();

    const utils = render(
      <DocumentUploadContainer
        ref={uploadContainerRef}
        parentId="1"
        relationshipType={
          renderOptions?.props?.relationshipType ??
          ApiGen_CodeTypes_DocumentRelationType.AcquisitionFiles
        }
        onUploadSuccess={renderOptions?.props?.onUploadSuccess ?? onUploadSuccess}
        onCancel={renderOptions?.props?.onCancel ?? onCancel}
        setCanUpload={renderOptions?.props?.setCanUpload ?? setCanUpload}
        maxDocumentCount={renderOptions?.props?.maxDocumentCount ?? 1}
        View={View}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
      uploadContainerRef,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the underlying form', () => {
    setup();
    expect(screen.getByText(/content rendered/i)).toBeVisible();
  });

  it('should call the api to fetch document types', async () => {
    mockDocumentApi.getDocumentRelationshipTypes.mockResolvedValue(mockDocumentTypesAcquisition());
    setup();

    await act(async () => {});
    expect(mockDocumentApi.getDocumentRelationshipTypes).toHaveBeenCalled();
    expect(viewProps?.documentTypes?.length).toBeGreaterThan(0);
    expect(viewProps?.documentStatusOptions?.length).toBeGreaterThan(0);
  });

  it('should call the api to fetch document types for CDOGS templates', async () => {
    mockDocumentApi.getDocumentTypes.mockResolvedValue(mockDocumentTypesAll());
    setup({ props: { relationshipType: ApiGen_CodeTypes_DocumentRelationType.Templates } });

    await act(async () => {});
    expect(mockDocumentApi.getDocumentTypes).toHaveBeenCalled();
    expect(viewProps?.documentTypes?.length).toBeGreaterThan(0);
    expect(viewProps?.documentStatusOptions?.length).toBeGreaterThan(0);
  });

  it.each([
    ['document count <= max documents', 1, 10, true],
    ['document count > max documents', 12, 10, false],
  ])(
    'should setCanUpload when documents are selected for upload - %s',
    async (_: string, documentCount: number, maxDocuments: number, expectedValue: boolean) => {
      setup({ props: { maxDocumentCount: maxDocuments } });

      await act(async () => {
        viewProps?.onDocumentsSelected(documentCount);
      });
      expect(setCanUpload).toHaveBeenCalledWith(expectedValue);
    },
  );

  it('should call uploadDocument API and report the result of file upload operation', async () => {
    mockDocumentRelationshipApi.uploadDocument.mockResolvedValue(mockDocumentBatchUploadResponse());
    setup();

    await act(async () => {});
    expect(viewProps?.documentTypes?.length).toBeGreaterThan(0);
    expect(viewProps?.documentStatusOptions?.length).toBeGreaterThan(0);

    await act(async () => {
      const formDocument = new DocumentUploadFormData(
        viewProps?.documentStatusOptions[0]?.value?.toString(),
        '',
        [],
      );
      formDocument.file = new File(['(⌐□_□)'], 'test.png', { type: 'image/png' });

      const batchRequest = new BatchUploadFormModel();
      batchRequest.documents.push(formDocument);

      // trigger mock file upload
      viewProps?.onUploadDocument(batchRequest);
    });

    expect(onUploadSuccess).toHaveBeenCalled();
  });

  it('should call mayan metadata API', async () => {
    setup();

    await act(async () => {});
    expect(viewProps?.documentTypes?.length).toBeGreaterThan(0);
    expect(viewProps?.documentStatusOptions?.length).toBeGreaterThan(0);

    await act(async () => {
      viewProps?.getDocumentMetadata(viewProps?.documentTypes[0]);
    });

    expect(mockDocumentApi.retrieveDocumentTypeMetadata).toHaveBeenCalled();
  });
});

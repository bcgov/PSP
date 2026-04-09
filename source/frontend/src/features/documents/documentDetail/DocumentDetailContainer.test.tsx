import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';
import { useDocumentProvider } from '@/features/documents/hooks/useDocumentProvider';
import {
  mockDocumentTypeMetadataBcAssessment,
  mockDocumentTypesAcquisition,
  mockDocumentTypesAll,
} from '@/mocks/documentTypes.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_System_HttpStatusCode } from '@/models/api/generated/ApiGen_System_HttpStatusCode';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, getByName, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { DocumentDetailContainer, IDocumentDetailContainerProps } from './DocumentDetailContainer';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onUpdateSuccess = vi.fn();

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

const mockDocumentAcquisitionRelationship: ApiGen_Concepts_DocumentRelationship = {
  id: 26,
  parentId: '64',
  parentNameOrNumber: '01-1-01',
  document: {
    id: 22,
    fileName: 'Form12_Carbone_Template.docx',
    documentType: {
      id: 41,
      documentType: 'BCASSE',
      documentTypeDescription: 'BC assessment search',
      documentTypePurpose: null,
      mayanId: 84,
      isDisabled: false,
      appCreateTimestamp: '2025-10-27T22:09:06.29',
      appLastUpdateTimestamp: '2025-10-27T22:12:24.617',
      appLastUpdateUserid: 'EHERRERA',
      appCreateUserid: 'EHERRERA',
      appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
      appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
      rowVersion: 2,
    },
    statusTypeCode: {
      id: 'FINAL',
      description: 'Final',
      isDisabled: false,
      displayOrder: 5,
    },
    documentQueueStatusTypeCode: {
      id: 'SUCCESS',
      description: 'Success',
      isDisabled: false,
      displayOrder: 5,
    },
    mayanDocumentId: 3,
    appCreateTimestamp: '2025-10-27T22:16:45.133',
    appLastUpdateTimestamp: '2025-10-27T22:17:00.783',
    appLastUpdateUserid: 'service',
    appCreateUserid: 'EHERRERA',
    appLastUpdateUserGuid: '00000000-0000-0000-0000-000000000000',
    appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
    rowVersion: 2,
  },
  relationshipType: ApiGen_CodeTypes_DocumentRelationType.AcquisitionFiles,
  appCreateTimestamp: '2025-10-27T22:16:45.2',
  appLastUpdateTimestamp: '2025-10-27T22:16:45.2',
  appLastUpdateUserid: 'EHERRERA',
  appCreateUserid: 'EHERRERA',
  appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
  appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
  rowVersion: 1,
};

vi.mock('@/features/documents/hooks/useDocumentProvider');
vi.mocked(useDocumentProvider).mockReturnValue(mockDocumentApi);

describe('DocumentDetailContainer component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IDocumentDetailContainerProps> } = {},
  ) => {
    const utils = render(
      <DocumentDetailContainer
        pimsDocumentRelationship={
          renderOptions?.props?.pimsDocumentRelationship ?? mockDocumentAcquisitionRelationship
        }
        relationshipType={
          renderOptions?.props?.relationshipType ??
          ApiGen_CodeTypes_DocumentRelationType.AcquisitionFiles
        }
        onUpdateSuccess={renderOptions?.props?.onUpdateSuccess ?? onUpdateSuccess}
        canEdit={renderOptions?.props?.canEdit ?? true}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
        claims: renderOptions.claims ?? [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_EDIT],
      },
    );

    return {
      ...utils,
      getDocumentTypeDropdown: () => getByName('documentTypeId') as HTMLSelectElement,
    };
  };

  beforeEach(() => {
    mockDocumentApi.getDocumentRelationshipTypes.mockResolvedValue(mockDocumentTypesAcquisition());
    mockDocumentApi.getDocumentTypes.mockResolvedValue(mockDocumentTypesAll());
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { asFragment } = setup();

    await act(async () => {});
    expect(screen.getByText(/Document Information/i)).toBeVisible();
    expect(asFragment()).toMatchSnapshot();
  });

  it('should call the api to fetch document types when in Edit mode', async () => {
    setup();

    await act(async () => {});
    await act(async () => {
      userEvent.click(screen.getByTitle(/Edit document information/i));
    });
    expect(mockDocumentApi.getDocumentRelationshipTypes).toHaveBeenCalled();
  });

  it('should call the api to fetch document types for CDOGS templates', async () => {
    setup({ props: { relationshipType: ApiGen_CodeTypes_DocumentRelationType.Templates } });

    await act(async () => {});
    await act(async () => {
      userEvent.click(screen.getByTitle(/Edit document information/i));
    });
    expect(mockDocumentApi.getDocumentTypes).toHaveBeenCalled();
  });

  it('should call the api to fetch mayan document type metadata', async () => {
    mockDocumentApi.retrieveDocumentTypeMetadata.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Success,
      message: null,
      payload: {
        results: mockDocumentTypeMetadataBcAssessment(),
      },
      httpStatusCode: ApiGen_System_HttpStatusCode.OK,
    });

    setup();

    await act(async () => {});
    expect(mockDocumentApi.retrieveDocumentTypeMetadata).toHaveBeenCalled();
  });

  it('should fetch additional document child entities upon rendering this component', async () => {
    mockDocumentApi.retrieveDocumentMetadata.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Success,
      message: null,
      payload: {
        results: [],
      },
      httpStatusCode: ApiGen_System_HttpStatusCode.OK,
    });
    mockDocumentApi.retrieveDocumentDetail.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Success,
      message: null,
      payload: {
        id: 1,
        label: null,
        datetime_created: new Date().toISOString(),
        description: null,
        uuid: null,
        file_latest: { id: 1 },
        document_type: null,
      },
      httpStatusCode: ApiGen_System_HttpStatusCode.OK,
    });

    setup();

    await act(async () => {});
    expect(mockDocumentApi.retrieveDocumentMetadata).toHaveBeenCalled();
    expect(mockDocumentApi.retrieveDocumentDetail).toHaveBeenCalled();
  });

  it('should refresh the document type metadata when document type is changed', async () => {
    mockDocumentApi.retrieveDocumentTypeMetadata.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Success,
      message: null,
      payload: {
        results: mockDocumentTypeMetadataBcAssessment(),
      },
      httpStatusCode: ApiGen_System_HttpStatusCode.OK,
    });
    vi.spyOn(console, 'error').mockImplementationOnce(() => {});

    const { getDocumentTypeDropdown } = setup();

    await act(async () => {});
    expect(mockDocumentApi.retrieveDocumentTypeMetadata).toHaveBeenCalled();
    await act(async () => {
      userEvent.click(screen.getByTitle(/Edit document information/i));
    });

    const documentType = getDocumentTypeDropdown();
    expect(documentType).toBeVisible();
    await act(async () => {
      userEvent.selectOptions(documentType, '17');
    });

    // document type metadata should be refreshed
    expect(mockDocumentApi.retrieveDocumentTypeMetadata).toHaveBeenCalled();
  });

  it('should submit the form as expected', async () => {
    mockDocumentApi.retrieveDocumentTypeMetadata.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Success,
      message: null,
      payload: {
        results: mockDocumentTypeMetadataBcAssessment(),
      },
      httpStatusCode: ApiGen_System_HttpStatusCode.OK,
    });
    mockDocumentApi.updateDocument.mockResolvedValue({
      metadataExternalResponse: [],
    });

    setup();

    await act(async () => {});
    expect(mockDocumentApi.retrieveDocumentTypeMetadata).toHaveBeenCalled();
    await act(async () => {
      userEvent.click(screen.getByTitle(/Edit document information/i));
    });

    const yesButton = screen.getByText('Yes');
    expect(yesButton).toBeVisible();
    await act(async () => {
      userEvent.click(yesButton);
    });

    // document update was called
    expect(onUpdateSuccess).toHaveBeenCalled();
  });
});

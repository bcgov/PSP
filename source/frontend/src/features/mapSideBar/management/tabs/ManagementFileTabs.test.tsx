import { createMemoryHistory } from 'history';
import { Route } from 'react-router-dom';

import Claims from '@/constants/claims';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';
import { act, getMockRepositoryObj, render, RenderOptions, userEvent } from '@/utils/test-utils';

import ManagementFileTabs, { IManagementFileTabsProps } from './ManagementFileTabs';

vi.mock('@/hooks/repositories/useNoteRepository');

const mockGetManagementFileOffersApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockGetManagementFileSalesApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockGetManagementFileAppraisalApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockDeleteManagementFileOfferApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockGetManagementPropertiesApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockGetManagementActivitiesApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mocked(useNoteRepository, { partial: true }).mockImplementation(() => ({
  getAllNotes: getMockRepositoryObj([]),
  addNote: getMockRepositoryObj(),
  getNote: getMockRepositoryObj(),
  updateNote: getMockRepositoryObj(),
  deleteNote: getMockRepositoryObj(),
}));

const mockRetrieveDocumentRelationship = vi.fn();

vi.mock('@/features/documents/hooks/useDocumentRelationshipProvider', () => ({
  useDocumentRelationshipProvider: () => {
    return {
      retrieveDocumentRelationship: mockRetrieveDocumentRelationship,
      retrieveDocumentRelationshipLoading: false,
    };
  },
}));

const mockGetDocumentRelationshipTypes = vi.fn();
vi.mock('@/features/documents/hooks/useDocumentProvider', () => ({
  useDocumentProvider: () => {
    return {
      getDocumentRelationshipTypes: mockGetDocumentRelationshipTypes,
      getDocumentRelationshipTypesLoading: false,
      getDocumentTypes: vi.fn(),
      getDocumentTypesLoading: false,
    };
  },
}));

vi.mock('@/hooks/repositories/useManagementFileRepository', () => ({
  useManagementFileRepository: () => {
    return {
      getManagementFileOffers: mockGetManagementFileOffersApi,
      getManagementFileSale: mockGetManagementFileSalesApi,
      deleteManagementOffer: mockDeleteManagementFileOfferApi,
      getManagementAppraisal: mockGetManagementFileAppraisalApi,
      getManagementProperties: mockGetManagementPropertiesApi,
      getAllManagementFileContacts: getMockRepositoryObj(),
      deleteManagementContact: getMockRepositoryObj(),
    };
  },
}));

vi.mock('@/hooks/repositories/useManagementActivityRepository', () => ({
  useManagementActivityRepository: () => {
    return {
      getManagementActivities: mockGetManagementActivitiesApi,
    };
  },
}));

const history = createMemoryHistory();
const setIsEditing = vi.fn();

const basePath = 'mapview/sidebar/management';

describe('ManagementFileTabs component', () => {
  // render component under test
  const setup = async (
    props: Omit<IManagementFileTabsProps, 'setIsEditing'>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <Route path={`/${basePath}/:fileId/:detailType`}>
        <ManagementFileTabs
          managementFile={props.managementFile}
          defaultTab={props.defaultTab}
          setIsEditing={setIsEditing}
        />
      </Route>,
      {
        useMockAuthentication: true,
        history,
        ...renderOptions,
      },
    );

    // wait for effects
    await act(async () => {});
    return { ...utils };
  };

  beforeEach(() => {
    history.replace(`/${basePath}/1/${FileTabType.FILE_DETAILS}`);
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = await setup(
      {
        managementFile: mockManagementFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW, Claims.NOTE_VIEW] },
    );
    expect(asFragment()).toMatchSnapshot();
  });

  it('has a documents tab', async () => {
    const { getByRole } = await setup(
      {
        managementFile: mockManagementFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const tab = getByRole('tab', { name: 'Documents' });
    expect(tab).toBeVisible();
  });

  it('documents tab can be changed to', async () => {
    const { getByRole } = await setup(
      {
        managementFile: mockManagementFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const tab = getByRole('tab', { name: 'Documents' });
    await act(async () => userEvent.click(tab));

    expect(history.location.pathname).toBe(`/${basePath}/1/${FileTabType.DOCUMENTS}`);
    expect(tab).toHaveClass('active');
  });

  it('has a notes tab', async () => {
    const { getByRole } = await setup(
      {
        managementFile: mockManagementFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.NOTE_VIEW] },
    );

    const tab = getByRole('tab', { name: 'Notes' });
    expect(tab).toBeVisible();
  });

  it.skip('notes tab can be changed to', async () => {
    const { getByRole } = await setup(
      {
        managementFile: mockManagementFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.NOTE_VIEW] },
    );

    const tab = getByRole('tab', { name: 'Notes' });
    await act(async () => userEvent.click(tab));

    expect(history.location.pathname).toBe(`/${basePath}/1/${FileTabType.NOTES}`);
    expect(tab).toHaveClass('active');
  });
});

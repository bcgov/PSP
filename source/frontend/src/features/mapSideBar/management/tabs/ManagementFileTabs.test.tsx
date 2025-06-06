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

vi.mocked(useNoteRepository, { partial: true }).mockImplementation(() => ({
  getAllNotes: getMockRepositoryObj([]),
  addNote: getMockRepositoryObj(),
  getNote: getMockRepositoryObj(),
  updateNote: getMockRepositoryObj(),
  deleteNote: getMockRepositoryObj(),
}));

vi.mock('@/features/documents/hooks/useDocumentRelationshipProvider', () => ({
  useDocumentRelationshipProvider: () => {
    return {
      retrieveDocumentRelationship: vi.fn(),
      retrieveDocumentRelationshipLoading: false,
    };
  },
}));

vi.mock('@/features/documents/hooks/useDocumentProvider', () => ({
  useDocumentProvider: () => {
    return {
      getDocumentRelationshipTypes: vi.fn(),
      getDocumentRelationshipTypesLoading: false,
      getDocumentTypes: vi.fn(),
      getDocumentTypesLoading: false,
    };
  },
}));

vi.mock('@/hooks/repositories/useManagementProvider', () => ({
  useManagementProvider: () => {
    return {
      getManagementFileOffers: mockGetManagementFileOffersApi,
      getManagementFileSale: mockGetManagementFileSalesApi,
      deleteManagementOffer: mockDeleteManagementFileOfferApi,
      getManagementAppraisal: mockGetManagementFileAppraisalApi,
      getManagementProperties: mockGetManagementPropertiesApi,
    };
  },
}));

const history = createMemoryHistory();
const setIsEditing = vi.fn();

describe('ManagementFileTabs component', () => {
  // render component under test
  const setup = async (
    props: Omit<IManagementFileTabsProps, 'setIsEditing'>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <Route path="/blah/:tab">
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
    history.replace(`/blah/${FileTabType.FILE_DETAILS}`);
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
    const { getByText } = await setup(
      {
        managementFile: mockManagementFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const tab = getByText('Documents');
    expect(tab).toBeVisible();
  });

  it.skip('documents tab can be changed to', async () => {
    const { getByText } = await setup(
      {
        managementFile: mockManagementFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const tab = getByText('Documents');
    await act(async () => userEvent.click(tab));

    expect(getByText('Documents')).toHaveClass('active');
    expect(history.location.pathname).toBe(`/blah/${FileTabType.DOCUMENTS}`);
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

  it('notes tab can be changed to', async () => {
    const { getByRole } = await setup(
      {
        managementFile: mockManagementFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.NOTE_VIEW] },
    );

    const tab = getByRole('tab', { name: 'Notes' });
    await act(async () => userEvent.click(tab));

    expect(tab).toHaveClass('active');
    expect(history.location.pathname).toBe(`/blah/${FileTabType.NOTES}`);
  });
});

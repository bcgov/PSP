import { createMemoryHistory } from 'history';
import { Route } from 'react-router-dom';
import { act } from 'react-test-renderer';

import Claims from '@/constants/claims';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { useApiNotes } from '@/hooks/pims-api/useApiNotes';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { render, RenderOptions, userEvent } from '@/utils/test-utils';

import DispositionFileTabs, { IDispositionFileTabsProps } from './DispositionFileTabs';

// mock auth library

vi.mock('@/hooks/repositories/useNoteRepository');
vi.mock('@/hooks/pims-api/useApiNotes');

const getNotes = vi.fn().mockResolvedValue([]);

const mockGetDispositionFileOffersApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockGetDispositionFileSalesApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockGetDispositionFileAppraisalApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockDeleteDispositionFileOfferApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mocked(useNoteRepository).mockImplementation(
  () =>
    ({
      addNote: { execute: vi.fn() },
      getNote: { execute: vi.fn() },
      updateNote: { execute: vi.fn() },
    } as unknown as ReturnType<typeof useNoteRepository>),
);

vi.mocked(useApiNotes).mockImplementation(
  () =>
    ({
      getNotes,
    } as unknown as ReturnType<typeof useApiNotes>),
);

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

vi.mock('@/hooks/repositories/useDispositionProvider', () => ({
  useDispositionProvider: () => {
    return {
      getDispositionFileOffers: mockGetDispositionFileOffersApi,
      getDispositionFileSale: mockGetDispositionFileSalesApi,
      deleteDispositionOffer: mockDeleteDispositionFileOfferApi,
      getDispositionAppraisal: mockGetDispositionFileAppraisalApi,
    };
  },
}));

const history = createMemoryHistory();
const setIsEditing = vi.fn();

const mockDispositionFileResponseApi = mockDispositionFileResponse();

describe('DispositionFileTabs component', () => {
  // render component under test
  const setup = (
    props: Omit<IDispositionFileTabsProps, 'setIsEditing'>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <Route path="/blah/:tab">
        <DispositionFileTabs
          dispositionFile={props.dispositionFile}
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

    return { ...utils };
  };

  beforeEach(() => {
    history.replace(`/blah/${FileTabType.FILE_DETAILS}`);
  });

  it('matches snapshot', () => {
    const { asFragment } = setup(
      {
        dispositionFile: mockDispositionFileResponseApi,
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );
    expect(asFragment()).toMatchSnapshot();
  });

  it('has a documents tab', () => {
    const { getByText } = setup(
      {
        dispositionFile: mockDispositionFileResponseApi,
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const tab = getByText('Documents');
    expect(tab).toBeVisible();
  });

  it('documents tab can be changed to', async () => {
    const { getByText } = setup(
      {
        dispositionFile: mockDispositionFileResponseApi,
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const tab = getByText('Documents');
    await act(async () => userEvent.click(tab));

    expect(getByText('Documents')).toHaveClass('active');
    expect(history.location.pathname).toBe(`/blah/${FileTabType.DOCUMENTS}`);
  });

  it('has an offers tab', () => {
    const { getByText } = setup(
      {
        dispositionFile: mockDispositionFileResponseApi,
        defaultTab: FileTabType.OFFERS_AND_SALE,
      },
      { claims: [] },
    );

    const tab = getByText('Offers & Sale');
    expect(tab).toBeVisible();
  });

  it('offers tab can be changed to', async () => {
    const { getByText } = setup(
      {
        dispositionFile: mockDispositionFileResponseApi,
        defaultTab: FileTabType.OFFERS_AND_SALE,
      },
      { claims: [] },
    );

    const tab = getByText('Offers & Sale');
    await act(async () => userEvent.click(tab));

    expect(getByText('Offers & Sale')).toHaveClass('active');
    expect(history.location.pathname).toBe(`/blah/${FileTabType.OFFERS_AND_SALE}`);
  });

  it('has a notes tab', () => {
    const { getAllByText } = setup(
      {
        dispositionFile: mockDispositionFileResponseApi,
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.NOTE_VIEW] },
    );

    const tab = getAllByText('Notes')[0];
    expect(tab).toBeVisible();
  });

  it('notes tab can be changed to', async () => {
    const { getAllByText } = setup(
      {
        dispositionFile: mockDispositionFileResponseApi,
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.NOTE_VIEW] },
    );

    const tab = getAllByText('Notes')[0];
    await act(async () => userEvent.click(tab));

    expect(tab).toHaveClass('active');
    expect(history.location.pathname).toBe(`/blah/${FileTabType.NOTES}`);
  });

  it('has a checklist tab', () => {
    const { getByText } = setup(
      {
        dispositionFile: mockDispositionFileResponseApi,
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [] },
    );

    const tab = getByText('Checklist');
    expect(tab).toBeVisible();
  });

  it('checklist tab can be changed to', async () => {
    const { getByText } = setup(
      {
        dispositionFile: mockDispositionFileResponseApi,
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [] },
    );

    const tab = getByText('Checklist');
    await act(async () => userEvent.click(tab));

    expect(getByText('Checklist')).toHaveClass('active');
    expect(history.location.pathname).toBe(`/blah/${FileTabType.CHECKLIST}`);
  });
});

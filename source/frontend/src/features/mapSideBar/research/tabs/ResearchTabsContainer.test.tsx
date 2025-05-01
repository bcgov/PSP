import { createMemoryHistory } from 'history';
import { Route } from 'react-router-dom';

import Claims from '@/constants/claims';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { act, getMockRepositoryObj, render, RenderOptions, userEvent } from '@/utils/test-utils';

import ResearchTabsContainer, { IResearchTabsContainerProps } from './ResearchTabsContainer';

const setIsEditing = vi.fn();
const history = createMemoryHistory();

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

vi.mock('@/hooks/repositories/useNoteRepository');
vi.mocked(useNoteRepository, { partial: true }).mockImplementation(() => ({
  getAllNotes: getMockRepositoryObj([]),
  addNote: getMockRepositoryObj(),
  getNote: getMockRepositoryObj(),
  updateNote: getMockRepositoryObj(),
  deleteNote: getMockRepositoryObj(),
}));

describe('ResearchFileTabs component', () => {
  // render component under test
  const setup = (props: IResearchTabsContainerProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <Route path="/blah/:tab">
        <ResearchTabsContainer
          researchFile={props.researchFile}
          setIsEditing={props.setIsEditing}
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

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup(
      {
        researchFile: getMockResearchFile(),
        setIsEditing,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );
    expect(asFragment()).toMatchSnapshot();
  });

  it('has a documents tab', () => {
    const { getByText } = setup(
      {
        researchFile: getMockResearchFile(),
        setIsEditing,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const tab = getByText('Documents');
    expect(tab).toBeVisible();
  });

  it('documents tab can be changed to', async () => {
    const { getByText } = setup(
      {
        researchFile: getMockResearchFile(),
        setIsEditing,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const tab = getByText('Documents');
    await act(async () => {
      userEvent.click(tab);
    });

    expect(tab).toHaveClass('active');
    expect(history.location.pathname).toBe(`/blah/${FileTabType.DOCUMENTS}`);
  });

  it('has a notes tab', async () => {
    const { getAllByText } = setup(
      {
        researchFile: getMockResearchFile(),
        setIsEditing,
      },
      { claims: [Claims.NOTE_VIEW] },
    );

    const tab = getAllByText('Notes')[0];
    expect(tab).toBeVisible();
  });

  it('notes tab can be changed to', async () => {
    const { getAllByText } = setup(
      {
        researchFile: getMockResearchFile(),
        setIsEditing,
      },
      { claims: [Claims.NOTE_VIEW] },
    );

    const tab = getAllByText('Notes')[0];
    await act(async () => {
      userEvent.click(tab);
    });

    expect(tab).toHaveClass('active');
    expect(history.location.pathname).toBe(`/blah/${FileTabType.NOTES}`);
  });
});

import Claims from '@/constants/claims';
import { mockProjectGetResponse } from '@/mocks/projects.mock';
import {
  cleanup,
  render,
  RenderOptions,
  screen,
  waitFor,
  waitForElementToBeRemoved,
} from '@/utils/test-utils';

import ProjectTabsContainer, { IProjectTabsContainerProps } from './ProjectTabsContainer';

// mock auth library

const setProjectMock = vi.fn();
const setEditModeMock = vi.fn();
const setContainerStateMock = vi.fn();

const mockProject = mockProjectGetResponse();

vi.mock('@/features/documents/hooks/useDocumentRelationshipProvider', () => ({
  useDocumentRelationshipProvider: () => {
    return {
      retrieveDocumentRelationship: vi.fn(),
      retrieveDocumentRelationshipLoading: false,
    };
  },
}));

describe('Project Tabs component', () => {
  // render component under test
  const setup = (props: IProjectTabsContainerProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <ProjectTabsContainer
        project={props.project}
        setProject={setProjectMock}
        setContainerState={setContainerStateMock}
        onEdit={setEditModeMock}
      />,
      {
        useMockAuthentication: true,
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  afterEach(() => {
    vi.clearAllMocks();
    cleanup();
  });

  it('matches snapshot', async () => {
    const { asFragment, getByTitle } = setup(
      {
        project: mockProject,
        setProject: setProjectMock,
        setContainerState: setContainerStateMock,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    expect(asFragment()).toMatchSnapshot();
  });

  it('has a documents tab', () => {
    setup(
      {
        project: mockProject,
        setProject: setProjectMock,
        setContainerState: setContainerStateMock,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const documentTab = screen.getByRole('tab', { name: 'Documents' });
    expect(documentTab).toBeVisible();
  });
});

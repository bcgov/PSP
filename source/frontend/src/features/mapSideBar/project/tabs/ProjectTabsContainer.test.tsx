import Claims from '@/constants/claims';
import { mockProjectGetResponse } from '@/mocks/projects.mock';
import { render, RenderOptions, screen } from '@/utils/test-utils';

import ProjectTabsContainer, { IProjectTabsContainerProps } from './ProjectTabsContainer';

// mock auth library
jest.mock('@react-keycloak/web');

const setProjectMock = jest.fn();
const setEditModeMock = jest.fn();
const setContainerStateMock = jest.fn();

const mockProject = mockProjectGetResponse();

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
    jest.clearAllMocks();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup(
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

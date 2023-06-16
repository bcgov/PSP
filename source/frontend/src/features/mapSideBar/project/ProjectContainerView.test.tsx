import { mockProjectGetResponse } from '@/mocks/projects.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import { IProjectContainerViewProps } from './ProjectContainer';
import ProjectContainerView from './ProjectContainerView';
import { ProjectTabNames } from './tabs/ProjectTabs';

// mock auth library
jest.mock('@react-keycloak/web');

const mockProps: IProjectContainerViewProps = {
  project: mockProjectGetResponse(),
  viewTitle: 'Title',
  loadingProject: false,
  activeTab: ProjectTabNames.projectDetails,
  isEditing: false,
  showConfirmModal: false,
  isSubmitting: false,
  onClose: jest.fn(),
  onSetProject: jest.fn(),
  onSetContainerState: jest.fn(),
  onSuccess: jest.fn(),
};

describe('ProjectSummaryView component', () => {
  // render component under test
  const setup = (props: IProjectContainerViewProps, renderOptions: RenderOptions = {}) => {
    const utils = render(<ProjectContainerView {...mockProps} />, {
      useMockAuthentication: true,
      ...renderOptions,
    });

    return { ...utils };
  };

  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup({ ...mockProps });
    expect(asFragment()).toMatchSnapshot();
  });
});

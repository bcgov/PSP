import { mockProjectGetResponse } from '@/mocks/projects.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import { IProjectContainerViewProps } from './ProjectContainer';
import ProjectContainerView from './ProjectContainerView';
import { ProjectTabNames } from './tabs/ProjectTabs';

// mock auth library

const mockProps: IProjectContainerViewProps = {
  project: mockProjectGetResponse(),
  viewTitle: 'Title',
  loadingProject: false,
  activeTab: ProjectTabNames.projectDetails,
  isEditing: false,
  showConfirmModal: false,
  isSubmitting: false,
  onClose: vi.fn(),
  onSetProject: vi.fn(),
  onSetContainerState: vi.fn(),
  onSuccess: vi.fn(),
  setIsValid: vi.fn(),
  displayRequiredFieldsError: false,
};

vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children;
    }),
  };
});

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
    vi.clearAllMocks();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup({ ...mockProps });
    expect(asFragment()).toMatchSnapshot();
  });
});

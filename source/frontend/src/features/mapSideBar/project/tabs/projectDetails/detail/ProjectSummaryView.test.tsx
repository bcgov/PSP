import { Claims } from '@/constants';
import { mockProjectGetResponse } from '@/mocks/projects.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import ProjectSummaryView, { IProjectSummaryViewProps } from './ProjectSummaryView';

// mock auth library
jest.mock('@react-keycloak/web');
const onEdit = jest.fn();

describe('ProjectSummaryView component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IProjectSummaryViewProps> } = {},
  ) => {
    const utils = render(
      <ProjectSummaryView
        project={renderOptions?.props?.project ?? mockProjectGetResponse()}
        onEdit={onEdit}
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
      },
    );

    return { ...utils };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the edit button when the user project edit permissions', () => {
    const { getByTitle } = setup({ claims: [Claims.PROJECT_EDIT] });
    const editButton = getByTitle('Edit project');
    expect(editButton).toBeVisible();
  });

  it('does not render the edit button when the user does not have project edit permissions', () => {
    const { queryByTitle } = setup({ claims: [] });
    const editButton = queryByTitle('Edit project');
    expect(editButton).toBeNull();
  });
});

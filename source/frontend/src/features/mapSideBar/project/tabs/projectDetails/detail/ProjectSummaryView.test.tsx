import { Claims } from '@/constants';
import { mockProjectGetResponse } from '@/mocks/projects.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import ProjectSummaryView, { IProjectSummaryViewProps } from './ProjectSummaryView';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { getMockPerson } from '@/mocks/contacts.mock';

// mock auth library

const onEdit = vi.fn();

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
    vi.clearAllMocks();
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

  it('renders the project management team members correctly', () => {
    const projectWithTeam = {
      ...mockProjectGetResponse(),
      projectPersons: [
        {
          personId: 1,
          person: getMockPerson({ id: 1, surname: 'Doe', firstName: 'John' }),
          id: 1,
          projectId: 1,
          project: null,
          ...getEmptyBaseAudit(),
        },
        {
          personId: 2,
          person: getMockPerson({ id: 2, surname: 'Smith', firstName: 'Jane' }),
          id: 1,
          projectId: 1,
          project: null,
          ...getEmptyBaseAudit(),
        },
      ],
    };
    const { getByText } = setup({
      props: { project: projectWithTeam },
    });

    expect(getByText('John Doe')).toBeVisible();
    expect(getByText('Jane Smith')).toBeVisible();
  });

  it('renders no team members when projectPersons is empty', () => {
    const projectWithoutTeam = {
      ...mockProjectGetResponse(),
      projectPersons: [],
    };
    const { queryByText } = setup({ props: { project: projectWithoutTeam } });

    expect(queryByText('Management team member')).toBeNull();
  });
});

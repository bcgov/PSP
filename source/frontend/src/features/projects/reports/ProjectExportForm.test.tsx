import { createMemoryHistory } from 'history';

import { getMockPerson } from '@/mocks/contacts.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockOrganization } from '@/mocks/organization.mock';
import { mockProjects } from '@/mocks/projects.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { ProjectExportTypes } from './models';
import ProjectExportForm, { IProjectExportFormProps } from './ProjectExportForm';

const history = createMemoryHistory();

const onExport = vi.fn();
const onExportTypeSelected = vi.fn();

describe('ProjectExportForm component', () => {
  // render component under test
  const setup = (props: IProjectExportFormProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <ProjectExportForm
        onExport={onExport}
        onExportTypeSelected={onExportTypeSelected}
        projects={props.projects ?? []}
        loading={props.loading}
        teamMembers={props.teamMembers ?? []}
      />,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        claims: [],
        history,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({} as any);
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls onExportTypeSelected when the user selects an export type', async () => {
    const { getByDisplayValue } = setup({} as any);

    const select = getByDisplayValue(/Select Export Type.../i);

    await act(async () => {
      userEvent.selectOptions(select, ProjectExportTypes.AGREEMENT);
    });
    expect(onExportTypeSelected).toHaveBeenCalled();
  });

  it('calls onExport when the user clicks the export button', async () => {
    const { getByDisplayValue, getByText } = setup({} as any);

    const select = getByDisplayValue(/Select Export Type.../i);
    await act(async () => {
      userEvent.selectOptions(select, ProjectExportTypes.AGREEMENT);
    });

    const exportButton = getByText('Export');
    await act(async () => {
      userEvent.click(exportButton);
    });

    expect(onExport).toHaveBeenCalled();
  });

  it('displays loading spinner when prop set', async () => {
    const { getByTestId } = setup({ loading: true } as any);

    const loading = getByTestId('filter-backdrop-loading');
    expect(loading).toBeVisible();
  });

  it('displays projects when passed', async () => {
    const { getByDisplayValue } = setup({ projects: mockProjects() } as any);

    const select = getByDisplayValue(/Select Export Type.../i);

    await act(async () => {
      userEvent.selectOptions(select, ProjectExportTypes.AGREEMENT);
    });

    expect(screen.getByText(/776/i)).not.toBeNull();
  });

  it('displays team members when passed person', async () => {
    const { getByDisplayValue } = setup({
      teamMembers: [
        {
          personId: 1,
          person: getMockPerson({ id: 1, surname: 'last', firstName: 'first' }),
        },
      ],
    } as any);

    const select = getByDisplayValue(/Select Export Type.../i);

    await act(async () => {
      userEvent.selectOptions(select, ProjectExportTypes.AGREEMENT);
    });

    expect(screen.getByText(/first last/i)).not.toBeNull();
  });

  it('displays team members when passed organization', async () => {
    const { getByDisplayValue } = setup({
      teamMembers: [
        {
          organizationId: 100,
          organization: getMockOrganization({ id: 100, name: 'FORTIS BC' }),
        },
      ],
    } as any);

    const select = getByDisplayValue(/Select Export Type.../i);

    await act(async () => {
      userEvent.selectOptions(select, ProjectExportTypes.AGREEMENT);
    });

    expect(screen.getByText(/FORTIS BC/i)).not.toBeNull();
  });

  it('hides submit, project, team members by default', async () => {
    setup({} as any);

    expect(screen.queryByText('Project')).toBeNull();
    expect(screen.queryByText('Team Member')).toBeNull();
    expect(screen.queryByText('Export')).toBeNull();
  });
});

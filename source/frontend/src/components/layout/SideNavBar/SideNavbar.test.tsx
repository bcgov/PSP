import { act, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';

import { Claims, Roles } from '@/constants/index';

import { SideNavBar } from './SideNavbar';
import { render } from '@/utils/test-utils';
import { useKeycloak } from '@react-keycloak/web';
import { SidebarStateContextProvider } from './SideNavbarContext';

interface IRenderProps {
  roles?: Roles[];
  claims?: Claims[];
}

const mocks = vi.hoisted(() => {
  return {
    useKeycloak: vi.fn(),
  };
});

vi.mock('@react-keycloak/web', () => {
  return {
    useKeycloak: mocks.useKeycloak,
  };
});

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      return children;
    }),
  };
});

const history = createMemoryHistory();
const renderComponent = (props?: IRenderProps) => {
  const defaultUserInfo = {
    organizations: [1],
    client_roles:
      [
        ...(props?.claims ?? []),
        Claims.LEASE_VIEW,
        Claims.RESEARCH_VIEW,
        Claims.PROJECT_VIEW,
        ...(props?.roles ?? [Roles.ACQUISITION_FUNCTIONAL]),
      ] ?? [],
    email: 'test@test.com',
    name: 'Chester Tester',
    idir_user_guid: '00000000000000000000000000000000',
  };

  mocks.useKeycloak.mockImplementation(() => ({
    keycloak: {
      userInfo: defaultUserInfo,
      subject: 'test',
      authenticated: true,
      loadUserInfo: vi.fn().mockResolvedValue(defaultUserInfo),
    } as any,
    initialized: true,
  }));
  return render(
    <SidebarStateContextProvider>
      <SideNavBar />
    </SidebarStateContextProvider>,
    { history: history },
  );
};

describe('SideNavbar display and logic', () => {
  it('renders', async () => {
    const { container } = renderComponent();
    await waitFor(async () => {
      expect(container).toMatchSnapshot();
    });
  });

  it('By default, the sidebar is collapsed', async () => {
    const { getByTitle } = renderComponent();
    await waitFor(async () => {
      expect(getByTitle('expand')).toBeInTheDocument();
    });
  });

  it('The collapsed sidebar only displays icons', async () => {
    const { getByTitle, queryByText, getByTestId } = renderComponent();
    await waitFor(async () => {
      expect(getByTitle('expand')).toBeInTheDocument();
      expect(queryByText('Leases & Licenses')?.clientWidth).toBe(0);
      expect(getByTestId('nav-tooltip-leases&licenses')).toContainHTML('svg');
    });
  });

  it('The sidebar can be expanded', async () => {
    const { getByTitle } = renderComponent();
    const expandButton = getByTitle('expand');
    await act(async () => {
      userEvent.click(expandButton);
    });
    await waitFor(async () => {
      expect(getByTitle('collapse')).toBeInTheDocument();
    });
  });

  it('The expanded sidebars displays an icon and text.', async () => {
    const { getByTitle, getByText, getByTestId } = renderComponent();
    const expandButton = getByTitle('expand');
    await act(async () => {
      userEvent.click(expandButton);
    });
    await waitFor(async () => {
      expect(getByTestId('nav-tooltip-leases&licenses')).toContainHTML('svg');
      expect(getByText('Leases & Licenses')).toBeInTheDocument();
    });
  });

  it('The sidebar restricts nav items by role.', async () => {
    const { getByTitle, queryByText } = renderComponent();
    const expandButton = getByTitle('expand');
    await act(async () => {
      userEvent.click(expandButton);
    });
    await waitFor(async () => {
      expect(queryByText('Admin Tools')).toBeNull();
    });
  });

  it('The sidebar restricts nav items by claim.', async () => {
    const { getByTitle, queryByText } = renderComponent();
    const expandButton = getByTitle('expand');
    await act(async () => {
      userEvent.click(expandButton);
    });
    await waitFor(async () => {
      expect(queryByText('Contacts')).toBeNull();
    });
  });

  it('The sidebar displays restricted items if the user has the required role.', async () => {
    const { getByTitle, getByText } = renderComponent({
      roles: [Roles.SYSTEM_ADMINISTRATOR],
    });
    const expandButton = getByTitle('expand');
    await act(async () => {
      userEvent.click(expandButton);
    });
    await waitFor(async () => {
      expect(getByText('Admin Tools')).toBeInTheDocument();
    });
  });

  describe('side tray tests', () => {
    it('Opens the side tray when an icon is clicked.', async () => {
      const { getByText, getByTestId } = renderComponent({
        roles: [Roles.SYSTEM_ADMINISTRATOR],
      });
      const managementButton = getByTestId('nav-tooltip-leases&licenses');
      await act(async () => {
        userEvent.click(managementButton);
      });
      await waitFor(async () => {
        expect(getByText('Manage Lease/License Files')).toBeInTheDocument();
      });
    });

    it('closes the side tray when the close button is clicked.', async () => {
      const { getByTestId, getByTitle } = renderComponent({
        roles: [Roles.SYSTEM_ADMINISTRATOR],
      });
      const managementButton = getByTestId('nav-tooltip-leases&licenses');
      await act(async () => {
        userEvent.click(managementButton);
      });
      const closeButton = getByTitle('close');
      await act(async () => {
        userEvent.click(closeButton);
      });
      await waitFor(async () => {
        expect(getByTestId('side-tray')).not.toHaveClass('show');
      });
    });

    it('closes the side tray when the close button is clicked.', async () => {
      const { getByTestId, getByTitle } = renderComponent({
        roles: [Roles.SYSTEM_ADMINISTRATOR],
      });
      const managementButton = getByTestId('nav-tooltip-leases&licenses');
      await act(async () => {
        userEvent.click(managementButton);
      });
      const closeButton = getByTitle('close');
      await act(async () => {
        userEvent.click(closeButton);
      });
      await waitFor(async () => {
        expect(getByTestId('side-tray')).not.toHaveClass('show');
      });
    });

    it('Changes to the expected page when tray item is clicked', async () => {
      const { getByTestId, getByText } = renderComponent({
        roles: [Roles.SYSTEM_ADMINISTRATOR],
      });
      const adminToolsButton = getByTestId('nav-tooltip-admintools');
      await act(async () => {
        userEvent.click(adminToolsButton);
      });
      const manageUsersLink = getByText('Manage Users');
      await act(async () => {
        userEvent.click(manageUsersLink);
      });
      await waitFor(async () => {
        expect(history.location.pathname).toBe('/admin/users');
      });
    });

    it('Opens project side tray when an icon is clicked.', async () => {
      const { getByText, getByTestId } = renderComponent({
        roles: [Roles.SYSTEM_ADMINISTRATOR],
      });
      const projectButton = getByTestId('nav-tooltip-project');
      await act(async () => {
        userEvent.click(projectButton);
      });
      const searchProjectLink = getByText('Manage Projects');
      await act(async () => {
        userEvent.click(searchProjectLink);
      });
      await waitFor(async () => {
        expect(history.location.pathname).toBe('/project/list');
      });
    });

    it('Opens research side tray when an icon is clicked.', async () => {
      const { getByText, getByTestId } = renderComponent({
        roles: [Roles.SYSTEM_ADMINISTRATOR],
      });
      const researchButton = getByTestId('nav-tooltip-research');
      await act(async () => {
        userEvent.click(researchButton);
      });
      const searchReseachFileLink = getByText('Manage Research Files');
      await act(async () => {
        userEvent.click(searchReseachFileLink);
      });
      await waitFor(async () => {
        expect(history.location.pathname).toBe('/research/list');
      });
    });

    it('Opens contact side tray when an icon is clicked.', async () => {
      const { getByText, getByTestId } = renderComponent({
        roles: [Roles.SYSTEM_ADMINISTRATOR],
        claims: [Claims.CONTACT_VIEW],
      });
      const contactButton = getByTestId('nav-tooltip-contacts');
      await act(async () => {
        userEvent.click(contactButton);
      });
      const searchContactLink = getByText('Manage Contacts');
      await act(async () => {
        userEvent.click(searchContactLink);
      });
      await waitFor(async () => {
        expect(history.location.pathname).toBe('/contact/list');
      });
    });
  });
});

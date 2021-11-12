import { act, render, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Roles } from 'constants/index';
import { createMemoryHistory } from 'history';
import React from 'react';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import { SideNavBar } from './SideNavbar';
import { SidebarStateContextProvider } from './SideNavbarContext';

interface IRenderProps {
  roles: string[];
}
const history = createMemoryHistory();
const renderComponent = (props?: IRenderProps) =>
  render(
    <TestCommonWrapper history={history} roles={props?.roles ?? [Roles.REAL_ESTATE_MANAGER]}>
      <SidebarStateContextProvider>
        <SideNavBar />
      </SidebarStateContextProvider>
    </TestCommonWrapper>,
  );

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
      expect(queryByText('Management')?.clientWidth).toBe(0);
      expect(getByTestId('nav-tooltip-management')).toContainHTML('svg');
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
      expect(getByTestId('nav-tooltip-management')).toContainHTML('svg');
      expect(getByText('Management')).toBeInTheDocument();
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
      const managementButton = getByTestId('nav-tooltip-management');
      await act(async () => {
        userEvent.click(managementButton);
      });
      await waitFor(async () => {
        expect(getByText('Leases & Licenses')).toBeInTheDocument();
      });
    });

    it('closes the side tray when the close button is clicked.', async () => {
      const { getByTestId, getByTitle } = renderComponent({
        roles: [Roles.SYSTEM_ADMINISTRATOR],
      });
      const managementButton = getByTestId('nav-tooltip-management');
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
      const managementButton = getByTestId('nav-tooltip-management');
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
  });
});

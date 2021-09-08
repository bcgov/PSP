import { act, render } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Roles } from 'constants/index';
import { createMemoryHistory } from 'history';
import React from 'react';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import { SideNavBar } from './SideNavbar';

interface IRenderProps {
  roles: string[];
}
const history = createMemoryHistory();
const renderComponent = (props?: IRenderProps) =>
  render(
    <TestCommonWrapper history={history} roles={props?.roles ?? [Roles.REAL_ESTATE_MANAGER]}>
      <SideNavBar />
    </TestCommonWrapper>,
  );

describe('SideNavbar display and logic', () => {
  it('renders', () => {
    const { container } = renderComponent();
    expect(container).toMatchSnapshot();
  });

  it('By default, the sidebar is collapsed', () => {
    const { getByTitle } = renderComponent();
    expect(getByTitle('expand')).toBeInTheDocument();
  });

  it('The collapsed sidebar only displays icons', () => {
    const { getByTitle, queryByText, getByTestId } = renderComponent();
    expect(getByTitle('expand')).toBeInTheDocument();
    expect(queryByText('Management')?.clientWidth).toBe(0);
    expect(getByTestId('nav-tooltip-management')).toContainHTML('svg');
  });

  it('The sidebar can be expanded', () => {
    const { getByTitle } = renderComponent();
    const expandButton = getByTitle('expand');
    act(() => {
      userEvent.click(expandButton);
    });
    expect(getByTitle('collapse')).toBeInTheDocument();
  });

  it('The expanded sidebars displays an icon and text.', () => {
    const { getByTitle, getByText, getByTestId } = renderComponent();
    const expandButton = getByTitle('expand');
    act(() => {
      userEvent.click(expandButton);
    });
    expect(getByTestId('nav-tooltip-management')).toContainHTML('svg');
    expect(getByText('Management')).toBeInTheDocument();
  });

  it('The sidebar restricts nav items by role.', () => {
    const { getByTitle, queryByText } = renderComponent();
    const expandButton = getByTitle('expand');
    act(() => {
      userEvent.click(expandButton);
    });
    expect(queryByText('Admin Tools')).toBeNull();
  });

  it('The sidebar displays restricted items if the user has the required role.', () => {
    const { getByTitle, getByText } = renderComponent({
      roles: [Roles.SYSTEM_ADMINISTRATOR],
    });
    const expandButton = getByTitle('expand');
    act(() => {
      userEvent.click(expandButton);
    });
    expect(getByText('Admin Tools')).toBeInTheDocument();
  });

  describe('side tray tests', () => {
    it('Opens the side tray when an icon is clicked.', () => {
      const { getByText, getByTestId } = renderComponent({
        roles: [Roles.SYSTEM_ADMINISTRATOR],
      });
      const managementButton = getByTestId('nav-tooltip-management');
      act(() => {
        userEvent.click(managementButton);
      });
      expect(getByText('Leases & Licenses')).toBeInTheDocument();
    });

    it('closes the side tray when the close button is clicked.', () => {
      const { getByTestId, getByTitle } = renderComponent({
        roles: [Roles.SYSTEM_ADMINISTRATOR],
      });
      const managementButton = getByTestId('nav-tooltip-management');
      act(() => {
        userEvent.click(managementButton);
      });
      const closeButton = getByTitle('close');
      act(() => {
        userEvent.click(closeButton);
      });
      expect(getByTestId('side-tray')).not.toHaveClass('show');
    });

    it('closes the side tray when the close button is clicked.', () => {
      const { getByTestId, getByTitle } = renderComponent({
        roles: [Roles.SYSTEM_ADMINISTRATOR],
      });
      const managementButton = getByTestId('nav-tooltip-management');
      act(() => {
        userEvent.click(managementButton);
      });
      const closeButton = getByTitle('close');
      act(() => {
        userEvent.click(closeButton);
      });
      expect(getByTestId('side-tray')).not.toHaveClass('show');
    });

    it('Changes to the expected page when tray item is clicked', () => {
      const { getByTestId, getByText } = renderComponent({
        roles: [Roles.SYSTEM_ADMINISTRATOR],
      });
      const adminToolsButton = getByTestId('nav-tooltip-admintools');
      act(() => {
        userEvent.click(adminToolsButton);
      });
      const manageUsersLink = getByText('Manage Users');
      act(() => {
        userEvent.click(manageUsersLink);
      });
      expect(history.location.pathname).toBe('/admin/users');
    });
  });
});

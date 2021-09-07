import { act, render } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Roles } from 'constants/index';
import React from 'react';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import { SideNavBar } from './SideNavbar';

interface IRenderProps {
  roles: string[];
}
const renderComponent = (props?: IRenderProps) =>
  render(
    <TestCommonWrapper roles={props?.roles ?? [Roles.REAL_ESTATE_MANAGER]}>
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
    expect(queryByText('Management')).toBeNull();
    expect(getByTestId('nav-tooltip-Management')).toContainHTML('svg');
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
    expect(getByTestId('nav-tooltip-Management')).toContainHTML('svg');
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
});

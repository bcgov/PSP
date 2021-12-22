import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';
import { render, RenderOptions } from 'utils/test-utils';

import LeaseIndex, { ILeaseAndLicenseIndexProps } from './LeaseIndex';

const history = createMemoryHistory();

describe('LeaseIndex component', () => {
  const setup = (renderOptions: RenderOptions & ILeaseAndLicenseIndexProps = {}) => {
    // render component under test
    const component = render(
      <LeaseIndex
        leaseId={renderOptions.leaseId}
        currentPageName={renderOptions.currentPageName}
      />,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      component,
    };
  };
  it('renders as expected', () => {
    const { component } = setup();
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('navigates to the expected location when clicked', () => {
    const { component } = setup({ leaseId: 1 });
    const { getByText } = component;
    userEvent.click(getByText('Details'));

    expect(history.location.pathname).toContain('details');
  });

  it('the active page displays the correct styling', () => {
    const { component } = setup({ leaseId: 1, currentPageName: 'details' });
    const { getByText } = component;
    const detailsLink = getByText('Details');
    expect(detailsLink).toHaveClass('active');
  });
});

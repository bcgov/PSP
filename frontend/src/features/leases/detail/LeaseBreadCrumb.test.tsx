import userEvent from '@testing-library/user-event';
import Claims from 'constants/claims';
import { createMemoryHistory } from 'history';
import { ReactElement } from 'react';
import { render, RenderOptions } from 'utils/test-utils';

import LeaseBreadCrumb from './LeaseBreadCrumb';
import { LeasePageNames, leasePages } from './LeaseContainer';

const history = createMemoryHistory();
const onClickManagement = jest.fn();

describe('LeaseBreadCrumb component', () => {
  const setup = (renderOptions: RenderOptions & { breadcrumb?: ReactElement } = {}) => {
    // render component under test
    const component = render(
      renderOptions.breadcrumb ?? (
        <LeaseBreadCrumb leaseId={1} onClickManagement={onClickManagement} />
      ),
      {
        ...renderOptions,
        claims: [Claims.LEASE_VIEW],
        history,
      },
    );

    return {
      component,
    };
  };
  beforeEach(() => {
    onClickManagement.mockReset();
  });
  it('renders as expected with no data', () => {
    const { component } = setup();
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('renders as expected with data', () => {
    const { component } = setup({
      breadcrumb: (
        <LeaseBreadCrumb
          onClickManagement={onClickManagement}
          leaseId={1}
          leasePage={leasePages.get(LeasePageNames.DETAILS)}
        />
      ),
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('fires the onClickManagement event when Management section of breadcrumb is clicked.', () => {
    const { component } = setup();
    const { getByText } = component;
    userEvent.click(getByText('Management'));
    expect(onClickManagement).toHaveBeenCalled();
  });
  it('navigates to the expected location when Lease & License search clicked', () => {
    const { component } = setup();
    const { getByText } = component;
    userEvent.click(getByText('Lease & License Search'));
    expect(history.location.pathname).toBe('/lease/list');
  });

  it('the last part of the breadcrumb is active', () => {
    const { component } = setup({
      breadcrumb: (
        <LeaseBreadCrumb
          onClickManagement={onClickManagement}
          leaseId={1}
          leasePage={leasePages.get(LeasePageNames.DETAILS)}
        />
      ),
    });
    const { getByText } = component;
    const detailsLink = getByText('Details');
    expect(detailsLink).toHaveClass('active');
  });
});

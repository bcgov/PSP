import { useKeycloak } from '@react-keycloak/web';
import userEvent from '@testing-library/user-event';
import Claims from 'constants/claims';
import { createMemoryHistory } from 'history';
import { render, RenderOptions } from 'utils/test-utils';

import LeaseIndex, { ILeaseAndLicenseIndexProps } from './LeaseIndex';

jest.mock('@react-keycloak/web');
const history = createMemoryHistory();

const mockKeycloak = (claims: string[]) => {
  (useKeycloak as jest.Mock).mockReturnValue({
    keycloak: {
      subject: 'test',
      userInfo: {
        client_roles: claims,
        organizations: ['1'],
      },
    },
  });
};

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
    mockKeycloak([Claims.DOCUMENT_VIEW]);

    const { component } = setup();
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('navigates to the expected location when clicked', () => {
    mockKeycloak([]);

    const { component } = setup({ leaseId: 1 });
    const { getByText } = component;
    userEvent.click(getByText('Details'));

    expect(history.location.pathname).toContain('details');
  });

  it('the active page displays the correct styling', () => {
    mockKeycloak([]);

    const { component } = setup({ leaseId: 1, currentPageName: 'details' });
    const { getByText } = component;
    const detailsLink = getByText('Details');
    expect(detailsLink).toHaveClass('active');
  });

  it('Hides Document link if incorrect permissions', () => {
    mockKeycloak([]);

    const { component } = setup({ leaseId: 1, currentPageName: 'details' });
    const { queryByText } = component;
    expect(queryByText('Documents')).not.toBeInTheDocument();
  });

  it('Shows Document link if incorrect permissions', () => {
    mockKeycloak([Claims.DOCUMENT_VIEW]);

    const { component } = setup({ leaseId: 1, currentPageName: 'details' });
    const { queryByText } = component;
    expect(queryByText('Documents')).toBeInTheDocument();
  });
});

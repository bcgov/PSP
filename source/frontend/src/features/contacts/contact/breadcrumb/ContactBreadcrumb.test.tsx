import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';
import { ReactElement } from 'react';

import { render, RenderOptions } from '@/utils/test-utils';

import { ContactBreadcrumb } from '../..';

const history = createMemoryHistory();
const onClickManagement = jest.fn();

describe('ContactBreadCrumb component', () => {
  const setup = (renderOptions: RenderOptions & { breadcrumb?: ReactElement } = {}) => {
    // render component under test
    const component = render(renderOptions.breadcrumb ?? <ContactBreadcrumb />, {
      ...renderOptions,
      history,
    });

    return {
      component,
    };
  };
  beforeEach(() => {
    onClickManagement.mockReset();
  });
  it('renders as expected', () => {
    const { component } = setup();
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('navigates to the expected location when contact search clicked', () => {
    const { component } = setup();
    const { getByText } = component;
    userEvent.click(getByText('Contact Search'));
    expect(history.location.pathname).toBe('/contact/list');
  });

  it('the last part of the breadcrumb is active', () => {
    const { component } = setup({});
    const { getByText } = component;
    const detailsLink = getByText('Contact Details');
    expect(detailsLink).toHaveClass('active');
  });
});

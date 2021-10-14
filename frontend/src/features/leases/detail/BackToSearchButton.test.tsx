import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';
import { render, RenderOptions } from 'utils/test-utils';

import BackToSearchButton from './BackToSearchButton';

const history = createMemoryHistory();

describe('BackToSearchButton component', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    // render component under test
    const component = render(<BackToSearchButton />, {
      ...renderOptions,
      history,
    });

    return {
      component,
    };
  };
  it('renders as expected', () => {
    const { component } = setup();
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('navigates to the expected location when clicked', () => {
    const { component } = setup();
    const { getByText } = component;
    userEvent.click(getByText('Back to Search'));
    expect(history.location.pathname).toBe('/lease/list');
  });
});

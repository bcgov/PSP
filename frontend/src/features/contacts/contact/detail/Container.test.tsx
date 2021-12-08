import { createMemoryHistory } from 'history';
import { render, RenderOptions } from 'utils/test-utils';
import ContactViewContainer from './Container';

const history = createMemoryHistory();

describe('ContactViewContainer component', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    // render component under test
    const component = render(<ContactViewContainer />, {
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
});

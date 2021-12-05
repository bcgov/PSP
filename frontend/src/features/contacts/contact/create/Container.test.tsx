import { createMemoryHistory } from 'history';
import { render, RenderOptions } from 'utils/test-utils';

import { ContactCreateContainer, IContactCreateContainerProps } from './';

const history = createMemoryHistory();

describe('ContactContainer component', () => {
  const setup = (renderOptions?: RenderOptions & Partial<IContactCreateContainerProps>) => {
    // render component under test
    const component = render(<ContactCreateContainer />, {
      ...renderOptions,
      history,
    });

    return {
      ...component,
    };
  };

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays contact selector', () => {
    const { getByLabelText } = setup();
    expect(getByLabelText('Individual')).toBeVisible();
  });

  it('renders Contact Person form by default', () => {
    const { queryByLabelText } = setup();
    expect(queryByLabelText('First Name')).not.toBeNull();
  });
});

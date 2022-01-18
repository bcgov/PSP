import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions } from 'utils/test-utils';

import { CreateContactContainer } from '.';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('CreateContactContainer component', () => {
  const setup = (renderOptions?: RenderOptions) => {
    // render component under test
    const component = render(<CreateContactContainer />, {
      ...renderOptions,
      store: storeState,
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

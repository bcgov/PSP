import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from 'utils/test-utils';

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

  it('should render as expected', async () => {
    const { asFragment } = setup();
    await waitFor(() => expect(asFragment()).toMatchSnapshot());
  });

  it('should display contact selector', async () => {
    const { getByLabelText } = setup();
    await waitFor(() => expect(getByLabelText('Individual')).toBeVisible());
  });

  it('should render Create Person form by default', async () => {
    const { queryByLabelText } = setup();
    await waitFor(() => expect(queryByLabelText('First Name')).not.toBeNull());
  });

  describe('when contact selector is changed', () => {
    it('should render the correct form', async () => {
      const { getByLabelText, queryByLabelText } = setup();
      userEvent.click(getByLabelText('Organization'));
      await waitFor(() => expect(queryByLabelText('Organization Name')).not.toBeNull());
    });
  });

  describe('when Cancel button is clicked', () => {
    it('should cancel the form and navigate to Contacts List view', async () => {
      const { getByText } = setup();
      userEvent.click(getByText('Cancel'));
      await waitFor(() => expect(history.location.pathname).toBe('/contact/list'));
    });
  });
});

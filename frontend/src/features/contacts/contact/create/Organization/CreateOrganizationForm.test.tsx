import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { ContactMethodTypes } from 'constants/contactMethodType';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, render, RenderOptions, waitFor } from 'utils/test-utils';

import CreateOrganizationForm from './CreateOrganizationForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

// mock http requests
const mockAxios = new MockAdapter(axios);

describe('CreateOrganizationForm', () => {
  const setup = (renderOptions?: RenderOptions) => {
    // render component under test
    const component = render(<CreateOrganizationForm />, {
      ...renderOptions,
      store: storeState,
      history,
    });

    const getSaveButton = () => component.getByText('Save');
    const getCancelButton = () => component.getByText('Cancel');

    return {
      ...component,
      getSaveButton,
      getCancelButton,
    };
  };

  it('renders as expected', async () => {
    const { asFragment } = setup();
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  describe('when Cancel button is clicked', () => {
    it('should cancel the form and navigate to Contacts List view', async () => {
      const { getCancelButton } = setup();
      const cancel = getCancelButton();
      userEvent.click(cancel);
      await waitFor(() => expect(history.location.pathname).toBe('/contact/list'));
    });
  });

  describe('when Save button is clicked', () => {
    it('should save the form with minimal data', async () => {
      mockAxios.onPost().reply(200, { id: 1 });
      const { getSaveButton, container } = setup();
      // provide required fields
      await fillInput(container, 'name', 'FooBarBaz Property Management');
      await fillInput(container, 'emailContactMethods.0.value', 'foo@bar.com');
      await fillInput(
        container,
        'emailContactMethods.0.contactMethodTypeCode',
        ContactMethodTypes.WorkEmail,
        'select',
      );

      const save = getSaveButton();
      userEvent.click(save);

      await waitFor(() => {
        expect(mockAxios.history.post[0].data).toEqual(JSON.stringify(expectedFormData));
      });
      await waitFor(() => {
        expect(history.location.pathname).toBe('/contact/O1');
      });
    });
  });
});

const expectedFormData = {
  isDisabled: false,
  name: 'FooBarBaz Property Management',
  alias: '',
  incorporationNumber: '',
  comment: '',
  addresses: [],
  contactMethods: [{ contactMethodTypeCode: { id: 'WORKEMAIL' }, value: 'foo@bar.com' }],
};

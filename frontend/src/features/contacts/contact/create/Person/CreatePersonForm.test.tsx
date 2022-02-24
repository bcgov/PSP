import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { ContactMethodTypes } from 'constants/contactMethodType';
import { createMemoryHistory } from 'history';
import { IEditablePerson } from 'interfaces/editable-contact';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, render, RenderOptions, waitFor } from 'utils/test-utils';

import CreatePersonForm from './CreatePersonForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

// mock http requests
const mockAxios = new MockAdapter(axios);

describe('CreatePersonForm', () => {
  const setup = (renderOptions?: RenderOptions) => {
    // render component under test
    const component = render(<CreatePersonForm />, {
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

  beforeEach(() => mockAxios.resetHistory());

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
      mockAxios.onPost().reply(200, {});
      const { getSaveButton, container } = setup();
      // provide required fields
      await fillInput(container, 'firstName', 'Chester');
      await fillInput(container, 'surname', 'Tester');
      await fillInput(container, 'emailContactMethods.0.value', 'test@test.com');
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
    });
  });
});

const expectedFormData: IEditablePerson = {
  isDisabled: false,
  firstName: 'Chester',
  middleNames: '',
  surname: 'Tester',
  preferredName: '',
  comment: '',
  organization: null,
  useOrganizationAddress: false,
  addresses: [],
  contactMethods: [
    {
      contactMethodTypeCode: {
        id: 'WORKEMAIL',
      },
      value: 'test@test.com',
    },
  ],
};

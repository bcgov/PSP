import { ContactMethodTypes } from 'constants/contactMethodType';
import { AddressTypes } from 'constants/index';
import useAddContact from 'features/contacts/hooks/useAddContact';
import { createMemoryHistory } from 'history';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { IEditableOrganization, IEditablePerson } from 'interfaces/editable-contact';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import CreatePersonForm from './CreatePersonForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockOrganization: IEditableOrganization = {
  id: 200,
  isDisabled: false,
  name: 'FooBarBaz Property Management',
  alias: '',
  incorporationNumber: 'BC123456789',
  comment: 'I got comments for you',
  addresses: [
    {
      organizationAddressId: 1,
      organizationAddressRowVersion: 2,
      organizationId: 200,
      id: 1,
      addressTypeId: { id: AddressTypes.Mailing },
      streetAddress1: '3000 Main Ave',
      streetAddress2: '',
      streetAddress3: '',
      municipality: 'Vancouver',
      provinceId: 1,
      countryId: 1,
      postal: 'V8V1A1',
      rowVersion: 3,
    },
  ],
  contactMethods: [
    {
      contactMethodTypeCode: { id: ContactMethodTypes.WorkEmail },
      value: 'foo@bar.com',
    },
  ],
};

const mockPerson: IEditablePerson = {
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

// Mock API service calls
jest.mock('hooks/pims-api/useApiContacts');
jest.mock('features/contacts/hooks/useAddContact');

const getOrganization = jest.fn(() => mockOrganization);
(useApiContacts as jest.Mock).mockReturnValue({ getOrganization });

const addPerson = jest.fn();

(useAddContact as jest.Mock).mockReturnValue({ addPerson });

describe('CreatePersonForm', () => {
  const setup = (renderOptions?: RenderOptions) => {
    // render component under test
    const component = render(<CreatePersonForm />, {
      ...renderOptions,
      store: storeState,
      history,
    });

    return {
      ...component,
      getSaveButton: () => component.getByText('Save'),
      getCancelButton: () => component.getByText('Cancel'),
    };
  };

  beforeEach(() => {
    getOrganization.mockReset();
    addPerson.mockReset();
  });

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
      addPerson.mockResolvedValue({ ...mockPerson, id: 1 });
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
      await waitFor(() => expect(addPerson).toBeCalledWith(mockPerson, expect.anything(), false));

      await waitFor(() => {
        expect(history.location.pathname).toBe('/contact/P1');
      });
    });
  });
});

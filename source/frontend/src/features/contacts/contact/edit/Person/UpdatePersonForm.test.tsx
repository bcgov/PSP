import { ContactMethodTypes } from 'constants/contactMethodType';
import { AddressTypes } from 'constants/index';
import { usePersonDetail } from 'features/contacts/hooks/usePersonDetail';
import useUpdateContact from 'features/contacts/hooks/useUpdateContact';
import { createMemoryHistory } from 'history';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { IEditableOrganization, IEditablePerson } from 'interfaces/editable-contact';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import UpdatePersonForm from './UpdatePersonForm';

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
  id: 1,
  isDisabled: false,
  firstName: 'Chester',
  middleNames: '',
  surname: 'Tester',
  preferredName: '',
  comment: 'Lorem ipsum',
  organization: { id: mockOrganization.id as number, text: mockOrganization.name },
  useOrganizationAddress: false,
  addresses: [],
  contactMethods: [
    {
      contactMethodTypeCode: { id: ContactMethodTypes.WorkEmail },
      value: 'test@test.com',
    },
  ],
};

// Mock API service calls
jest.mock('hooks/pims-api/useApiContacts');
jest.mock('features/contacts/hooks/usePersonDetail');
jest.mock('features/contacts/hooks/useUpdateContact');

const getOrganization = jest.fn(() => mockOrganization);
(useApiContacts as jest.Mock).mockReturnValue({ getOrganization });

(usePersonDetail as jest.Mock).mockReturnValue({ person: mockPerson });

const updatePerson = jest.fn();
(useUpdateContact as jest.Mock).mockReturnValue({ updatePerson });

describe('UpdatePersonForm', () => {
  const setup = (renderOptions: RenderOptions & { id: number } = { id: 1 }) => {
    // render component under test
    const component = render(<UpdatePersonForm id={renderOptions.id} />, {
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
    updatePerson.mockReset();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    const fragment = await waitFor(() => asFragment());
    await act(async () => expect(fragment).toMatchSnapshot());
  });

  describe('when Cancel button is clicked', () => {
    it('should cancel the form and navigate to Contacts Details view', async () => {
      const { getCancelButton } = setup();
      const cancel = getCancelButton();
      act(() => userEvent.click(cancel));
      await act(async () => expect(history.location.pathname).toBe(`/contact/P${mockPerson.id}`));
    });
  });

  describe('when Save button is clicked', () => {
    it('should save the form with minimal data', async () => {
      const { getSaveButton } = setup();
      const save = getSaveButton();
      act(() => userEvent.click(save));

      await waitFor(() => expect(updatePerson).toBeCalledWith(mockPerson));
    });

    it('should save the form with updated values', async () => {
      const { getSaveButton, container } = setup();

      const newValues: IEditablePerson = {
        ...mockPerson,
        firstName: 'UpdatedName',
        surname: 'UpdatedLastname',
        contactMethods: [
          {
            contactMethodTypeCode: { id: ContactMethodTypes.PersonalEmail },
            value: 'newaddress@test.com',
          },
        ],
      };

      // provide required fields
      await fillInput(container, 'firstName', newValues.firstName);
      await fillInput(container, 'surname', newValues.surname);
      await fillInput(
        container,
        'emailContactMethods.0.value',
        newValues?.contactMethods?.[0].value,
      );
      await fillInput(
        container,
        'emailContactMethods.0.contactMethodTypeCode',
        newValues?.contactMethods?.[0].contactMethodTypeCode?.id,
        'select',
      );

      const save = getSaveButton();
      act(() => userEvent.click(save));

      await waitFor(() => expect(updatePerson).toBeCalledWith(newValues));
    });
  });
});

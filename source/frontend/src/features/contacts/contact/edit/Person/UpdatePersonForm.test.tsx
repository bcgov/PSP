import { ContactMethodTypes } from 'constants/contactMethodType';
import { AddressTypes } from 'constants/index';
import { usePersonDetail } from 'features/contacts/hooks/usePersonDetail';
import useUpdateContact from 'features/contacts/hooks/useUpdateContact';
import { createMemoryHistory } from 'history';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import {
  IEditableContactMethod,
  IEditableOrganization,
  IEditablePerson,
  IEditablePersonAddress,
} from 'interfaces/editable-contact';
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

const mockContactMethod: IEditableContactMethod = {
  contactMethodTypeCode: {
    id: 'WORKEMAIL',
  },
  value: 'test@test.com',
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
  contactMethods: [mockContactMethod],
};

const mockAddress: IEditablePersonAddress = {
  streetAddress1: 'Test Street',
  streetAddress2: '',
  streetAddress3: '',
  municipality: 'Amsterdam',
  provinceId: undefined,
  countryId: 4,
  countryOther: 'Netherlands',
  postal: '123456',
  addressTypeId: { id: AddressTypes.Mailing },
};

// Mock API service calls
jest.mock('hooks/pims-api/useApiContacts');
jest.mock('features/contacts/hooks/usePersonDetail');
jest.mock('features/contacts/hooks/useUpdateContact');

const getOrganization = jest.fn(() => mockOrganization);
(useApiContacts as jest.Mock).mockReturnValue({ getOrganization });

const mockUsePersonDetail = usePersonDetail as jest.MockedFunction<typeof usePersonDetail>;

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
    mockUsePersonDetail.mockReturnValue({ person: mockPerson });
  });

  afterEach(() => {
    getOrganization.mockClear();
    updatePerson.mockClear();
    mockUsePersonDetail.mockClear();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
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
      await act(() => userEvent.click(save));

      expect(updatePerson).toBeCalledWith(mockPerson);
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
      await act(async () => {
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
      });

      const save = getSaveButton();
      await act(() => userEvent.click(save));

      expect(updatePerson).toBeCalledWith(newValues);
    });

    it(`should save the form with address information when 'Other' country selected and no province is supplied`, async () => {
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
        addresses: [{ ...mockAddress }],
      };

      // provide required fields
      await act(async () => {
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
        await fillInput(container, 'mailingAddress.streetAddress1', mockAddress.streetAddress1);
        await fillInput(container, 'mailingAddress.municipality', mockAddress.municipality);
      });

      // wait for re-render upon changing country to OTHER
      await act(async () => fillInput(container, 'mailingAddress.countryId', 4, 'select'));

      await act(async () => {
        await fillInput(container, 'mailingAddress.countryOther', mockAddress.countryOther);
        await fillInput(container, 'mailingAddress.postal', mockAddress.postal);
      });

      const save = getSaveButton();
      await act(() => userEvent.click(save));

      expect(updatePerson).toBeCalledWith(newValues);
    });
  });
});

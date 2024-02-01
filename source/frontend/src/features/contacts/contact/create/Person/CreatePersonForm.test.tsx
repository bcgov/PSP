import { createMemoryHistory } from 'history';

import { ContactMethodTypes } from '@/constants/contactMethodType';
import { AddressTypes } from '@/constants/index';
import useAddContact from '@/features/contacts/hooks/useAddContact';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import {
  IEditableContactMethod,
  IEditableOrganization,
  IEditablePerson,
  IEditablePersonAddress,
} from '@/interfaces/editable-contact';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

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
      addressTypeId: {
        id: AddressTypes.Mailing,
        description: null,
        isDisabled: false,
        displayOrder: null,
      },
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
      contactMethodTypeCode: {
        id: ContactMethodTypes.WorkEmail,
        description: null,
        isDisabled: false,
        displayOrder: null,
      },
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
  contactMethods: [],
};

const mockContactMethod: IEditableContactMethod = {
  contactMethodTypeCode: {
    id: 'WORKEMAIL',
    description: null,
    isDisabled: false,
    displayOrder: null,
  },
  value: 'test@test.com',
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
  addressTypeId: {
    id: AddressTypes.Mailing,
    description: null,
    isDisabled: false,
    displayOrder: null,
  },
};

// Mock API service calls
jest.mock('@/hooks/pims-api/useApiContacts');
jest.mock('@/features/contacts/hooks/useAddContact');

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
    expect(asFragment()).toMatchSnapshot();
  });

  describe('when Cancel button is clicked', () => {
    it('should cancel the form and navigate to Contacts List view', async () => {
      const { getCancelButton } = setup();
      const cancel = getCancelButton();
      await act(async () => userEvent.click(cancel));
      await waitFor(() => expect(history.location.pathname).toBe('/contact/list'));
    });
  });

  describe('when Save button is clicked', () => {
    it('should save the form with minimal data', async () => {
      const expectedFormData: IEditablePerson = {
        ...mockPerson,
        contactMethods: [{ ...mockContactMethod }],
      };

      addPerson.mockResolvedValue({ ...expectedFormData, id: 1 });
      const { getSaveButton, container } = setup();

      // provide required fields
      await act(async () => {
        await fillInput(container, 'firstName', 'Chester');
        await fillInput(container, 'surname', 'Tester');
        await fillInput(container, 'emailContactMethods.0.value', 'test@test.com');
        await fillInput(
          container,
          'emailContactMethods.0.contactMethodTypeCode',
          ContactMethodTypes.WorkEmail,
          'select',
        );
      });

      const save = getSaveButton();
      await act(async () => userEvent.click(save));

      expect(addPerson).toBeCalledWith(expectedFormData, expect.anything(), false);
      expect(history.location.pathname).toBe('/contact/P1');
    });

    it(`should save the form with address information when 'Other' country selected and no province is supplied`, async () => {
      const expectedFormData: IEditablePerson = {
        ...mockPerson,
        addresses: [{ ...mockAddress }],
      };

      const { getSaveButton, container } = setup();

      // provide required fields
      await act(async () => {
        await fillInput(container, 'firstName', 'Chester');
        await fillInput(container, 'surname', 'Tester');
        await fillInput(container, 'mailingAddress.streetAddress1', mockAddress.streetAddress1);
        await fillInput(container, 'mailingAddress.municipality', mockAddress.municipality);
        await fillInput(container, 'mailingAddress.countryId', 4, 'select');
      });

      await act(async () => {
        await fillInput(container, 'mailingAddress.countryOther', mockAddress.countryOther);
        await fillInput(container, 'mailingAddress.postal', mockAddress.postal);
      });

      const save = getSaveButton();
      await act(async () => userEvent.click(save));

      expect(addPerson).toBeCalledWith(expectedFormData, expect.anything(), false);
      expect(history.location.pathname).toBe('/contact/P1');
    });
  });
});

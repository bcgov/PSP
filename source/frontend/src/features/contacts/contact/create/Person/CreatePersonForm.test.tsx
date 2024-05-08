import { createMemoryHistory } from 'history';

import { ContactMethodTypes } from '@/constants/contactMethodType';
import { AddressTypes } from '@/constants/index';
import useAddContact from '@/features/contacts/hooks/useAddContact';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { getEmptyAddress } from '@/mocks/address.mock';
import { getEmptyContactMethod, getEmptyPerson } from '@/mocks/contacts.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_ContactMethod } from '@/models/api/generated/ApiGen_Concepts_ContactMethod';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { ApiGen_Concepts_PersonAddress } from '@/models/api/generated/ApiGen_Concepts_PersonAddress';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import CreatePersonForm from './CreatePersonForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockOrganization: ApiGen_Concepts_Organization = {
  ...getEmptyOrganization(),
  id: 200,
  isDisabled: false,
  name: 'FooBarBaz Property Management',
  alias: '',
  incorporationNumber: 'BC123456789',
  comment: 'I got comments for you',
  rowVersion: null,
  organizationPersons: null,
  organizationAddresses: [
    {
      id: 1,
      rowVersion: 2,
      organizationId: 200,
      addressUsageType: {
        id: AddressTypes.Mailing,
        description: null,
        isDisabled: false,
        displayOrder: null,
      },
      address: {
        ...getEmptyAddress(),
        streetAddress1: '3000 Main Ave',
        streetAddress2: '',
        streetAddress3: '',
        municipality: 'Vancouver',
        provinceStateId: 1,
        countryId: 1,
        postal: 'V8V1A1',
        rowVersion: 3,
      },
    },
  ],
  contactMethods: [
    {
      id: 1,
      rowVersion: null,
      contactMethodType: {
        id: ContactMethodTypes.WorkEmail,
        description: null,
        isDisabled: false,
        displayOrder: null,
      },
      value: 'foo@bar.com',
      personId: null,
      organizationId: 200,
    },
  ],
};

const mockPerson: ApiGen_Concepts_Person = {
  ...getEmptyPerson(),
  isDisabled: false,
  firstName: 'Chester',
  middleNames: '',
  surname: 'Tester',
  preferredName: '',
  comment: '',
  personOrganizations: null,
  personAddresses: [],
  contactMethods: [],
  useOrganizationAddress: false,
};

const mockContactMethod: ApiGen_Concepts_ContactMethod = {
  ...getEmptyContactMethod(),
  contactMethodType: {
    id: 'WORKEMAIL',
    description: null,
    isDisabled: false,
    displayOrder: null,
  },
  value: 'test@test.com',
};

const mockAddress: ApiGen_Concepts_PersonAddress = {
  id: 0,
  personId: 0,
  rowVersion: null,
  address: {
    ...getEmptyAddress(),
    streetAddress1: 'Test Street',
    streetAddress2: '',
    streetAddress3: '',
    municipality: 'Amsterdam',
    provinceStateId: null,
    countryId: 4,
    countryOther: 'Netherlands',
    postal: '123456',
  },
  addressUsageType: {
    id: AddressTypes.Mailing,
    description: null,
    isDisabled: false,
    displayOrder: null,
  },
};

// Mock API service calls
vi.mock('@/hooks/pims-api/useApiContacts');
vi.mock('@/features/contacts/hooks/useAddContact');

const getOrganization = vi.fn(() => mockOrganization);
vi.mocked(useApiContacts).mockReturnValue({ getOrganization } as unknown as ReturnType<
  typeof useApiContacts
>);

const addPerson = vi.fn();

vi.mocked(useAddContact).mockReturnValue({ addPerson } as unknown as ReturnType<
  typeof useAddContact
>);

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
    await act(async () => {});
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
      const expectedFormData: ApiGen_Concepts_Person = {
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
      const expectedFormData: ApiGen_Concepts_Person = {
        ...mockPerson,
        personAddresses: [{ ...mockAddress }],
      };

      addPerson.mockResolvedValue({ ...expectedFormData, id: 1 });
      const { getSaveButton, container } = setup();

      // provide required fields
      await act(async () => {
        await fillInput(container, 'firstName', 'Chester');
        await fillInput(container, 'surname', 'Tester');
        await fillInput(
          container,
          'mailingAddress.streetAddress1',
          mockAddress.address?.streetAddress1,
        );
        await fillInput(
          container,
          'mailingAddress.municipality',
          mockAddress.address?.municipality,
        );
        await fillInput(container, 'mailingAddress.countryId', 4, 'select');
      });

      await act(async () => {
        await fillInput(
          container,
          'mailingAddress.countryOther',
          mockAddress.address?.countryOther,
        );
        await fillInput(container, 'mailingAddress.postal', mockAddress.address?.postal);
      });

      const save = getSaveButton();
      await act(async () => userEvent.click(save));

      expect(addPerson).toBeCalledWith(expectedFormData, expect.anything(), false);
      expect(history.location.pathname).toBe('/contact/P1');
    });
  });
});

import { createMemoryHistory } from 'history';

import { ContactMethodTypes } from '@/constants/contactMethodType';
import { AddressTypes } from '@/constants/index';
import { usePersonDetail } from '@/features/contacts/hooks/usePersonDetail';
import useUpdateContact from '@/features/contacts/hooks/useUpdateContact';
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
import { act, fillInput, render, RenderOptions, userEvent } from '@/utils/test-utils';

import UpdatePersonForm from './UpdatePersonForm';

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
        id: 1,
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
        displayOrder: null,
        isDisabled: false,
      },
      value: 'foo@bar.com',
      personId: null,
      organizationId: 200,
    },
  ],
};

const mockContactMethod: ApiGen_Concepts_ContactMethod = {
  ...getEmptyContactMethod(),
  contactMethodType: {
    id: 'WORKEMAIL',
    description: null,
    displayOrder: null,
    isDisabled: false,
  },
  value: 'test@test.com',
  rowVersion: 0,
};

const mockPerson: ApiGen_Concepts_Person = {
  ...getEmptyPerson(),
  id: 1,
  isDisabled: false,
  firstName: 'Chester',
  middleNames: '',
  surname: 'Tester',
  preferredName: '',
  comment: 'Lorem ipsum',
  personOrganizations: [
    {
      id: 1,
      rowVersion: 7,
      organizationId: mockOrganization.id,
      personId: 1,
      person: null,
      organization: mockOrganization,
    },
  ],
  personAddresses: [],
  contactMethods: [mockContactMethod],
  useOrganizationAddress: false,
};

const mockAddress: ApiGen_Concepts_PersonAddress = {
  personId: 1,
  rowVersion: null,
  id: 1,
  addressUsageType: {
    id: AddressTypes.Mailing,
    description: null,
    displayOrder: null,
    isDisabled: false,
  },
  address: {
    ...getEmptyAddress(),
    id: 1,
    streetAddress1: 'Test Street',
    streetAddress2: '',
    streetAddress3: '',
    municipality: 'Amsterdam',
    provinceStateId: null,
    countryId: 4,
    countryOther: 'Netherlands',
    postal: '123456',
  },
};

// Mock API service calls
vi.mock('@/hooks/pims-api/useApiContacts');
vi.mock('@/features/contacts/hooks/usePersonDetail');
vi.mock('@/features/contacts/hooks/useUpdateContact');

const getOrganization = vi.fn(() => mockOrganization);
vi.mocked(useApiContacts).mockReturnValue({ getOrganization } as unknown as ReturnType<
  typeof useApiContacts
>);

const mockUsePersonDetail = vi.mocked(usePersonDetail);

const updatePerson = vi.fn();
vi.mocked(useUpdateContact).mockReturnValue({ updatePerson } as unknown as ReturnType<
  typeof useUpdateContact
>);

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
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders 2 address lines if provided', async () => {
    mockUsePersonDetail.mockClear();
    mockUsePersonDetail.mockReturnValue({
      person: {
        ...mockPerson,
        personAddresses: [
          {
            ...mockAddress,
            address: {
              ...mockAddress.address!,
              streetAddress2: 'line 2',
            },
          },
        ],
      },
    });
    const { findByDisplayValue } = setup();
    expect(await findByDisplayValue('line 2')).toBeVisible();
  });

  it('renders 3 address lines if provided', async () => {
    mockUsePersonDetail.mockClear();
    mockUsePersonDetail.mockReturnValue({
      person: {
        ...mockPerson,
        personAddresses: [
          {
            ...mockAddress,
            address: {
              ...mockAddress.address!,
              streetAddress3: 'line 3',
              streetAddress2: 'line 2',
              streetAddress1: mockAddress.address!.streetAddress1,
            },
          },
        ],
      },
    });
    const { findByDisplayValue } = setup();
    expect(await findByDisplayValue('line 2')).toBeVisible();
    expect(await findByDisplayValue('line 3')).toBeVisible();
  });

  describe('when Cancel button is clicked', () => {
    it('should cancel the form and navigate to Contacts Details view', async () => {
      const { getCancelButton } = setup();
      const cancel = getCancelButton();
      await act(async () => userEvent.click(cancel));
      expect(history.location.pathname).toBe(`/contact/P${mockPerson.id}`);
    });
  });

  describe('when Save button is clicked', () => {
    it('should save the form with minimal data', async () => {
      const { getSaveButton } = setup();
      const save = getSaveButton();
      await act(async () => userEvent.click(save));

      const expectedPerson = { ...mockPerson };
      expectedPerson!.personOrganizations![0].organization = null;
      expect(updatePerson).toBeCalledWith(expectedPerson);
    });

    it('should save the form with updated values', async () => {
      mockUsePersonDetail.mockReturnValue({
        person: {
          ...mockPerson,
          personOrganizations: [
            { ...mockPerson!.personOrganizations![0], organization: mockOrganization },
          ],
        },
      });

      const { getSaveButton, container } = setup();

      const newContactMethod = mockPerson!.contactMethods![0];
      newContactMethod.value = 'newaddress@test.com';

      const newValues: ApiGen_Concepts_Person = {
        ...mockPerson,
        firstName: 'UpdatedName',
        surname: 'UpdatedLastname',
        personOrganizations: [{ ...mockPerson!.personOrganizations![0], organization: null }],
        contactMethods: [newContactMethod],
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
          newValues?.contactMethods?.[0].contactMethodType?.id,
          'select',
        );
      });

      const save = getSaveButton();
      await act(async () => userEvent.click(save));

      expect(updatePerson).toBeCalledWith(newValues);
    });

    it(`should save the form with address information when 'Other' country selected and no province is supplied`, async () => {
      mockUsePersonDetail.mockReturnValue({
        person: {
          ...mockPerson,
          personAddresses: [{ ...mockAddress }],
          personOrganizations: [
            { ...mockPerson!.personOrganizations![0], organization: mockOrganization },
          ],
        },
      });

      const { getSaveButton, container } = setup();

      const newValues: ApiGen_Concepts_Person = {
        ...mockPerson,
        firstName: 'UpdatedName',
        surname: 'UpdatedLastname',
        contactMethods: [
          {
            ...mockPerson!.contactMethods![0],
            contactMethodType: {
              id: ContactMethodTypes.PersonalEmail,
              description: null,
              displayOrder: null,
              isDisabled: false,
            },
            value: 'newaddress@test.com',
          },
        ],
        personOrganizations: [{ ...mockPerson!.personOrganizations![0], organization: null }],
        personAddresses: [{ ...mockAddress }],
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
          newValues?.contactMethods?.[0].contactMethodType?.id,
          'select',
        );
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
      });

      // wait for re-render upon changing country to OTHER
      fillInput(container, 'mailingAddress.countryId', 4, 'select');

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

      expect(updatePerson).toHaveBeenCalledWith(newValues);
    });
  });
});

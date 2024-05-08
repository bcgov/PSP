import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';

import { ContactMethodTypes } from '@/constants/contactMethodType';
import { AddressTypes } from '@/constants/index';
import { useOrganizationDetail } from '@/features/contacts/hooks/useOrganizationDetail';
import { useUpdateContact } from '@/features/contacts/hooks/useUpdateContact';
import { getEmptyAddress } from '@/mocks/address.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_OrganizationAddress } from '@/models/api/generated/ApiGen_Concepts_OrganizationAddress';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { toTypeCode } from '@/utils/formUtils';
import { act, fillInput, render, RenderOptions } from '@/utils/test-utils';

import UpdateOrganizationForm from './UpdateOrganizationForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockOrganization: ApiGen_Concepts_Organization = {
  ...getEmptyOrganization(),
  id: 1,
  isDisabled: false,
  name: 'FooBarBaz Property Management',
  alias: '',
  incorporationNumber: 'BC123456789',
  comment: 'I got comments for you',
  organizationPersons: null,
  organizationAddresses: null,
  contactMethods: [
    {
      id: 1,
      rowVersion: 7,
      contactMethodType: toTypeCode(ContactMethodTypes.WorkEmail),
      value: 'foo@bar.com',
      personId: null,
      organizationId: 1,
    },
  ],
};

const mockAddress: ApiGen_Concepts_OrganizationAddress = {
  id: 1,
  rowVersion: null,
  organizationId: 9,
  addressUsageType: toTypeCode(AddressTypes.Mailing),
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
};

// Mock API service calls
vi.mock('@/features/contacts/hooks/useOrganizationDetail');
vi.mock('@/features/contacts/hooks/useUpdateContact');

const getOrganization = vi.mocked(useOrganizationDetail);

const updateOrganization = vi.fn();
vi.mocked(useUpdateContact).mockReturnValue({
  updateOrganization,
  updatePerson: vi.fn(),
});

describe('UpdateOrganizationForm', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { id: number } = { id: 1 }) => {
    const component = render(<UpdateOrganizationForm id={renderOptions.id} />, {
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
    getOrganization.mockReturnValue({
      organization: mockOrganization,
    });
    updateOrganization.mockClear();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup({ id: 1 });
    expect(asFragment()).toMatchSnapshot();
  });

  describe('when Cancel button is clicked', () => {
    it('should cancel the form and navigate to Contacts Details view', async () => {
      const { getCancelButton } = setup({ id: 1 });
      const cancel = getCancelButton();
      await act(async () => userEvent.click(cancel));
      expect(history.location.pathname).toBe('/contact/O1');
    });
  });

  describe('when Save button is clicked', () => {
    it('should update the organization with minimal data', async () => {
      const { getSaveButton } = setup({ id: 1 });
      const save = getSaveButton();
      await act(async () => userEvent.click(save));
      expect(updateOrganization).toBeCalledWith(mockOrganization);
    });

    it('should save the organization with new values', async () => {
      const newValues: ApiGen_Concepts_Organization = {
        ...mockOrganization,
        name: 'RandomName Property Management',
        contactMethods: [
          {
            id: 1,
            contactMethodType: toTypeCode(ContactMethodTypes.PersonalEmail),
            value: 'test@test.com',
            rowVersion: 7,
            personId: null,
            organizationId: mockOrganization.id,
          },
        ],
      };
      const { getSaveButton, container } = setup({ id: 1 });

      // change some fields
      await act(async () => {
        await fillInput(container, 'name', newValues.name);
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

      expect(updateOrganization).toBeCalledWith(newValues);
    });

    it(`should save the form with address information when 'Other' country selected and no province is supplied`, async () => {
      const retrievedOrganization = {
        ...mockOrganization,
        name: 'RandomName Property Management',
        organizationAddresses: [mockAddress],
      };
      const newValues: ApiGen_Concepts_Organization = {
        ...retrievedOrganization,
        name: 'RandomName Property Management',
      };

      getOrganization.mockReturnValue({
        organization: retrievedOrganization,
      });
      const { getSaveButton, container } = setup({ id: 1 });

      // change some fields
      await act(async () => {
        await fillInput(container, 'name', newValues.name);
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

        // wait for re-render upon changing country to OTHER
        fillInput(container, 'mailingAddress.countryId', 4, 'select');

        await fillInput(
          container,
          'mailingAddress.countryOther',
          mockAddress.address?.countryOther,
        );
        await fillInput(container, 'mailingAddress.postal', mockAddress.address?.postal);
      });

      const save = getSaveButton();
      await act(async () => userEvent.click(save));

      expect(updateOrganization).toBeCalledWith(newValues);
    });
  });
});

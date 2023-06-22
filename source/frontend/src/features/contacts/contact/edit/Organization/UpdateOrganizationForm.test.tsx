import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';

import { ContactMethodTypes } from '@/constants/contactMethodType';
import { AddressTypes } from '@/constants/index';
import { useOrganizationDetail } from '@/features/contacts/hooks/useOrganizationDetail';
import { useUpdateContact } from '@/features/contacts/hooks/useUpdateContact';
import { IEditableOrganization, IEditableOrganizationAddress } from '@/interfaces/editable-contact';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, waitFor } from '@/utils/test-utils';

import UpdateOrganizationForm from './UpdateOrganizationForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockOrganization: IEditableOrganization = {
  id: 1,
  isDisabled: false,
  name: 'FooBarBaz Property Management',
  alias: '',
  incorporationNumber: 'BC123456789',
  comment: 'I got comments for you',
  persons: undefined,
  addresses: [],
  contactMethods: [
    { contactMethodTypeCode: { id: ContactMethodTypes.WorkEmail }, value: 'foo@bar.com' },
  ],
};

const mockAddress: IEditableOrganizationAddress = {
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
jest.mock('@/features/contacts/hooks/useOrganizationDetail');
jest.mock('@/features/contacts/hooks/useUpdateContact');

(useOrganizationDetail as jest.MockedFunction<typeof useOrganizationDetail>).mockReturnValue({
  organization: mockOrganization,
});

const updateOrganization = jest.fn();
(useUpdateContact as jest.MockedFunction<typeof useUpdateContact>).mockReturnValue({
  updateOrganization,
  updatePerson: jest.fn(),
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
    updateOrganization.mockClear();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup({ id: 1 });
    const fragment = await waitFor(() => asFragment());
    await act(async () => expect(fragment).toMatchSnapshot());
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
      const newValues: IEditableOrganization = {
        ...mockOrganization,
        name: 'RandomName Property Management',
        contactMethods: [
          {
            contactMethodTypeCode: { id: ContactMethodTypes.PersonalEmail },
            value: 'test@test.com',
          },
        ],
      };
      const { getSaveButton, container } = setup({ id: 1 });

      // change some fields
      await fillInput(container, 'name', newValues.name);
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
      await act(async () => userEvent.click(save));

      expect(updateOrganization).toBeCalledWith(newValues);
    });

    it(`should save the form with address information when 'Other' country selected and no province is supplied`, async () => {
      const newValues: IEditableOrganization = {
        ...mockOrganization,
        name: 'RandomName Property Management',
        addresses: [mockAddress],
      };
      const { getSaveButton, container } = setup({ id: 1 });

      // change some fields
      await fillInput(container, 'name', newValues.name);
      await fillInput(container, 'mailingAddress.streetAddress1', mockAddress.streetAddress1);
      await fillInput(container, 'mailingAddress.municipality', mockAddress.municipality);

      // wait for re-render upon changing country to OTHER
      await act(async () => fillInput(container, 'mailingAddress.countryId', 4, 'select'));

      await fillInput(container, 'mailingAddress.countryOther', mockAddress.countryOther);
      await fillInput(container, 'mailingAddress.postal', mockAddress.postal);

      const save = getSaveButton();
      await act(async () => userEvent.click(save));

      expect(updateOrganization).toBeCalledWith(newValues);
    });
  });
});

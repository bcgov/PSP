import userEvent from '@testing-library/user-event';
import { ContactMethodTypes } from 'constants/contactMethodType';
import { useOrganizationDetail } from 'features/contacts/hooks/useOrganizationDetail';
import { useUpdateContact } from 'features/contacts/hooks/useUpdateContact';
import { createMemoryHistory } from 'history';
import { IEditableOrganization } from 'interfaces/editable-contact';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, render, RenderOptions, waitFor } from 'utils/test-utils';

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
  addresses: [],
  contactMethods: [
    { contactMethodTypeCode: { id: ContactMethodTypes.WorkEmail }, value: 'foo@bar.com' },
  ],
};

// Mock API service calls
jest.mock('features/contacts/hooks/useOrganizationDetail');
jest.mock('features/contacts/hooks/useUpdateContact');

(useOrganizationDetail as jest.Mock<ReturnType<typeof useOrganizationDetail>>).mockReturnValue({
  organization: mockOrganization,
});

const updateOrganization = jest.fn();
(useUpdateContact as jest.Mock<ReturnType<typeof useUpdateContact>>).mockReturnValue({
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
    updateOrganization.mockReset();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup({ id: 1 });
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  describe('when Cancel button is clicked', () => {
    it('should cancel the form and navigate to Contacts Details view', async () => {
      const { getCancelButton } = setup({ id: 1 });
      const cancel = getCancelButton();
      userEvent.click(cancel);

      await waitFor(() => expect(history.location.pathname).toBe('/contact/O1'));
    });
  });

  describe('when Save button is clicked', () => {
    it('should update the organization with minimal data', async () => {
      const { getSaveButton } = setup({ id: 1 });
      const save = getSaveButton();
      userEvent.click(save);

      await waitFor(() => expect(updateOrganization).toBeCalledWith(mockOrganization));
    });

    it('should save the organization with new values', async () => {
      const newValues: Partial<IEditableOrganization> = {
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
      userEvent.click(save);

      await waitFor(() =>
        expect(updateOrganization).toBeCalledWith({ ...mockOrganization, ...newValues }),
      );
    });
  });
});

import { createMemoryHistory } from 'history';
import { Route } from 'react-router-dom';

import { ContactMethodTypes } from '@/constants/contactMethodType';
import { useOrganizationDetail } from '@/features/contacts/hooks/useOrganizationDetail';
import { usePersonDetail } from '@/features/contacts/hooks/usePersonDetail';
import { IEditableOrganization, IEditablePerson } from '@/interfaces/editable-contact';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { toTypeCode } from '@/utils/formUtils';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import UpdateContactContainer from './UpdateContactContainer';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockPerson: IEditablePerson = {
  id: 1,
  isDisabled: false,
  firstName: 'Chester',
  surname: 'Tester',
  comment: 'I got comments for you',
  organization: null,
  useOrganizationAddress: false,
  addresses: [],
  contactMethods: [
    {
      contactMethodTypeCode: toTypeCode(ContactMethodTypes.PersonalEmail),
      value: 'foo@bar.com',
    },
  ],
};

const mockOrganization: IEditableOrganization = {
  id: 1,
  isDisabled: false,
  name: 'FooBarBaz Property Management',
  alias: '',
  incorporationNumber: 'BC123456789',
  comment: 'I got comments for you',
  persons: [],
  addresses: [],
  contactMethods: [
    { contactMethodTypeCode: toTypeCode(ContactMethodTypes.WorkEmail), value: 'foo@bar.com' },
  ],
};

// Mock API service calls
jest.mock('@/features/contacts/hooks/useOrganizationDetail');
jest.mock('@/features/contacts/hooks/usePersonDetail');

(useOrganizationDetail as jest.Mock<ReturnType<typeof useOrganizationDetail>>).mockReturnValue({
  organization: mockOrganization,
});

(usePersonDetail as jest.Mock<ReturnType<typeof usePersonDetail>>).mockReturnValue({
  person: mockPerson,
});

describe('UpdateContactContainer', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions = {}) => {
    const component = render(
      <Route exact path="/contact/:id/edit">
        <UpdateContactContainer />
      </Route>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...component,
      getSaveButton: () => component.getByText('Save'),
      getCancelButton: () => component.getByText('Cancel'),
    };
  };

  beforeEach(() => {
    history.push('/contact/P1/edit');
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('should render the update person view when contact is a person', async () => {
    const { findByText } = setup();
    expect(await findByText(/First name/i)).toBeVisible();
  });

  it('should render the update organization view when contact is an organization', async () => {
    history.push('/contact/O1/edit');
    const { findByText } = setup();
    expect(await findByText(/Organization name/i)).toBeVisible();
  });

  describe('when Cancel button is clicked', () => {
    it('should cancel the form and navigate to Contacts Details view', async () => {
      const { getCancelButton } = setup();
      await act(async () => userEvent.click(getCancelButton()));
      await waitFor(() => expect(history.location.pathname).toBe('/contact/P1'));
    });
  });
});

import { createMemoryHistory } from 'history';
import { Route } from 'react-router-dom';

import { ContactMethodTypes } from '@/constants/contactMethodType';
import { useOrganizationDetail } from '@/features/contacts/hooks/useOrganizationDetail';
import { usePersonDetail } from '@/features/contacts/hooks/usePersonDetail';
import { getEmptyPerson } from '@/mocks/contacts.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { toTypeCode } from '@/utils/formUtils';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import UpdateContactContainer from './UpdateContactContainer';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockPerson: ApiGen_Concepts_Person = {
  ...getEmptyPerson(),
  id: 1,
  isDisabled: false,
  firstName: 'Chester',
  surname: 'Tester',
  comment: 'I got comments for you',
  personAddresses: [],
  contactMethods: [
    {
      id: 1,
      rowVersion: null,
      contactMethodType: toTypeCode(ContactMethodTypes.PersonalEmail),
      value: 'foo@bar.com',
      personId: 1,
      organizationId: null,
    },
  ],
};

const mockOrganization: ApiGen_Concepts_Organization = {
  ...getEmptyOrganization(),
  id: 1,
  isDisabled: false,
  name: 'FooBarBaz Property Management',
  alias: '',
  incorporationNumber: 'BC123456789',
  comment: 'I got comments for you',
  organizationPersons: [],
  organizationAddresses: [],
  contactMethods: [
    {
      rowVersion: null,
      id: 1,
      contactMethodType: toTypeCode(ContactMethodTypes.WorkEmail),
      value: 'foo@bar.com',
      personId: null,
      organizationId: 1,
    },
  ],
};

// Mock API service calls
vi.mock('@/features/contacts/hooks/useOrganizationDetail');
vi.mock('@/features/contacts/hooks/usePersonDetail');

vi.mocked(useOrganizationDetail).mockReturnValue({
  organization: mockOrganization,
});

vi.mocked(usePersonDetail).mockReturnValue({
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
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('should render the update person view when contact is a person', async () => {
    const { findByText } = setup();
    await act(async () => {});
    expect(await findByText(/First name/i)).toBeVisible();
  });

  it('should render the update organization view when contact is an organization', async () => {
    history.push('/contact/O1/edit');
    const { findByText } = setup();
    await act(async () => {});
    expect(await findByText(/Organization name/i)).toBeVisible();
  });

  describe('when Cancel button is clicked', () => {
    it('should cancel the form and navigate to Contacts Details view', async () => {
      const { getCancelButton } = setup();
      await act(async () => {});
      await act(async () => userEvent.click(getCancelButton()));
      await waitFor(() => expect(history.location.pathname).toBe('/contact/P1'));
    });
  });
});

import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { Claims } from '@/constants/index';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_PropertyContact } from '@/models/api/Property';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

import { PropertyContactListView } from './PropertyContactListView';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

// mock keycloak auth library
jest.mock('@react-keycloak/web');

describe('PropertyContactListView component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & { propertyContacts: Api_PropertyContact[] } = {
      propertyContacts: mockPropertyContacts,
    },
  ) => {
    const { ...rest } = renderOptions;
    const component = render(
      <PropertyContactListView
        isLoading={false}
        propertyContacts={renderOptions.propertyContacts}
        setEditManagementState={noop}
        onDelete={noop}
      />,
      {
        ...rest,
        store: storeState,
        claims: [Claims.PROPERTY_EDIT],
        history,
      },
    );

    return {
      ...component,
    };
  };

  it('renders as expected when provided valid data object', () => {
    const { asFragment } = setup({ propertyContacts: mockPropertyContacts });
    expect(asFragment()).toMatchSnapshot();
  });

  it('Shows primary contact message if contact is a person', () => {
    const testContacts: Api_PropertyContact[] = [
      { ...mockPropertyContacts[0], primaryContact: null },
    ];
    const { getByText } = setup({ propertyContacts: testContacts });

    expect(getByText('Not applicable')).toBeVisible();
  });

  it('Shows primary contact message if contact is an organization', () => {
    const testContacts: Api_PropertyContact[] = [
      {
        ...emptyPropertyContact,
        person: null,
        organization: { id: 1, name: 'Org name' },
        primaryContact: null,
      },
    ];
    const { getByText } = setup({ propertyContacts: testContacts });

    expect(getByText('No contacts available')).toBeVisible();
  });
});

const emptyPropertyContact: Api_PropertyContact = {
  id: 0,
  propertyId: 0,
  organizationId: null,
  organization: null,
  personId: null,
  person: null,
  primaryContactId: null,
  primaryContact: null,
  purpose: null,
  rowVersion: 0,
};

const mockPropertyContacts: Api_PropertyContact[] = [
  {
    ...emptyPropertyContact,
    id: 1,
    person: {
      id: 1,
      isDisabled: false,
      surname: 'Doe',
      firstName: 'John',
      middleNames: '',
      preferredName: '',
      personOrganizations: [],
      personAddresses: [],
      contactMethods: [],
      comment: '',
      rowVersion: 0,
    },
    purpose: 'Test Purpouse',
  },
];

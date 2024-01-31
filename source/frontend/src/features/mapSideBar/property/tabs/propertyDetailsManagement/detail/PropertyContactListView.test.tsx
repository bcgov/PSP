import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { Claims } from '@/constants/index';
import { mockLookups } from '@/mocks/lookups.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_PropertyContact } from '@/models/api/generated/ApiGen_Concepts_PropertyContact';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
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
    renderOptions: RenderOptions & { propertyContacts: ApiGen_Concepts_PropertyContact[] } = {
      propertyContacts: mockPropertyContacts,
    },
  ) => {
    const { ...rest } = renderOptions;
    const component = render(
      <PropertyContactListView
        isLoading={false}
        propertyContacts={renderOptions.propertyContacts}
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
    const testContacts: ApiGen_Concepts_PropertyContact[] = [
      { ...mockPropertyContacts[0], primaryContact: null },
    ];
    const { getByText } = setup({ propertyContacts: testContacts });

    expect(getByText('Not applicable')).toBeVisible();
  });

  it('Shows primary contact message if contact is an organization', () => {
    const testContacts: ApiGen_Concepts_PropertyContact[] = [
      {
        ...emptyPropertyContact,
        person: null,
        organization: { ...getEmptyOrganization(), id: 1, name: 'Org name' },
        primaryContact: null,
      },
    ];
    const { getByText } = setup({ propertyContacts: testContacts });

    expect(getByText('No contacts available')).toBeVisible();
  });
});

const emptyPropertyContact: ApiGen_Concepts_PropertyContact = {
  id: 0,
  propertyId: 0,
  organizationId: null,
  organization: null,
  personId: null,
  person: null,
  primaryContactId: null,
  primaryContact: null,
  purpose: null,
  ...getEmptyBaseAudit(0),
};

const mockPropertyContacts: ApiGen_Concepts_PropertyContact[] = [
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

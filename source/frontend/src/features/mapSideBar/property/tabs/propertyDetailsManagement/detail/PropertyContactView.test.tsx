import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { Claims } from '@/constants/index';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_PropertyContact } from '@/models/api/Property';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

import { PropertyContactView } from './PropertyContactView';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

// mock keycloak auth library
jest.mock('@react-keycloak/web');

describe('PropertyContactView component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & { propertyContacts: Api_PropertyContact[] } = {
      propertyContacts: mockPropertyContacts,
    },
  ) => {
    const { ...rest } = renderOptions;
    const component = render(
      <PropertyContactView
        isLoading={false}
        propertyContacts={renderOptions.propertyContacts}
        setEditMode={noop}
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
});

const mockPropertyContacts: Api_PropertyContact[] = [
  {
    id: 1,
    organization: null,
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
    primaryContact: null,
    purpose: 'Test Purpouse',
  },
];

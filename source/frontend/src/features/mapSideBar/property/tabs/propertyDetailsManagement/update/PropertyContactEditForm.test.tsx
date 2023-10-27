import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { Claims } from '@/constants/index';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_PropertyContact } from '@/models/api/Property';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

import { PropertyContactEditForm } from './PropertyContactEditForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

// mock keycloak auth library
jest.mock('@react-keycloak/web');

describe('PropertyContactEditForm component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & { propertyContact: Api_PropertyContact } = {
      propertyContact: mockPropertyContact,
    },
  ) => {
    const { ...rest } = renderOptions;
    const component = render(
      <PropertyContactEditForm
        isLoading={false}
        propertyContact={renderOptions.propertyContact}
        onSave={noop}
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
    const { asFragment } = setup({ propertyContact: mockPropertyContact });
    expect(asFragment()).toMatchSnapshot();
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

const mockPropertyContact: Api_PropertyContact = {
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
};

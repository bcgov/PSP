import { act, screen } from '@testing-library/react';
import { Claims } from 'constants/claims';
import { createMemoryHistory } from 'history';
import { defaultFormLease } from 'interfaces';
import { mockLookups } from 'mocks';
import { getMockContactOrganizationWithOnePerson, getMockContactPerson } from 'mocks/mockContacts';
import React from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { mockKeycloak, renderAsync, RenderOptions, userEvent } from 'utils/test-utils';

import AddLeaseTenantForm, { IAddLeaseTenantFormProps } from './AddLeaseTenantForm';
import { FormTenant } from './ViewTenantForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

jest.mock('@react-keycloak/web');
const setSelectedContacts = jest.fn();
const setShowContactManager = jest.fn();
const setTenants = jest.fn();
const onSubmit = jest.fn();

const defaultRenderOptions: IAddLeaseTenantFormProps = {
  selectedContacts: [],
  setSelectedContacts,
  setShowContactManager,
  setTenants,
  tenants: [],
  showContactManager: false,
  onSubmit,
  formikRef: React.createRef(),
};

describe('AddLeaseTenantForm component', () => {
  const setup = async (renderOptions: RenderOptions & Partial<IAddLeaseTenantFormProps> = {}) => {
    // render component under test
    const component = await renderAsync(
      <AddLeaseTenantForm
        lease={defaultFormLease}
        {...{ ...defaultRenderOptions, ...renderOptions }}
      ></AddLeaseTenantForm>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );
    return { component };
  };

  beforeEach(() => {
    jest.resetAllMocks();
    mockKeycloak({ claims: [Claims.CONTACT_VIEW] });
  });
  it('renders as expected', async () => {
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('sets display modal prop when button set', async () => {
    const {
      component: { getByText },
    } = await setup({});

    const tenantButton = getByText('Select Tenant(s)');
    act(() => userEvent.click(tenantButton));

    expect(setShowContactManager).toHaveBeenCalledWith(true);
  });

  it('displays modal when prop is set', async () => {
    await setup({ showContactManager: true });

    const modal = screen.getByText('Select a contact');

    expect(modal).toBeVisible();
  });

  it('confirming the modal sets the tenants', async () => {
    await setup({ showContactManager: true });

    const modal = screen.getByText('Select a contact');
    expect(modal).toBeVisible();

    const confirm = screen.getByText('Select');
    act(() => userEvent.click(confirm));

    expect(setShowContactManager).toHaveBeenLastCalledWith(false);
    expect(setTenants).toHaveBeenCalledWith([]);
  });

  it('cancelling the modal resets the tenants', async () => {
    const tenants = [new FormTenant(undefined, getMockContactOrganizationWithOnePerson())];
    await setup({ showContactManager: true, tenants: tenants });

    const modal = screen.getByText('Select a contact');
    expect(modal).toBeVisible();

    const cancel = screen.getByText('Cancel');
    act(() => userEvent.click(cancel));

    expect(setShowContactManager).toHaveBeenLastCalledWith(false);
    expect(setSelectedContacts.mock.calls[0][0][0].id).toBe(tenants[0].id);
  });

  it('displays modal when prop is set', async () => {
    await setup({ showContactManager: true });

    const modal = screen.getByText('Select a contact');

    expect(modal).toBeVisible();
  });

  it('displays the number of previously selected tenants', async () => {
    await setup({
      tenants: [new FormTenant(undefined, getMockContactOrganizationWithOnePerson())],
    });

    const number = screen.getByText('1 Tenants associated with this Lease/License');

    expect(number).toBeVisible();
  });

  it('displays previously selected tenants', async () => {
    await setup({
      tenants: [new FormTenant(undefined, getMockContactOrganizationWithOnePerson())],
    });

    const summary = screen.getByText('Dairy Queen Forever! Property Management');

    expect(summary).toBeVisible();
  });

  it('displays Not applicable for contact when contact is a person', async () => {
    await setup({
      tenants: [new FormTenant(undefined, getMockContactPerson())],
    });

    const contactMsg = screen.getByText('Not applicable');

    expect(contactMsg).toBeVisible();
  });

  it('displays no contacts available if organization has no contacts', async () => {
    const organization = {
      ...getMockContactOrganizationWithOnePerson(),
      organization: { organizationPersons: [] },
    };

    await setup({
      tenants: [new FormTenant(undefined, organization)],
    });

    const contactMsg = screen.getByText('No contacts available');

    expect(contactMsg).toBeVisible();
  });

  it('displays no contacts available if organization has no contacts', async () => {
    const organization = {
      ...getMockContactOrganizationWithOnePerson(),
      organization: { organizationPersons: [] },
    };

    await setup({
      tenants: [new FormTenant(undefined, organization)],
    });

    const contactMsg = screen.getByText('No contacts available');

    expect(contactMsg).toBeVisible();
  });

  it('displays no contacts available if organization has no contacts', async () => {
    const organization = {
      ...getMockContactOrganizationWithOnePerson(),
      organization: { organizationPersons: [] },
    };

    await setup({
      tenants: [new FormTenant(undefined, organization)],
    });

    const contactMsg = screen.getByText('No contacts available');

    expect(contactMsg).toBeVisible();
  });

  it('displays the primary contact if there is only one', async () => {
    const organization = {
      ...getMockContactOrganizationWithOnePerson(),
      organization: {
        organizationPersons: [
          {
            personId: 3,
            organizationId: 3,
            isDisabled: false,
            rowVersion: 1,
            person: { firstName: 'test', surname: 'testerson' },
          },
        ],
      },
    };

    await setup({
      tenants: [new FormTenant(undefined, organization)],
    });

    const contactPerson = screen.getByText('test testerson');

    expect(contactPerson).toBeVisible();
  });

  it('displays a list if there are multiple', async () => {
    const organization = {
      ...getMockContactOrganizationWithOnePerson(),
      organization: {
        organizationPersons: [
          {
            personId: 3,
            organizationId: 3,
            isDisabled: false,
            rowVersion: 1,
            person: { firstName: 'test', surname: 'testerson' },
          },
          {
            personId: 2,
            organizationId: 3,
            isDisabled: false,
            rowVersion: 1,
            person: { firstName: 'second', surname: 'testerson' },
          },
        ],
      },
    };

    await setup({
      tenants: [new FormTenant(undefined, organization)],
    });

    const contactMsg = screen.getByDisplayValue('Select a contact');

    expect(contactMsg).toBeVisible();
  });

  it('can remove previously selected tenants', async () => {
    const {
      component: { getByTitle },
    } = await setup({
      tenants: [new FormTenant(undefined, getMockContactOrganizationWithOnePerson())],
    });

    const deleteButton = getByTitle('Click to remove');
    act(() => userEvent.click(deleteButton));

    expect(setTenants).toHaveBeenCalledWith([]);
    expect(setSelectedContacts).toHaveBeenCalledWith([]);
  });
});

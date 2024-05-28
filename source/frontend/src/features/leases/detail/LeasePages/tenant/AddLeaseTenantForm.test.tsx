import {
  act,
  getByTestId,
  prettyDOM,
  screen,
  waitForElementToBeRemoved,
} from '@testing-library/react';
import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/claims';
import { IContactSearchResult } from '@/interfaces';
import {
  getEmptyPerson,
  getMockContactOrganizationWithOnePerson,
  getMockContactPerson,
} from '@/mocks/contacts.mock';
import { mockLookups } from '@/mocks/index.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_Contact } from '@/models/api/generated/ApiGen_Concepts_Contact';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockKeycloak, renderAsync, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import AddLeaseTenantForm, { IAddLeaseTenantFormProps } from './AddLeaseTenantForm';
import { FormTenant } from './models';
import { createRef } from 'react';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockGetContactsFn = vi
  .fn()
  .mockResolvedValue({ data: {} as ApiGen_Base_Page<ApiGen_Concepts_Contact> });

vi.mock('@/hooks/pims-api/useApiContacts', () => ({
  useApiContacts: () => {
    return {
      getContacts: mockGetContactsFn,
    };
  },
}));

const setSelectedContacts = vi.fn();
const setShowContactManager = vi.fn();
const setSelectedTenants = vi.fn();
const onSubmit = vi.fn();

const defaultRenderOptions: IAddLeaseTenantFormProps = {
  selectedContacts: [],
  setSelectedContacts,
  setShowContactManager,
  setSelectedTenants,
  selectedTenants: [],
  showContactManager: false,
  onSubmit,
  formikRef: createRef(),
};

describe('AddLeaseTenantForm component', () => {
  const setup = async (renderOptions: RenderOptions & Partial<IAddLeaseTenantFormProps> = {}) => {
    // render component under test
    const component = await renderAsync(
      <AddLeaseTenantForm
        {...{
          ...defaultRenderOptions,
          ...renderOptions,
        }}
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
    vi.resetAllMocks();
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
    await act(async () => userEvent.click(tenantButton));

    expect(setShowContactManager).toHaveBeenCalledWith(true);
  });

  it('displays modal when contact prop is set', async () => {
    await act(async () => {
      await setup({ showContactManager: true });
    });
    const modal = screen.getByText('Select a contact');

    expect(modal).toBeVisible();
  });

  it('confirming the modal sets the tenants', async () => {
    await act(async () => {
      await setup({ showContactManager: true });
    });

    const modal = screen.getByText('Select a contact');
    expect(modal).toBeVisible();

    const confirm = screen.getByText('Select');
    await act(async () => userEvent.click(confirm));

    expect(setShowContactManager).toHaveBeenLastCalledWith(false);
    expect(setSelectedTenants).toHaveBeenCalledWith([]);
  });

  it('cancelling the modal resets the tenants', async () => {
    const tenants = [new FormTenant(undefined, getMockContactOrganizationWithOnePerson())];
    await act(async () => {
      await setup({ showContactManager: true, selectedTenants: tenants });
    });

    const modal = screen.getByText('Select a contact');
    expect(modal).toBeVisible();

    const cancel = screen.getByText('Cancel');
    await act(async () => userEvent.click(cancel));

    expect(setShowContactManager).toHaveBeenLastCalledWith(false);
    expect(setSelectedContacts.mock.calls[0][0][0].id).toBe(tenants[0].id);
  });

  it('displays modal when prop is set', async () => {
    await act(async () => {
      await setup({ showContactManager: true });
    });

    const modal = screen.getByText('Select a contact');

    expect(modal).toBeVisible();
  });

  it('displays the number of previously selected tenants', async () => {
    await setup({
      selectedTenants: [new FormTenant(undefined, getMockContactOrganizationWithOnePerson())],
    });

    const number = screen.getByText('1 Tenants associated with this Lease/License');

    expect(number).toBeVisible();
  });

  it('displays previously selected tenants', async () => {
    await setup({
      selectedTenants: [new FormTenant(undefined, getMockContactOrganizationWithOnePerson())],
    });

    const summary = screen.getByText('Dairy Queen Forever! Property Management');

    expect(summary).toBeVisible();
  });

  it('displays Not applicable for contact when contact is a person', async () => {
    await setup({
      selectedTenants: [new FormTenant(undefined, getMockContactPerson())],
    });

    const contactMsg = screen.getByText('Not applicable');

    expect(contactMsg).toBeVisible();
  });

  it('displays no contacts available if organization has no contacts', async () => {
    const organization: IContactSearchResult = {
      ...getMockContactOrganizationWithOnePerson(),
      organization: { ...getEmptyOrganization(), organizationPersons: [] },
      personId: undefined,
      person: undefined,
      surname: undefined,
      firstName: undefined,
      middleNames: undefined,
    } as unknown as IContactSearchResult;

    await setup({
      selectedTenants: [new FormTenant(undefined, organization)],
    });

    const contactMsg = screen.getByText('No contacts available');

    expect(contactMsg).toBeVisible();
  });

  it('displays no contacts available if organization has no contacts', async () => {
    const organization: IContactSearchResult = {
      ...getMockContactOrganizationWithOnePerson(),
      organization: { ...getEmptyOrganization(), organizationPersons: [] },
      personId: undefined,
      person: undefined,
      surname: undefined,
      firstName: undefined,
      middleNames: undefined,
    } as unknown as IContactSearchResult;

    await setup({
      selectedTenants: [new FormTenant(undefined, organization)],
    });

    const contactMsg = screen.getByText('No contacts available');

    expect(contactMsg).toBeVisible();
  });

  it('displays no contacts available if organization has no contacts', async () => {
    const organization: IContactSearchResult = {
      ...getMockContactOrganizationWithOnePerson(),
      organization: { ...getEmptyOrganization(), organizationPersons: [] },
      personId: undefined,
      person: undefined,
      surname: undefined,
      firstName: undefined,
      middleNames: undefined,
    } as unknown as IContactSearchResult;

    await setup({
      selectedTenants: [new FormTenant(undefined, organization)],
    });

    const contactMsg = screen.getByText('No contacts available');

    expect(contactMsg).toBeVisible();
  });

  it('displays the primary contact if there is only one', async () => {
    const organization: IContactSearchResult = {
      ...getMockContactOrganizationWithOnePerson(),
      organization: {
        ...getEmptyOrganization(),
        organizationPersons: [
          {
            id: 1,
            organization: null,
            personId: 3,
            organizationId: 3,
            rowVersion: 1,
            person: { ...getEmptyPerson(), firstName: 'test', surname: 'testerson' },
          },
        ],
      },
      personId: undefined,
      person: undefined,
      surname: undefined,
      firstName: undefined,
      middleNames: undefined,
    } as unknown as IContactSearchResult;

    await setup({
      selectedTenants: [new FormTenant(undefined, organization)],
    });

    const contactPerson = screen.getByText('test testerson');

    expect(contactPerson).toBeVisible();
  });

  it('displays a list if there are multiple', async () => {
    const organization: IContactSearchResult = {
      ...getMockContactOrganizationWithOnePerson(),
      organization: {
        ...getEmptyOrganization(),
        organizationPersons: [
          {
            id: 1,
            organization: null,
            personId: 3,
            organizationId: 3,
            rowVersion: 1,
            person: { ...getEmptyPerson(), id: 1, firstName: 'test', surname: 'testerson' },
          },
          {
            id: 2,
            organization: null,
            personId: 2,
            organizationId: 3,
            rowVersion: 1,
            person: { ...getEmptyPerson(), id: 2, firstName: 'second', surname: 'testerson' },
          },
        ],
      },
      personId: undefined,
      person: undefined,
      surname: undefined,
      firstName: undefined,
      middleNames: undefined,
    } as unknown as IContactSearchResult;

    await setup({
      selectedTenants: [new FormTenant(undefined, organization)],
    });

    const contactMsg = screen.getByDisplayValue('Select a contact');

    expect(contactMsg).toBeVisible();
  });

  it('can remove previously selected tenants', async () => {
    const {
      component: { getByTitle },
    } = await setup({
      selectedTenants: [new FormTenant(undefined, getMockContactOrganizationWithOnePerson())],
    });

    const deleteButton = getByTitle('Click to remove');
    await act(async () => userEvent.click(deleteButton));

    expect(setSelectedTenants).toHaveBeenCalledWith([]);
    expect(setSelectedContacts).toHaveBeenCalledWith([]);
  });
});

import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims } from 'constants/claims';
import { apiLeaseToFormLease } from 'features/leases/leaseUtils';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { getMockLease } from 'mocks/mockLease';
import {
  getAllByRole as getAllByRoleBase,
  mockKeycloak,
  renderAsync,
  RenderOptions,
  within,
} from 'utils/test-utils';

import AddLeaseTenantForm, { IAddLeaseTenantFormProps } from './AddLeaseTenantForm';
import { FormTenant } from './Tenant';

// mock auth library
jest.mock('@react-keycloak/web');

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

describe('AddLeaseTenantForm component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IAddLeaseTenantFormProps> & {
        initialValues?: IFormLease;
        selectedTenants?: FormTenant[];
        onCancel?: () => void;
        setSelectedTenants?: (tenants: FormTenant[]) => void;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <AddLeaseTenantForm
          selectedTenants={renderOptions.selectedTenants ?? []}
          setSelectedTenants={renderOptions.setSelectedTenants ?? noop}
          onCancel={renderOptions.onCancel ?? noop}
          onSubmit={noop as any}
          formikRef={null}
          initialValues={renderOptions.initialValues}
        />
      </Formik>,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      findFirstRow: () => {
        const rows = component.getAllByRole('row');
        return rows && rows.length > 1 ? rows[1] : null;
      },
      findFirstRowTableTwo: () => {
        const rows = within(component.getByTestId('selected-items')).getAllByRole('row');
        return rows && rows.length > 1 ? rows[1] : null;
      },
      findCell: (row: HTMLElement, index: number) => {
        const columns = getAllByRoleBase(row, 'cell');
        return columns && columns.length > index ? columns[index] : null;
      },
      component,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
    mockKeycloak({ claims: [Claims.CONTACT_VIEW] });
  });
  it('renders as expected', async () => {
    mockAxios.onGet().reply(200, []);
    const { component } = await setup({});
    const { findByTitle } = component;
    await findByTitle('table-loading');

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('the cancel button triggers the cancel action', async () => {
    const cancel = jest.fn();
    mockAxios.onGet().reply(200, { items: [] });
    const {
      component: { getByText, findByTitle },
    } = await setup({ onCancel: cancel });
    await findByTitle('table-loading');

    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);
    expect(cancel).toHaveBeenCalled();
  });

  it('items from the contact list view can be selected', async () => {
    const setSelectedTenants = jest.fn();
    mockAxios.onGet().reply(200, {
      items: sampleContactResponse,
    });
    const {
      component: { findByTestId, findByTitle },
    } = await setup({ setSelectedTenants: setSelectedTenants });
    await findByTitle('table-loading');

    const checkBox = await findByTestId('selectrow-O5');
    userEvent.click(checkBox);
    expect(setSelectedTenants).toHaveBeenCalledWith([
      {
        id: 'O5',
        isDisabled: false,
        organizationId: 5,
        organizationName: "Bob's Property Management",
        rowVersion: 0,
        summary: "Bob's Property Management",
      },
    ]);
  });

  it('items from the contact list view can be added', async () => {
    mockAxios.onGet().reply(200, {
      items: sampleContactResponse,
    });
    const {
      component: { getByText, findAllByTitle },
      findFirstRowTableTwo,
      findCell,
    } = await setup({
      initialValues: { tenants: [] } as any,
      selectedTenants: [sampleContactResponse[0] as any],
    });

    const addButton = getByText('Add selected tenants');
    userEvent.click(addButton);

    await findAllByTitle('Click to remove');
    const dataRow = findFirstRowTableTwo() as HTMLElement;
    expect(dataRow).not.toBeNull();
    expect(findCell(dataRow, 3)?.textContent).toBe('Bob Billy Smith');
    expect(findCell(dataRow, 4)?.textContent).toBe('Not applicable');
    const saveButton = getByText('Save');
    expect(saveButton).not.toBeDisabled();
  });

  it('items from the contact list cannot be duplicated', async () => {
    mockAxios.onGet().reply(200, {
      items: sampleContactResponse,
    });
    const {
      component: { getByText, getByTestId, findAllByTitle },
    } = await setup({
      initialValues: { tenants: sampleContactResponse } as any,
      selectedTenants: [new FormTenant(undefined, sampleContactResponse[0])],
    });
    const addButton = getByText('Add selected tenants');
    userEvent.click(addButton);

    await findAllByTitle('Click to remove');
    const dataRows = within(getByTestId('selected-items')).getAllByRole('row');
    expect(dataRows).not.toBeNull();
    expect(dataRows).toHaveLength(4);
  });

  it('items can be removed', async () => {
    mockAxios.onAny().reply(200, {
      items: sampleContactResponse,
    });
    const {
      findFirstRowTableTwo,
      findCell,
      component: { findAllByTitle, getByText },
    } = await setup({
      initialValues: { tenants: sampleContactResponse } as any,
      selectedTenants: [sampleContactResponse[0] as any],
    });

    const addButton = getByText('Add selected tenants');
    userEvent.click(addButton);

    await findAllByTitle('Click to remove');
    let dataRow = findFirstRowTableTwo() as HTMLElement;
    expect(dataRow).not.toBeNull();
    const deleteCell = findCell(dataRow, 0);
    deleteCell && userEvent.click(deleteCell);

    dataRow = findFirstRowTableTwo() as HTMLElement;
    expect(findCell(dataRow, 3)?.textContent).toBe('Bob Billy Smith');
    expect(findCell(dataRow, 4)?.textContent).toBe('Not applicable');
  });
  describe('primary contact behaviour', () => {
    it('previously selected organization with one primary contact displays correctly', async () => {
      mockAxios.onAny().reply(200, {
        items: sampleContactResponse,
      });
      const {
        component: { findByText },
      } = await setup({
        initialValues: apiLeaseToFormLease(getMockLease()),
        selectedTenants: [],
      });

      const primaryContact = await findByText('Stinky Cheese');
      expect(primaryContact).toBeVisible();
    });

    it('previously selected organization with multiple primary contacts displays correctly', async () => {
      mockAxios.onAny().reply(200, {
        items: sampleContactResponse,
      });
      const {
        component: { findByDisplayValue },
      } = await setup({
        initialValues: apiLeaseToFormLease(getMockLease()),
        selectedTenants: [],
      });

      const primaryContact = await findByDisplayValue('Bob Billy Smith');
      expect(primaryContact).toBeVisible();
      expect(primaryContact).toContainHTML(
        '<option value="">Select a contact</option><option value="1" class="option">Bob Billy Smith</option><option value="4" class="option">Minnie Nacho Cheese Mouse</option>',
      );
    });

    it('previously selected organization with no primary contacts displays correctly', async () => {
      mockAxios.onAny().reply(200, {
        items: sampleContactResponse,
      });
      const {
        component: { findByText },
      } = await setup({
        initialValues: apiLeaseToFormLease(getMockLease()),
        selectedTenants: [],
      });

      const primaryContact = await findByText('No contacts available');
      expect(primaryContact).toBeVisible();
    });

    it('previously selected organization with multiple primary contacts does not allow duplicate selection', async () => {
      mockAxios.onAny().reply(200, {
        items: sampleContactResponse,
      });
      const {
        component: { findByDisplayValue, getByText },
      } = await setup({
        initialValues: apiLeaseToFormLease(getMockLease()),
        selectedTenants: [sampleContactResponse[0] as any],
      });

      const addButton = getByText('Add selected tenants');
      userEvent.click(addButton);

      const primaryContact = await findByDisplayValue('Bob Billy Smith');
      expect(primaryContact).toBeVisible();
    });

    it('selecting organization with one primary contact displays correctly', async () => {
      mockAxios.onAny().reply(200, {
        items: [orgWithOnePerson],
      });
      const {
        component: { getByText, findByText },
      } = await setup({
        initialValues: { tenants: [] } as any,
        selectedTenants: [new FormTenant(undefined, orgWithOnePerson)],
      });

      const addButton = getByText('Add selected tenants');
      userEvent.click(addButton);

      const organization = await findByText('Dairy Queen Forever! Property Management');

      expect(organization).toBeVisible();

      const primaryContact = await findByText('Stinky Cheese');
      expect(primaryContact).toBeVisible();
    });

    it('selecting organization with multiple primary contacts displays correctly', async () => {
      mockAxios.onAny().reply(200, {
        items: [orgWithMultiplePeople],
      });
      const {
        component: { findByText, getByText, findByDisplayValue },
      } = await setup({
        initialValues: { tenants: [] } as any,
        selectedTenants: [new FormTenant(undefined, orgWithMultiplePeople)],
      });

      const addButton = getByText('Add selected tenants');
      userEvent.click(addButton);

      const organization = await findByText('French Mouse Property Management');

      expect(organization).toBeVisible();

      const primaryContact = await findByDisplayValue('Select a contact');
      expect(primaryContact).toBeVisible();
      expect(primaryContact).toContainHTML(
        '<option value="">Select a contact</option><option value="1" class="option">Bob Billy Smith</option><option value="4" class="option">Minnie Nacho Cheese Mouse</option>',
      );
    });

    it('selecting organization with no primary contacts displays correctly', async () => {
      mockAxios.onAny().reply(200, {
        items: [orgWithNoPeople],
      });
      const {
        component: { findByText, getByText },
      } = await setup({
        initialValues: { tenants: [] } as any,
        selectedTenants: [new FormTenant(undefined, orgWithNoPeople)],
      });

      const addButton = getByText('Add selected tenants');
      userEvent.click(addButton);

      const organization = await findByText('Pussycat Property Management');

      expect(organization).toBeVisible();

      const primaryContact = await findByText('No contacts available');
      expect(primaryContact).toBeVisible();
      expect(primaryContact).toContainHTML('<p>No contacts available</p>');
    });
  });
});

const sampleContactResponse = [
  {
    id: 'P2',
    personId: 2,
    organizationId: 5,
    rowVersion: 0,
    summary: 'Bob Billy Smith',
    surname: 'Smith',
    firstName: 'Bob',
    isDisabled: false,
  },
  {
    id: 'O5',
    organizationId: 5,
    rowVersion: 0,
    summary: "Bob's Property Management",
    organizationName: "Bob's Property Management",
    isDisabled: false,
  },
];

const orgWithOnePerson = {
  id: 'O3',
  organizationId: 3,
  organization: {
    id: 3,
    isDisabled: false,
    name: 'Dairy Queen Forever! Property Management',
    organizationPersons: [
      {
        person: {
          id: 3,
          isDisabled: false,
          surname: 'Cheese',
          firstName: 'Stinky',
          middleNames: '',
          personOrganizations: [
            {
              personId: 3,
              isDisabled: false,
              rowVersion: 1,
            },
          ],
          personAddresses: [],
          contactMethods: [],
          rowVersion: 1,
        },
        personId: 3,
        organizationId: 3,
        isDisabled: false,
        rowVersion: 1,
      },
    ],
    organizationAddresses: [],
    contactMethods: [],
    rowVersion: 1,
  },
  rowVersion: 0,
  summary: 'Dairy Queen Forever! Property Management',
  organizationName: 'Dairy Queen Forever! Property Management',
  isDisabled: false,
};
const orgWithMultiplePeople = {
  id: 'O2',
  organizationId: 2,
  organization: {
    id: 2,
    isDisabled: false,
    name: 'French Mouse Property Management',
    alias: '',
    incorporationNumber: '',
    organizationPersons: [
      {
        person: {
          id: 1,
          isDisabled: false,
          surname: 'Smith',
          firstName: 'Bob',
          middleNames: 'Billy',
          preferredName: 'Tester McTest',
          personOrganizations: [
            {
              personId: 1,
              isDisabled: false,
              rowVersion: 3,
            },
          ],
          personAddresses: [],
          contactMethods: [],
          comment: 'This is a test comment.',
          rowVersion: 4,
        },
        personId: 1,
        organizationId: 2,
        isDisabled: false,
        rowVersion: 3,
      },
      {
        person: {
          id: 4,
          isDisabled: false,
          surname: 'Mouse',
          firstName: 'Minnie',
          middleNames: 'Nacho Cheese',
          personOrganizations: [
            {
              personId: 4,
              isDisabled: false,
              rowVersion: 1,
            },
          ],
          personAddresses: [],
          contactMethods: [],
          rowVersion: 1,
        },
        personId: 4,
        organizationId: 2,
        isDisabled: false,
        rowVersion: 1,
      },
    ],
    organizationAddresses: [],
    contactMethods: [],
    comment: '',
    rowVersion: 2,
  },
  rowVersion: 0,
  summary: 'French Mouse Property Management',
  organizationName: 'French Mouse Property Management',
  mailingAddress: '1450 Glentana rd.',
  municipalityName: 'Victoria',
  provinceState: 'BC',
  isDisabled: false,
};
const orgWithNoPeople = {
  id: 'O4',
  organizationId: 4,
  organization: {
    id: 4,
    isDisabled: false,
    name: 'Pussycat Property Management',
    organizationPersons: [],
    organizationAddresses: [],
    contactMethods: [],
    rowVersion: 1,
  },
  rowVersion: 0,
  summary: 'Pussycat Property Management',
  organizationName: 'Pussycat Property Management',
  isDisabled: false,
};

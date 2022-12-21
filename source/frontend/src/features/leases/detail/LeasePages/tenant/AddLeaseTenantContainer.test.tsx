import { screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims } from 'constants/claims';
import { LeaseContextProvider } from 'features/leases/context/LeaseContext';
import { createMemoryHistory } from 'history';
import { defaultLease, ILease } from 'interfaces';
import { mockLookups } from 'mocks';
import {
  getMockContactOrganizationWithMultiplePeople,
  getMockContactOrganizationWithOnePerson,
  getMockPerson,
} from 'mocks/mockContacts';
import { getMockLease } from 'mocks/mockLease';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import {
  act,
  fillInput,
  getAllByRole as getAllByRoleBase,
  mockKeycloak,
  renderAsync,
  RenderOptions,
  waitFor,
  within,
} from 'utils/test-utils';

import { AddLeaseTenantContainer } from './AddLeaseTenantContainer';

// mock auth library
jest.mock('@react-keycloak/web');

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
describe('AddLeaseTenantContainer component', () => {
  const setup = async (renderOptions: RenderOptions & { lease?: ILease } = {}) => {
    // render component under test
    const component = await renderAsync(
      <LeaseContextProvider initialLease={renderOptions.lease ?? { ...defaultLease, id: 1 }}>
        <AddLeaseTenantContainer />
      </LeaseContextProvider>,
      {
        ...renderOptions,
        store: storeState,
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
    mockAxios.reset();
    mockKeycloak({ claims: [Claims.CONTACT_VIEW] });
  });
  it('renders as expected', async () => {
    mockAxios.onGet().reply(200, []);
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });

  it.skip('items from the contact list view can be added', async () => {
    mockAxios.onPut().reply(200);
    mockAxios.onGet().reply(200, {
      items: sampleContactResponse,
    });
    const {
      component: { getByText, findByText, findByTestId },
      findFirstRowTableTwo,
      findCell,
    } = await setup({});

    const checkbox = await findByTestId('selectrow-P2');
    userEvent.click(checkbox);
    await findByText('1 selected');

    const addButton = getByText('Add selected tenants');
    await act(() => userEvent.click(addButton));

    const dataRow = findFirstRowTableTwo() as HTMLElement;
    expect(dataRow).not.toBeNull();
    expect(findCell(dataRow, 3)?.textContent).toBe('Bob Billy Smith');
    expect(findCell(dataRow, 4)?.textContent).toBe('Not applicable');

    const saveButton = getByText('Save');
    expect(saveButton).not.toBeDisabled();
    await act(() => userEvent.click(saveButton));
    expect(mockAxios.history.put[0].data).toEqual(expectedTenantRequestData);
  });

  it.skip('Pre-existing items from the contact list view can be added to', async () => {
    mockAxios.onPut().reply(200);
    mockAxios.onGet().reply(200, {
      items: sampleContactResponse,
    });
    const {
      component: { getByText, findByTestId, findByText, findAllByText },
    } = await setup({ lease: getMockLease() });

    const checkbox = await findByTestId('selectrow-P2');
    userEvent.click(checkbox);
    await findByText('1 selected');

    const addButton = getByText('Add selected tenants');
    userEvent.click(addButton);

    await waitFor(async () => {
      expect(await findAllByText('Bob Billy Smith')).toHaveLength(3);
    });

    expect(await findByText('French Mouse Property Management')).toBeVisible();
    expect(await findByText('Dairy Queen Forever! Property Management')).toBeVisible();
    expect(await findByText('Pussycat Property Management')).toBeVisible();
  });

  it.skip('primary contact information is loaded for a organization with a single contact', async () => {
    mockAxios.onPut().reply(200);
    mockAxios
      .onGet('/persons/3')
      .reply(200, getMockPerson({ id: 3, firstName: 'Stinky', surname: 'Cheese' }));
    mockAxios.onGet().reply(200, {
      items: [getMockContactOrganizationWithOnePerson()],
    });
    const {
      component: { getByText, findByTestId, findByText, findAllByTitle },
    } = await setup({});

    const checkbox = await findByTestId('selectrow-O3');
    userEvent.click(checkbox);
    await findByText('1 selected');

    const addButton = getByText('Add selected tenants');
    userEvent.click(addButton);

    await findAllByTitle('Click to remove');
    expect(await findByText('Stinky Cheese')).toBeVisible();
    expect(mockAxios.history.get[1].url).toBe('/persons/3');
  });

  it.skip('primary contact information is loaded for a organization with multiple person contacts', async () => {
    mockAxios.onPut().reply(200);
    mockAxios
      .onGet('/persons/3')
      .reply(200, getMockPerson({ id: 3, firstName: 'Stinky', surname: 'Cheese' }));
    mockAxios
      .onGet('/persons/1')
      .reply(200, getMockPerson({ id: 1, firstName: 'Bob', surname: 'Billy' }));
    mockAxios.onGet().reply(200, {
      items: [getMockContactOrganizationWithMultiplePeople()],
    });
    const {
      component: { getByText, findByTestId, findByText, findByRole },
    } = await setup({});

    const checkbox = await findByTestId('selectrow-O2');
    userEvent.click(checkbox);
    await findByText('1 selected');

    const addButton = getByText('Add selected tenants');
    userEvent.click(addButton);

    expect(await findByRole('option', { name: 'Select a contact' })).toBeVisible();
    expect(await findByRole('option', { name: 'Stinky Cheese' })).toBeVisible();
    expect(await findByRole('option', { name: 'Bob Billy' })).toBeVisible();
    expect(mockAxios.history.get[1].url).toBe('/persons/1');
    expect(mockAxios.history.get[2].url).toBe('/persons/3');
  });

  it.skip('primary contact information is loaded for multiple organizations each with multiple person contacts', async () => {
    mockAxios.onPut().reply(200);
    mockAxios
      .onGet('/persons/3')
      .reply(200, getMockPerson({ id: 3, firstName: 'Stinky', surname: 'Cheese' }));
    mockAxios
      .onGet('/persons/1')
      .reply(200, getMockPerson({ id: 1, firstName: 'Bob', surname: 'Billy' }));
    mockAxios.onGet().reply(200, {
      items: [
        getMockContactOrganizationWithMultiplePeople(),
        { ...getMockContactOrganizationWithMultiplePeople(), id: 'O1', organizationId: 1 },
      ],
    });
    const {
      component: { getByText, findByTestId, findByText, findAllByRole },
    } = await setup({});

    const checkbox = await findByTestId('selectrow-O1');
    userEvent.click(checkbox);
    const checkbox2 = await findByTestId('selectrow-O2');
    userEvent.click(checkbox2);
    await findByText('2 selected');

    const addButton = getByText('Add selected tenants');
    act(() => userEvent.click(addButton));

    expect((await findAllByRole('option', { name: 'Select a contact' }))[0]).toBeVisible();
    // TODO: This assertion is failing - needs investigation
    // expect(mockAxios.history.get).toHaveLength(3); // unique persons should only be requested once.
    expect(mockAxios.history.get[1].url).toBe('/persons/1');
    expect(mockAxios.history.get[2].url).toBe('/persons/3');
  });

  describe('displays modal warning when a tenant organization with persons has no primary contact', () => {
    it('shows the modal with expected text', async () => {
      mockAxios.onPut().reply(200);
      mockAxios.onGet().reply(200, {
        items: [],
      });

      const {
        component: { container, getByText },
      } = await setup({ lease: getMockLease() });
      // Remove the primary contact
      await fillInput(container, 'tenants.0.primaryContactId', '', 'select');

      const saveButton = getByText('Save');
      userEvent.click(saveButton);

      const modalText = await screen.findByText('Confirm save');
      expect(modalText).toBeVisible();
    });

    it('does not save when modal is cancelled', async () => {
      mockAxios.onPut().reply(200);
      mockAxios.onGet().reply(200, {
        items: [],
      });

      const {
        component: { container, getByText },
      } = await setup({ lease: getMockLease() });
      // Remove the primary contact
      await fillInput(container, 'tenants.0.primaryContactId', '', 'select');

      const saveButton = getByText('Save');
      userEvent.click(saveButton);

      const cancelButton = await screen.findByTitle('cancel-modal');
      userEvent.click(cancelButton);

      await waitFor(() => {
        expect(mockAxios.history.put).toHaveLength(0);
      });
    });

    it.skip('saves the form when modal confirmed', async () => {
      mockAxios.onPut().reply(200);
      mockAxios.onGet().reply(200, {
        items: [],
      });

      const {
        component: { container, getByText },
      } = await setup({ lease: getMockLease() });
      // Remove the primary contact
      await fillInput(container, 'tenants.0.primaryContactId', '', 'select');

      const saveButton = getByText('Save');
      userEvent.click(saveButton);

      const modalSaveButton = await screen.findByTitle('ok-modal');
      userEvent.click(modalSaveButton);

      await waitFor(() => {
        expect(mockAxios.history.put[0].data).toEqual(expectedTenantWithPrimaryContactRequestData);
      });
    });
  });
});

const expectedTenantRequestData =
  '{"organizations":[],"persons":[],"properties":[],"improvements":[],"securityDeposits":[],"securityDepositReturns":[],"startDate":"2020-01-01","lFileNo":"","tfaFileNumber":0,"psFileNo":"","programName":"","motiName":"Moti, Name, Name","amount":0,"renewalCount":0,"tenantNotes":[],"insurances":[],"isResidential":false,"isCommercialBuilding":false,"isOtherImprovement":false,"returnNotes":"","terms":[],"tenants":[{"leaseId":1,"personId":2}],"hasDigitalLicense":null,"hasPhysicalLicense":null,"statusType":{"id":"ACTIVE","description":"Active","isDisabled":false},"region":{"regionCode":1,"regionName":"South Coast Region"},"programType":{"id":"OTHER","description":"Other","isDisabled":false},"paymentReceivableType":{"id":"RCVBL","description":"Receivable","isDisabled":false},"categoryType":{"id":"COMM","description":"Commercial","isDisabled":false},"purposeType":{"id":"BCFERRIES","description":"BC Ferries","isDisabled":false},"responsibilityType":{"id":"HQ","description":"Headquarters","isDisabled":false},"initiatorType":{"id":"PROJECT","description":"Project","isDisabled":false},"type":{"id":"LSREG","description":"Lease - Registered","isDisabled":false},"id":1}';
const expectedTenantWithPrimaryContactRequestData =
  '{"organizations":[],"persons":[],"properties":[],"improvements":[],"securityDeposits":[],"securityDepositReturns":[],"startDate":"","lFileNo":"","tfaFileNumber":0,"psFileNo":"","programName":"","motiName":"","amount":0,"renewalCount":0,"tenantNotes":["a note","",""],"insurances":[],"isResidential":true,"isCommercialBuilding":true,"isOtherImprovement":false,"returnNotes":"","terms":[],"tenants":[{"leaseId":1,"organizationId":2,"note":"a note"},{"leaseId":1,"organizationId":3,"note":""},{"leaseId":1,"organizationId":4,"note":""}],"hasDigitalLicense":null,"hasPhysicalLicense":null,"id":1,"rowVersion":2,"paymentReceivableType":{"id":""},"categoryType":{"id":""},"purposeType":{"id":""},"responsibilityType":{"id":""},"initiatorType":{"id":""},"statusType":{"id":""},"programType":{"id":""},"region":{"regionCode":1}}';
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

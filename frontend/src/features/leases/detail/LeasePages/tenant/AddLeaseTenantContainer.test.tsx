import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims } from 'constants/claims';
import { LeaseContextProvider } from 'features/leases/context/LeaseContext';
import { createMemoryHistory } from 'history';
import { defaultLease, ILease } from 'interfaces';
import {
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

describe('AddLeaseTenantForm component', () => {
  const setup = async (renderOptions: RenderOptions & { lease?: ILease } = {}) => {
    // render component under test
    const component = await renderAsync(
      <LeaseContextProvider initialLease={renderOptions.lease ?? { ...defaultLease, id: 1 }}>
        <AddLeaseTenantContainer />
      </LeaseContextProvider>,
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

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('items from the contact list view can be added', async () => {
    mockAxios.onPut().reply(200);
    mockAxios.onGet().reply(200, {
      items: sampleContactResponse,
    });
    const {
      component: { getByText, findAllByTitle, findByTestId },
      findFirstRowTableTwo,
      findCell,
    } = await setup({});

    const checkbox = await findByTestId('selectrow-P2');
    userEvent.click(checkbox);

    const addButton = getByText('Add selected tenants');
    userEvent.click(addButton);

    await findAllByTitle('Click to remove');
    const dataRow = findFirstRowTableTwo() as HTMLElement;
    expect(dataRow).not.toBeNull();
    expect(findCell(dataRow, 3)?.textContent).toBe('Bob Billy Smith');
    expect(findCell(dataRow, 4)?.textContent).toBe('Smith');
    expect(findCell(dataRow, 5)?.textContent).toBe('Bob');

    const saveButton = getByText('Save');
    expect(saveButton).not.toBeDisabled();
    userEvent.click(saveButton);
    await waitFor(() => {
      expect(mockAxios.history.put[0].data).toEqual(expectedTenantRequestData);
    });
  });
});

const expectedTenantRequestData =
  '{"organizations":[],"persons":[],"properties":[],"improvements":[],"securityDeposits":[],"securityDepositReturns":[],"startDate":"2020-01-01","lFileNo":"","tfaFileNo":0,"psFileNo":"","programName":"","motiName":"Moti, Name, Name","amount":0,"renewalCount":0,"tenantNotes":[],"insurances":[],"isResidential":false,"isCommercialBuilding":false,"isOtherImprovement":false,"returnNotes":"","terms":[],"tenants":[{"id":"P2","personId":2,"rowVersion":0,"summary":"Bob Billy Smith","surname":"Smith","firstName":"Bob","isDisabled":false,"leaseId":1}],"statusType":{"id":"ACTIVE","description":"Active","isDisabled":false},"region":{"regionCode":1,"regionName":"South Coast Region"},"programType":{"id":"OTHER","description":"Other","isDisabled":false},"paymentReceivableType":{"id":"RCVBL","description":"Receivable","isDisabled":false},"categoryType":{"id":"COMM","description":"Commercial","isDisabled":false},"purposeType":{"id":"BCFERRIES","description":"BC Ferries","isDisabled":false},"responsibilityType":{"id":"HQ","description":"Headquarters","isDisabled":false},"initiatorType":{"id":"PROJECT","description":"Project","isDisabled":false},"type":{"id":"LSREG","description":"Lease - Registered","isDisabled":false},"id":1}';

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

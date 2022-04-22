import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions, waitFor } from 'utils/test-utils';

import AddLeaseContainer, { IAddLeaseContainerProps } from './AddLeaseContainer';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockAxios = new MockAdapter(axios);

describe('AddLeaseContainer component', () => {
  const setup = async (renderOptions: RenderOptions & Partial<IAddLeaseContainerProps> = {}) => {
    // render component under test
    const component = await renderAsync(<AddLeaseContainer />, {
      ...renderOptions,
      store: storeState,
      history,
    });

    return {
      component,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
  });
  it('renders as expected', async () => {
    const { component } = await setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('cancels the form', async () => {
    const {
      component: { getAllByText },
    } = await setup({});
    userEvent.click(getAllByText('Cancel')[0]);
    expect(history.location.pathname).toBe('/lease/list');
  });

  it('saves the form with minimal data', async () => {
    const {
      component: { getByText, container },
    } = await setup({});
    await fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    await fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
    await fillInput(container, 'paymentReceivableType', 'RCVBL', 'select');
    await fillInput(container, 'region', '1', 'select');
    await fillInput(container, 'programType', 'BCFERRIES', 'select');
    await fillInput(container, 'type', 'LICONSTRC', 'select');
    await fillInput(container, 'purposeType', 'BCFERRIES', 'select');
    userEvent.click(getByText('Remove'));

    mockAxios.onPost().reply(200, {});
    userEvent.click(getByText('Save'));
    await waitFor(() => {
      expect(mockAxios.history.post[0].data).toEqual(expectedFormData);
    });
  });

  it('triggers the confirm popup', async () => {
    const {
      component: { getByText, findByText, container },
    } = await setup({});
    await fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    await fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
    await fillInput(container, 'paymentReceivableType', 'RCVBL', 'select');
    await fillInput(container, 'region', '1', 'select');
    await fillInput(container, 'programType', 'BCFERRIES', 'select');
    await fillInput(container, 'type', 'LICONSTRC', 'select');
    await fillInput(container, 'purposeType', 'BCFERRIES', 'select');
    userEvent.click(getByText('Remove'));

    mockAxios.onPost().reply(409, { error: 'test message' });
    userEvent.click(getByText('Save'));
    expect(await findByText('test message')).toBeVisible();
  });

  it('clicking on the save anyways popup saves the form', async () => {
    const {
      component: { getByText, findByText, container },
    } = await setup({});
    await fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    await fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
    await fillInput(container, 'paymentReceivableType', 'RCVBL', 'select');
    await fillInput(container, 'region', '1', 'select');
    await fillInput(container, 'programType', 'BCFERRIES', 'select');
    await fillInput(container, 'type', 'LICONSTRC', 'select');
    await fillInput(container, 'purposeType', 'BCFERRIES', 'select');
    userEvent.click(getByText('Remove'));

    mockAxios.onPost().reply(409, { error: 'test message' });
    userEvent.click(getByText('Save'));
    userEvent.click(await findByText('Save Anyways'));
    await waitFor(() => {
      expect(mockAxios.history.post[1].data).toEqual(expectedFormData);
    });
  });
});

const expectedFormData =
  '{"lFileNo":"","tfaFileNo":0,"expiryDate":"2020-01-02","startDate":"2020-01-01","paymentReceivableType":{"id":"RCVBL"},"purposeType":{"id":"BCFERRIES"},"initiatorType":{"id":"HQ"},"type":{"id":"LICONSTRC"},"statusType":{"id":"DRAFT"},"region":{"regionCode":"1"},"programType":{"id":"BCFERRIES"},"otherType":"","otherProgramType":"","otherCategoryType":"","otherPurposeType":"","note":"","motiName":"","amount":0,"renewalCount":0,"description":"","isResidential":false,"isCommercialBuilding":false,"isOtherImprovement":false,"returnNotes":"","documentationReference":"","tenantNotes":[],"insurances":[],"terms":[],"tenants":[],"properties":[],"persons":[],"organizations":[],"improvements":[],"securityDeposits":[],"securityDepositReturns":[]}';

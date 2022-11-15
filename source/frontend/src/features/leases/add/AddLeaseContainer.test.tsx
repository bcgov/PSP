import { useKeycloak } from '@react-keycloak/web';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, fillInput, renderAsync, RenderOptions } from 'utils/test-utils';

import AddLeaseContainer, { IAddLeaseContainerProps } from './AddLeaseContainer';

jest.mock('@react-keycloak/web');
(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    subject: 'test',
    authenticated: true,
    userInfo: {
      roles: [],
    },
  },
});

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
    mockAxios.resetHandlers();
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
    await fillInput(container, 'paymentReceivableType.id', 'RCVBL', 'select');
    await fillInput(container, 'region.id', '1', 'select');
    await fillInput(container, 'programType.id', 'BCFERRIES', 'select');
    await fillInput(container, 'type.id', 'LICONSTRC', 'select');
    await fillInput(container, 'purposeType.id', 'BCFERRIES', 'select');
    await fillInput(container, 'statusType.id', 'DRAFT', 'select');
    await act(async () => {
      userEvent.click(getByText('Remove'));
    });

    mockAxios.onPost().reply(200, {});

    await act(async () => {
      userEvent.click(getByText('Save'));
    });

    expect(mockAxios.history.post[0].data).toEqual(expectedFormData);
  });

  it('triggers the confirm popup', async () => {
    const {
      component: { getByText, findByText, container },
    } = await setup({});
    await fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    await fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
    await fillInput(container, 'paymentReceivableType.id', 'RCVBL', 'select');
    await fillInput(container, 'region.id', '1', 'select');
    await fillInput(container, 'programType.id', 'BCFERRIES', 'select');
    await fillInput(container, 'type.id', 'LICONSTRC', 'select');
    await fillInput(container, 'purposeType.id', 'BCFERRIES', 'select');
    await fillInput(container, 'statusType.id', 'DRAFT', 'select');
    await act(async () => {
      userEvent.click(getByText('Remove'));
    });

    mockAxios.onPost().reply(409, { error: 'test message' });

    await act(async () => {
      userEvent.click(getByText('Save'));
    });

    expect(await findByText('test message')).toBeVisible();
  });

  it('clicking on the save anyways popup saves the form', async () => {
    const {
      component: { getByText, findByText, container },
    } = await setup({});
    await fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    await fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
    await fillInput(container, 'paymentReceivableType.id', 'RCVBL', 'select');
    await fillInput(container, 'region.id', '1', 'select');
    await fillInput(container, 'programType.id', 'BCFERRIES', 'select');
    await fillInput(container, 'type.id', 'LICONSTRC', 'select');
    await fillInput(container, 'purposeType.id', 'BCFERRIES', 'select');
    await fillInput(container, 'statusType.id', 'DRAFT', 'select');
    await act(async () => {
      userEvent.click(getByText('Remove'));
    });

    mockAxios.onPost().reply(409, { error: 'test message' });

    await act(async () => {
      userEvent.click(getByText('Save'));
    });

    await act(async () => {
      userEvent.click(await findByText('Save Anyways'));
    });

    expect(mockAxios.history.post[1].data).toEqual(expectedFormData);
  });
});

const expectedFormData =
  '{"expiryDate":"2020-01-02","startDate":"2020-01-01","amount":0,"paymentReceivableType":{"id":"RCVBL"},"purposeType":{"id":"BCFERRIES"},"initiatorType":{"id":"HQ"},"statusType":{"id":"DRAFT"},"type":{"id":"LICONSTRC"},"region":{"id":"1"},"programType":{"id":"BCFERRIES"},"returnNotes":"","motiName":"","properties":[],"isResidential":false,"isCommercialBuilding":false,"isOtherImprovement":false,"otherCategoryType":"","otherProgramType":"","otherPurposeType":"","otherType":""}';

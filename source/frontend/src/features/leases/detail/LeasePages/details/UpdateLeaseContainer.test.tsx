import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { IAddLeaseContainerProps } from 'features/leases';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { createMemoryHistory } from 'history';
import { defaultLease } from 'interfaces';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import { defaultApiLease } from 'models/api/Lease';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { renderAsync, RenderOptions } from 'utils/test-utils';

import { UpdateLeaseContainer } from './UpdateLeaseContainer';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockAxios = new MockAdapter(axios);

describe('Update lease container component', () => {
  const setup = async (renderOptions: RenderOptions & Partial<IAddLeaseContainerProps> = {}) => {
    // render component under test
    const component = await renderAsync(
      <LeaseStateContext.Provider value={{ lease: { ...defaultLease, id: 1 }, setLease: noop }}>
        <UpdateLeaseContainer />
      </LeaseStateContext.Provider>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      component,
    };
  };

  beforeEach(() => {
    mockAxios.onGet().reply(200, { id: 1, ...defaultApiLease });
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
    expect(history.location.pathname).toBe('/lease/undefined');
  });

  // TODOL: Disabled until Lease update refactor is completed
  /*it('saves the form with minimal data', async () => {
    const {
      component: { getByText, findByDisplayValue, container },
    } = await setup({});

    await findByDisplayValue('BC Ferries');
    await fillInput(container, 'purposeType.id', 'COMMBLDG', 'select');

    mockAxios.onPut().reply(200, {});
    await act(() => userEvent.click(getByText('Save')));

    expect(mockAxios.history.put[0].data).toEqual(expectedFormData);
  });

  it('triggers the confirm popup', async () => {
    const {
      component: { getByText, findByText, findByDisplayValue, container },
    } = await setup({});

    await findByDisplayValue('BC Ferries');
    await fillInput(container, 'purposeType.id', 'COMMBLDG', 'select');

    mockAxios.onPut().reply(409, { error: 'test message' });
    await act(() => userEvent.click(getByText('Save')));
    expect(await findByText('test message')).toBeVisible();
  });*/

  /*it('clicking on the save anyways popup saves the form', async () => {
    const {
      component: { getByText, findByText, findByDisplayValue, container },
    } = await setup({});

    await findByDisplayValue('BC Ferries');
    await fillInput(container, 'purposeType.id', 'COMMBLDG', 'select');

    mockAxios.onPut().reply(409, { error: 'test message' });
    await act(() => userEvent.click(getByText('Save')));
    await act(async () => userEvent.click(await findByText('Save Anyways')));

    expect(mockAxios.history.put[1].data).toEqual(expectedFormData);
  });*/
});

/*const expectedFormData =
  '{"id":1,"startDate":"2020-01-01","amount":0,"paymentReceivableType":{"id":"RCVBL","description":"Receivable","isDisabled":false},"categoryType":{"id":"COMM","description":"Commercial","isDisabled":false},"purposeType":{"id":"COMMBLDG","description":"BC Ferries","isDisabled":false},"responsibilityType":{"id":"HQ","description":"Headquarters","isDisabled":false},"initiatorType":{"id":"PROJECT","description":"Project","isDisabled":false},"statusType":{"id":"ACTIVE","description":"Active","isDisabled":false},"type":{"id":"LSREG","description":"Lease - Registered","isDisabled":false},"region":{"id":1,"description":"South Coast Region"},"programType":{"id":"OTHER","description":"Other","isDisabled":false},"returnNotes":"","motiName":"Moti, Name, Name","properties":[],"isResidential":false,"isCommercialBuilding":false,"isOtherImprovement":false,"otherCategoryType":"","otherPurposeType":"","otherType":""}';
  */

import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, renderAsync, RenderOptions, waitFor } from '@/utils/test-utils';

import ExportLeasePaymentsContainer, {
  IExportLeasePaymentsContainer,
} from './ExportLeasePaymentsContainer';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('PaymentsContainer component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IExportLeasePaymentsContainer> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(<ExportLeasePaymentsContainer />, {
      ...renderOptions,
      history,
      store: storeState,
    });

    return {
      component,
    };
  };
  beforeEach(() => {
    Date.now = vi.fn().mockReturnValue(new Date('2020-10-15T18:33:37.000Z'));
    mockAxios.resetHistory();
  });
  afterAll(() => {
    vi.restoreAllMocks();
  });
  it('renders as expected', async () => {
    const { component } = await setup();

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders with the current fiscal year selected by default', async () => {
    const {
      component: { getByDisplayValue },
    } = await setup();

    //this is based on the mocked value
    expect(getByDisplayValue('2020-21')).toBeVisible();
  });

  it('makes a get request for a report', async () => {
    const {
      component: { getByTitle },
    } = await setup();
    mockAxios.onGet().reply(200, {});

    await act(async () => userEvent.click(getByTitle('Export Aggregated Report')));

    await waitFor(() => {
      expect(mockAxios.history.get[0].url).toEqual(`/reports/leases/payments?fiscalYearStart=2020`);
    });
  });

  it('displays an error when request fails', async () => {
    const {
      component: { getByTitle, findByText },
    } = await setup();
    mockAxios.onGet().reply(400, {});

    await act(async () => userEvent.click(getByTitle('Export Aggregated Report')));
    const errorText = await findByText(
      'Failed to export report. If this error persists, please contact your System Administrator.',
    );
    expect(errorText).toBeVisible();
  });
});

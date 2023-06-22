import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { renderAsync, RenderOptions, waitFor } from '@/utils/test-utils';

import ExportAggregatedLeasesContainer, {
  IExportAggregatedLeasesContainer,
} from './ExportAggregatedLeasesContainer';

jest.mock('@react-keycloak/web');

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('PaymentsContainer component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IExportAggregatedLeasesContainer> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(<ExportAggregatedLeasesContainer />, {
      ...renderOptions,
      history,
      store: storeState,
    });

    return {
      component,
    };
  };
  beforeEach(() => {
    Date.now = jest.fn().mockReturnValue(new Date('2020-10-15T18:33:37.000Z'));
    mockAxios.resetHistory();
  });
  afterAll(() => {
    jest.restoreAllMocks();
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

  it('makes a get request for an aggregated report', async () => {
    const {
      component: { getByTitle },
    } = await setup();
    mockAxios.onGet().reply(200, {});

    userEvent.click(getByTitle('Export Aggregated Report'));

    await waitFor(() => {
      expect(mockAxios.history.get[0].url).toEqual(
        `/reports/leases/aggregated?fiscalYearStart=2020`,
      );
    });
  });

  it('displays an error when request fails', async () => {
    const {
      component: { getByTitle, findByText },
    } = await setup();
    mockAxios.onGet().reply(400, {});

    userEvent.click(getByTitle('Export Aggregated Report'));
    const errorText = await findByText(
      'Failed to export report. If this error persists, please contact your System Administrator.',
    );
    expect(errorText).toBeVisible();
  });
});

import { AxiosResponse } from 'axios';
import fileDownload from 'js-file-download';
import { MockedFunction } from 'vitest';

import { useApiManagementActivities } from '@/hooks/pims-api/useApiManagementActivities';
import { useModalContext } from '@/hooks/useModalContext';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockManagementActivity } from '@/mocks/managementActivity.mock';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { Api_ManagementActivityFilter } from '@/models/api/ManagementActivityFilter';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  cleanup,
  getByName,
  render,
  screen,
  userEvent,
  waitForElementToBeRemoved,
} from '@/utils/test-utils';

import { useManagementActivityExport } from '../../hooks/useManagementActivityExport';
import { ManagementActivityFilterModel } from '../models/ManagementActivityFilterModel';
import ManagementActivitiesListView from './ManagementActivitiesListView';

vi.mock('@/hooks/useModalContext');
vi.mock('@/hooks/pims-api/useApiManagementActivities');
vi.mock('../../hooks/useManagementActivityExport');
vi.mock('js-file-download');

const setModalContent = vi.fn();
const setDisplayModal = vi.fn();

vi.mocked(useModalContext, { partial: true }).mockReturnValue({
  setModalContent,
  setDisplayModal,
});

const getManagementActivitiesPagedApiFn = vi.fn();
vi.mocked(useApiManagementActivities, { partial: true }).mockReturnValue({
  getManagementActivitiesPagedApi: getManagementActivitiesPagedApiFn,
});

type ExecuteFn = (
  filter: Partial<Api_ManagementActivityFilter>,
) => Promise<AxiosResponse<Blob, any>>;

const overviewExecuteFn = vi.fn() as MockedFunction<ExecuteFn>;
const invoiceExecuteFn = vi.fn() as MockedFunction<ExecuteFn>;

vi.mocked(useManagementActivityExport, { partial: true }).mockReturnValue({
  generateManagementActivitiesOverviewReport: {
    execute: overviewExecuteFn,
    status: 0,
    response: undefined,
    error: undefined,
    loading: false,
  },
  generateManagementActivitiesInvoiceReport: {
    execute: invoiceExecuteFn,
    status: 0,
    response: undefined,
    error: undefined,
    loading: false,
  },
});

const mockPagedResults = (
  searchResults?: ApiGen_Concepts_ManagementActivity[],
): Partial<AxiosResponse<ApiGen_Base_Page<ApiGen_Concepts_ManagementActivity>, any>> => {
  const results = searchResults ?? [];
  const len = results.length;
  return {
    data: {
      items: results,
      quantity: len,
      total: len,
      page: 1,
    },
  };
};

describe('ManagementActivitiesListView', () => {
  const setup = async () => {
    const rendered = render(<ManagementActivitiesListView />, {
      store: { [lookupCodesSlice.name]: { lookupCodes: mockLookups } },
      useMockAuthentication: true,
    });

    // wait for table to finish loading
    await waitForElementToBeRemoved(() => screen.getByTitle('table-loading'));
    return { ...rendered };
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  afterEach(() => {
    cleanup();
  });

  it('matches snapshot', async () => {
    const results = mockPagedResults([getMockManagementActivity()]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);
    const { asFragment } = await setup();

    expect(asFragment()).toMatchSnapshot();
  });

  it('displays search results', async () => {
    const results = mockPagedResults([
      { ...getMockManagementActivity(1), description: 'Test Activity' },
    ]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);
    await setup();

    expect(await screen.findByText(/test activity/i)).toBeInTheDocument();
  });

  it('displays error toast when api fails', async () => {
    getManagementActivitiesPagedApiFn.mockRejectedValue(new Error('network error'));
    await setup();

    const toast = await screen.findByText('network error');
    expect(toast).toBeVisible();
  });

  it('calls overview export when button clicked', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);
    await setup();

    const button = screen.getByTestId('excel-icon-overview');
    await act(async () => userEvent.click(button));

    expect(overviewExecuteFn).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ManagementActivityFilterModel>>({}),
    );
  });

  it('calls invoice export when button clicked', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);
    await setup();

    const button = screen.getByTestId('excel-icon-invoices');
    await act(async () => userEvent.click(button));

    expect(invoiceExecuteFn).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ManagementActivityFilterModel>>({}),
    );
  });

  it('downloads overview report when response is 200', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);
    overviewExecuteFn.mockResolvedValue({
      status: 200,
      data: new Blob(['test']),
      statusText: 'OK',
      headers: {},
      config: null,
    });

    await setup();

    const button = screen.getByTestId('excel-icon-overview');
    await act(async () => userEvent.click(button));

    expect(fileDownload).toHaveBeenCalledWith(
      expect.any(Blob),
      'Management_Activities_Overview_Report.xlsx',
    );
  });

  it('downloads invoice report when response is 200', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);
    invoiceExecuteFn.mockResolvedValue({
      status: 200,
      data: new Blob(['invoice']),
      statusText: 'OK',
      headers: {},
      config: null,
    });

    await setup();

    const button = screen.getByTestId('excel-icon-invoices');
    await act(async () => userEvent.click(button));

    expect(fileDownload).toHaveBeenCalledWith(
      expect.any(Blob),
      'Management_Activities_Invoice_Report.xlsx',
    );
  });

  it('shows modal when report has no data (204) - overview report', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);
    overviewExecuteFn.mockResolvedValue({
      status: 204,
      data: undefined,
      statusText: 'OK',
      headers: {},
      config: null,
    });

    await setup();

    const button = screen.getByTestId('excel-icon-overview');
    await act(async () => userEvent.click(button));

    expect(setModalContent).toHaveBeenCalledWith(
      expect.objectContaining({ message: expect.stringMatching(/no data/i) }),
    );
    expect(setDisplayModal).toHaveBeenCalledWith(true);
  });

  it('shows modal when report has no data (204) - invoices report', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);
    invoiceExecuteFn.mockResolvedValue({
      status: 204,
      data: undefined,
      statusText: 'OK',
      headers: {},
      config: null,
    });

    await setup();

    const button = screen.getByTestId('excel-icon-invoices');
    await act(async () => userEvent.click(button));

    expect(setModalContent).toHaveBeenCalledWith(
      expect.objectContaining({ message: expect.stringMatching(/no data/i) }),
    );
    expect(setDisplayModal).toHaveBeenCalledWith(true);
  });

  it('searches by file name or reference', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);

    await setup();

    const input = screen.getByPlaceholderText(/Management file number or name/i);
    await act(async () => userEvent.type(input, 'Activity File 123'));

    const searchButton = screen.getByTestId('search');
    await act(async () => userEvent.click(searchButton));

    expect(getManagementActivitiesPagedApiFn).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ManagementActivityFilterModel>>({
        fileNameOrNumberOrReference: 'Activity File 123',
      }),
    );
  });

  it('searches by project name', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);

    await setup();

    const input = screen.getByPlaceholderText(/Enter a project name/i);
    await act(async () => userEvent.type(input, 'Project X'));

    const searchButton = screen.getByTestId('search');
    await act(async () => userEvent.click(searchButton));

    expect(getManagementActivitiesPagedApiFn).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ManagementActivityFilterModel>>({
        projectNameOrNumber: 'Project X',
      }),
    );
  });

  it('searches by PID', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);

    await setup();

    const searchBy = getByName('searchBy');
    await act(async () => userEvent.selectOptions(searchBy, 'pid'));

    const input = screen.getByPlaceholderText(/Enter a PID/i);
    await act(async () => userEvent.type(input, '123456'));

    const searchButton = screen.getByTestId('search');
    await act(async () => userEvent.click(searchButton));

    expect(getManagementActivitiesPagedApiFn).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ManagementActivityFilterModel>>({
        pid: '123456',
      }),
    );
  });

  it('searches by PIN', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);

    await setup();

    const searchBy = getByName('searchBy');
    await act(async () => userEvent.selectOptions(searchBy, 'pin'));

    const input = screen.getByPlaceholderText(/Enter a PIN/i);
    await act(async () => userEvent.type(input, '7890'));

    const searchButton = screen.getByTestId('search');
    await act(async () => userEvent.click(searchButton));

    expect(getManagementActivitiesPagedApiFn).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ManagementActivityFilterModel>>({
        pin: '7890',
      }),
    );
  });

  it('resets filter when reset button clicked', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);

    await setup();

    const input = screen.getByPlaceholderText(/Enter a project name/i);
    await act(async () => userEvent.type(input, 'Project Y'));

    const resetButton = screen.getByTitle(/reset-button/i);
    await act(async () => userEvent.click(resetButton));

    expect(getManagementActivitiesPagedApiFn).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ManagementActivityFilterModel>>({
        projectNameOrNumber: '',
        fileNameOrNumberOrReference: '',
        pid: '',
        pin: '',
        address: '',
      }),
    );
  });
});

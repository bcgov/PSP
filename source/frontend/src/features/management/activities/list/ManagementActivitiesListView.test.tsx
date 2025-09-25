import { mockLookups } from '@/mocks/lookups.mock';
import { mockManagementActivityResponse } from '@/mocks/managementActivities.mock';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  cleanup,
  render,
  screen,
  userEvent,
  waitForElementToBeRemoved,
} from '@/utils/test-utils';
import { AxiosResponse } from 'axios';
import { ManagementActivityFilterModel } from '../models/ManagementActivityFilterModel';
import ManagementActivitiesListView from './ManagementActivitiesListView';

import { useApiManagementActivities } from '@/hooks/pims-api/useApiManagementActivities';
import fileDownload from 'js-file-download';
import { useManagementActivityExport } from '../../hooks/useManagementActivityExport';

vi.mock('@/hooks/pims-api/useApiManagementActivities');
vi.mock('../../hooks/useManagementActivityExport');
vi.mock('js-file-download');

const getManagementActivitiesPagedApiFn = vi.fn();
vi.mocked(useApiManagementActivities).mockReturnValue({
  getManagementActivitiesPagedApi: getManagementActivitiesPagedApiFn,
} as unknown as ReturnType<typeof useApiManagementActivities>);

const overviewExecuteFn = vi.fn();
const invoiceExecuteFn = vi.fn();
vi.mocked(useManagementActivityExport).mockReturnValue({
  generateManagementActivitiesOverviewReport: {
    execute: overviewExecuteFn,
    status: 0,
    response: undefined,
  },
  generateManagementActivitiesInvoiceReport: {
    execute: invoiceExecuteFn,
    status: 0,
    response: undefined,
  },
} as unknown as ReturnType<typeof useManagementActivityExport>);

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
  const setup = () => {
    return render(<ManagementActivitiesListView />, {
      store: { [lookupCodesSlice.name]: { lookupCodes: mockLookups } },
    });
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  afterEach(() => {
    cleanup();
  });

  it('matches snapshot', async () => {
    const results = mockPagedResults([mockManagementActivityResponse()]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);
    const { asFragment } = setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays search results', async () => {
    const results = mockPagedResults([mockManagementActivityResponse(1, 'test activity')]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);

    setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));

    expect(await screen.findByText(/test activity/i)).toBeInTheDocument();
  });

  it('displays error toast when api fails', async () => {
    getManagementActivitiesPagedApiFn.mockRejectedValue(new Error('network error'));
    setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    const toast = await screen.findByText('network error');
    expect(toast).toBeVisible();
  });

  it('calls overview export when button clicked', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);
    setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));

    const button = screen.getAllByTestId('excel-icon')[0];
    await act(async () => userEvent.click(button));

    expect(overviewExecuteFn).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ManagementActivityFilterModel>>({}),
    );
  });

  it('calls invoice export when button clicked', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);
    setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));

    const button = screen.getAllByTestId('excel-icon')[1];
    await act(async () => userEvent.click(button));

    expect(invoiceExecuteFn).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ManagementActivityFilterModel>>({}),
    );
  });

  it('downloads overview report when response is 200', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);

    (useManagementActivityExport as jest.Mock).mockReturnValue({
      generateManagementActivitiesOverviewReport: {
        execute: overviewExecuteFn,
        status: 200,
        response: new Blob(['test']),
      },
      generateManagementActivitiesInvoiceReport: {
        execute: invoiceExecuteFn,
        status: 0,
        response: undefined,
      },
    });

    setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    expect(fileDownload).toHaveBeenCalledWith(
      expect.any(Blob),
      'Management_Activities_Overview_Report.xlsx',
    );
  });

  it('downloads invoice report when response is 200', async () => {
    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);

    (useManagementActivityExport as jest.Mock).mockReturnValue({
      generateManagementActivitiesOverviewReport: {
        execute: overviewExecuteFn,
        status: 0,
        response: undefined,
      },
      generateManagementActivitiesInvoiceReport: {
        execute: invoiceExecuteFn,
        status: 200,
        response: new Blob(['invoice']),
      },
    });

    setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    expect(fileDownload).toHaveBeenCalledWith(
      expect.any(Blob),
      'Management_Activities_Invoice_Report.xlsx',
    );
  });

  it('shows modal when report has no data (204)', async () => {
    const setModalContent = vi.fn();
    const setDisplayModal = vi.fn();

    vi.mock('@/hooks/useModalContext', () => ({
      useModalContext: () => ({ setModalContent, setDisplayModal }),
    }));

    (useManagementActivityExport as jest.Mock).mockReturnValue({
      generateManagementActivitiesOverviewReport: {
        execute: overviewExecuteFn,
        status: 204,
        response: undefined,
      },
      generateManagementActivitiesInvoiceReport: {
        execute: invoiceExecuteFn,
        status: 0,
        response: undefined,
      },
    });

    const results = mockPagedResults([]);
    getManagementActivitiesPagedApiFn.mockResolvedValue(results);

    setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));

    expect(setModalContent).toHaveBeenCalledWith(
      expect.objectContaining({ message: expect.stringMatching(/no data/i) }),
    );
    expect(setDisplayModal).toHaveBeenCalledWith(true);
  });
});

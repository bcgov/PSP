import { AxiosResponse } from 'axios';
import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/index';
import { useApiManagementFile } from '@/hooks/pims-api/useApiManagementFile';
import { getMockApiAddress } from '@/mocks/address.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  cleanup,
  getByName,
  render,
  RenderOptions,
  screen,
  userEvent,
  waitForElementToBeRemoved,
} from '@/utils/test-utils';

import ManagementListView from './ManagementListView';
import { ManagementFilterModel } from './models';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';

vi.mock('@/hooks/pims-api/useApiManagementFile');
const getManagementFilesPagedApiFn = vi.fn();
const getAllManagementFileTeamMembersFn = vi.fn();
const exportManagementFilesFn = vi.fn();
vi.mocked(useApiManagementFile).mockReturnValue({
  getManagementFilesPagedApi: getManagementFilesPagedApiFn,
  getAllManagementFileTeamMembers: getAllManagementFileTeamMembersFn,
  exportManagementFiles: exportManagementFilesFn,
} as unknown as ReturnType<typeof useApiManagementFile>);

const mockPagedResults = (
  searchResults?: ApiGen_Concepts_ManagementFile[],
): Partial<AxiosResponse<ApiGen_Base_Page<ApiGen_Concepts_ManagementFile>, any>> => {
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

const history = createMemoryHistory();

describe('Management List View', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(<ManagementListView />, {
      ...renderOptions,
      claims: renderOptions?.claims || [Claims.MANAGEMENT_VIEW],
      history,
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
    });
    return {
      ...utils,
      getSearchButton: () => screen.getByTestId('search'),
    };
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  afterEach(() => {
    cleanup();
  });

  it('matches snapshot', async () => {
    const results = mockPagedResults([mockManagementFileResponse()]);
    getManagementFilesPagedApiFn.mockResolvedValue(results);
    const { asFragment } = setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    expect(asFragment()).toMatchSnapshot();
  });

  it('searches by file name', async () => {
    let results = mockPagedResults([]);
    getManagementFilesPagedApiFn.mockResolvedValue(results);

    const { getSearchButton } = setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    expect(await screen.queryByText(/test management/i)).toBeNull();

    results = mockPagedResults([mockManagementFileResponse(1, 'test management')]);
    getManagementFilesPagedApiFn.mockResolvedValue(results);

    const input = getByName('fileNameOrNumberOrReference');
    expect(input).not.toBeNull();
    await act(async () => userEvent.paste(input!, 'test management'));
    await act(async () => userEvent.click(getSearchButton()));

    expect(getManagementFilesPagedApiFn).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ManagementFilterModel>>({
        fileNameOrNumberOrReference: 'test management',
      }),
    );

    expect(await screen.findByText(/test management/i)).toBeInTheDocument();
  });

  it('searches by pid', async () => {
    const results = mockPagedResults([
      {
        ...mockManagementFileResponse(),
        fileProperties: [
          {
            id: 12,
            fileId: 1,
            propertyId: 1,
            property: {
              ...getEmptyProperty(),
              id: 1,
              address: getMockApiAddress(),
              pid: 123,
            },
            displayOrder: null,
            file: null,
            propertyName: null,
            isActive: null,
            location: null,
            rowVersion: null,
          },
        ],
      },
    ]);
    getManagementFilesPagedApiFn.mockResolvedValue(results);

    const { getSearchButton } = setup();
    await act(async () => {});

    const searchBy = getByName('searchBy');
    expect(searchBy).not.toBeNull();
    await act(async () => userEvent.selectOptions(searchBy!, 'pid'));

    const pidInput = getByName('pid');
    expect(pidInput).not.toBeNull();
    await act(async () => userEvent.paste(pidInput!, '123'));

    await act(async () => userEvent.click(getSearchButton()));

    expect(getManagementFilesPagedApiFn).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ManagementFilterModel>>({ pid: '123' }),
    );
    const address = getMockApiAddress().streetAddress1;
    expect(await screen.findByText(address!, { exact: false })).toBeInTheDocument();
  });

  it('displays an error toast when api call fails', async () => {
    getManagementFilesPagedApiFn.mockRejectedValue(new Error('network error'));
    setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    const toast = await screen.findByText('network error');
    expect(toast).toBeVisible();
  });

  it(`renders the 'add management' button when user has permissions`, async () => {
    const results = mockPagedResults([]);
    getManagementFilesPagedApiFn.mockResolvedValue(results);
    setup({ claims: [Claims.MANAGEMENT_VIEW, Claims.MANAGEMENT_ADD] });
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    const button = await screen.findByText(/Add a Management File/i);
    expect(button).toBeVisible();
  });

  it(`hides the 'add management' button when user has no permissions`, async () => {
    const results = mockPagedResults([]);
    getManagementFilesPagedApiFn.mockResolvedValue(results);
    setup({ claims: [Claims.MANAGEMENT_VIEW] });
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    const button = await screen.queryByText(/Add a Management File/i);
    expect(button).toBeNull();
  });

  it('navigates to create management route when user clicks add button', async () => {
    const results = mockPagedResults([]);
    getManagementFilesPagedApiFn.mockResolvedValue(results);
    setup({ claims: [Claims.MANAGEMENT_VIEW, Claims.MANAGEMENT_ADD] });
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    const button = await screen.findByText(/Add a Management File/i);
    expect(button).toBeVisible();
    await act(async () => userEvent.click(button));
    expect(history.location.pathname).toBe('/mapview/sidebar/management/new');
  });
});

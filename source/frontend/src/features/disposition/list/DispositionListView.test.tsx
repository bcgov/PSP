import { AxiosResponse } from 'axios';
import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/index';
import { useApiDispositionFile } from '@/hooks/pims-api/useApiDispositionFile';
import { IPagedItems } from '@/interfaces';
import { getMockApiAddress } from '@/mocks/address.mock';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  getByName,
  render,
  RenderOptions,
  screen,
  userEvent,
  waitForElementToBeRemoved,
} from '@/utils/test-utils';

import DispositionListView from './DispositionListView';
import { DispositionFilterModel } from './models';

jest.mock('@react-keycloak/web');

jest.mock('@/hooks/pims-api/useApiDispositionFile');
const getDispositionFilesPagedApiFn = jest.fn();
const getAllDispositionFileTeamMembersFn = jest.fn();
const exportDispositionFilesFn = jest.fn();
(useApiDispositionFile as jest.Mock).mockReturnValue({
  getDispositionFilesPagedApi: getDispositionFilesPagedApiFn,
  getAllDispositionFileTeamMembers: getAllDispositionFileTeamMembersFn,
  exportDispositionFiles: exportDispositionFilesFn,
});

const mockPagedResults = (
  searchResults?: ApiGen_Concepts_DispositionFile[],
): Partial<AxiosResponse<IPagedItems<ApiGen_Concepts_DispositionFile>, any>> => {
  const results = searchResults ?? [];
  const len = results.length;
  return {
    data: {
      items: results,
      quantity: len,
      total: len,
      page: 1,
      pageIndex: 0,
    },
  };
};

const history = createMemoryHistory();

describe('Disposition List View', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(<DispositionListView />, {
      ...renderOptions,
      claims: renderOptions?.claims || [Claims.DISPOSITION_VIEW],
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
    jest.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const results = mockPagedResults([mockDispositionFileResponse()]);
    getDispositionFilesPagedApiFn.mockResolvedValue(results);
    const { asFragment } = setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    expect(asFragment()).toMatchSnapshot();
  });

  it('searches by file name', async () => {
    let results = mockPagedResults([]);
    getDispositionFilesPagedApiFn.mockResolvedValue(results);

    const { getSearchButton } = setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    expect(await screen.queryByText(/test disposition/i)).toBeNull();

    results = mockPagedResults([mockDispositionFileResponse(1, 'test disposition')]);
    getDispositionFilesPagedApiFn.mockResolvedValue(results);

    const input = getByName('fileNameOrNumberOrReference');
    expect(input).not.toBeNull();
    await act(async () => userEvent.paste(input!, 'test disposition'));
    await act(async () => userEvent.click(getSearchButton()));

    expect(getDispositionFilesPagedApiFn).toHaveBeenCalledWith(
      expect.objectContaining<Partial<DispositionFilterModel>>({
        fileNameOrNumberOrReference: 'test disposition',
      }),
    );

    expect(await screen.findByText(/test disposition/i)).toBeInTheDocument();
  });

  it('searches by pid', async () => {
    const results = mockPagedResults([
      {
        ...mockDispositionFileResponse(),
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
            rowVersion: null,
          },
        ],
      },
    ]);
    getDispositionFilesPagedApiFn.mockResolvedValue(results);

    const { getSearchButton } = setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));

    const searchBy = getByName('searchBy');
    expect(searchBy).not.toBeNull();
    await act(async () => userEvent.selectOptions(searchBy!, 'pid'));

    const pidInput = getByName('pid');
    expect(pidInput).not.toBeNull();
    await act(async () => userEvent.paste(pidInput!, '123'));

    await act(async () => userEvent.click(getSearchButton()));

    expect(getDispositionFilesPagedApiFn).toHaveBeenCalledWith(
      expect.objectContaining<Partial<DispositionFilterModel>>({ pid: '123' }),
    );
    const address = getMockApiAddress().streetAddress1;
    expect(await screen.findByText(address!, { exact: false })).toBeInTheDocument();
  });

  it('displays an error toast when api call fails', async () => {
    getDispositionFilesPagedApiFn.mockRejectedValue(new Error('network error'));
    setup();
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    const toast = await screen.findByText('network error');
    expect(toast).toBeVisible();
  });

  it(`renders the 'add disposition' button when user has permissions`, async () => {
    const results = mockPagedResults([]);
    getDispositionFilesPagedApiFn.mockResolvedValue(results);
    setup({ claims: [Claims.DISPOSITION_VIEW, Claims.DISPOSITION_ADD] });
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    const button = await screen.findByText(/Add a Disposition File/i);
    expect(button).toBeVisible();
  });

  it(`hides the 'add disposition' button when user has no permissions`, async () => {
    const results = mockPagedResults([]);
    getDispositionFilesPagedApiFn.mockResolvedValue(results);
    setup({ claims: [Claims.DISPOSITION_VIEW] });
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    const button = await screen.queryByText(/Add a Disposition File/i);
    expect(button).toBeNull();
  });

  it('navigates to create disposition route when user clicks add button', async () => {
    const results = mockPagedResults([]);
    getDispositionFilesPagedApiFn.mockResolvedValue(results);
    setup({ claims: [Claims.DISPOSITION_VIEW, Claims.DISPOSITION_ADD] });
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    const button = await screen.findByText(/Add a Disposition File/i);
    expect(button).toBeVisible();
    await act(async () => userEvent.click(button));
    expect(history.location.pathname).toBe('/mapview/sidebar/disposition/new');
  });

  it('calls export function when export button clicked', async () => {
    const results = mockPagedResults([]);
    getDispositionFilesPagedApiFn.mockResolvedValue(results);
    setup({ claims: [Claims.DISPOSITION_VIEW, Claims.DISPOSITION_ADD] });
    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    const button = await screen.findByTestId(/excel-icon/i);
    expect(button).toBeVisible();
    await act(async () => userEvent.click(button));
    expect(exportDispositionFilesFn).toHaveBeenCalled();
  });
});

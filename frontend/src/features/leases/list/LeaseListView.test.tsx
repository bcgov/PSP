import userEvent from '@testing-library/user-event';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { ILeaseSearchResult } from 'interfaces';
import { act, fillInput, render, RenderOptions, waitFor } from 'utils/test-utils';

import { LeaseListView } from './LeaseListView';

jest.mock('hooks/pims-api/useApiLeases');
const getLeases = jest.fn();
(useApiLeases as jest.Mock).mockReturnValue({
  getLeases,
});

// render component under test
const setup = (renderOptions: RenderOptions = {}) => {
  const utils = render(<LeaseListView />, { ...renderOptions });
  const searchButton = utils.getByTestId('search');
  return { searchButton, ...utils };
};

const setupMockSearch = (searchResults?: ILeaseSearchResult[]) => {
  const results = searchResults ?? [];
  const len = results.length;
  getLeases.mockResolvedValue({
    data: {
      items: results,
      quantity: len,
      total: len,
      page: 1,
      pageIndex: 0,
    },
  });
};

describe('Lease and License List View', () => {
  beforeEach(() => {
    getLeases.mockClear();
  });

  it('matches snapshot', async () => {
    setupMockSearch();
    const { asFragment } = setup();

    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('searches by pid/pin', async () => {
    setupMockSearch([
      {
        id: 1,
        lFileNo: 'L-123-456',
        address: '123 mock st',
        pidOrPin: '123',
        programName: 'TRAN-IT',
        tenantName: 'Chester Tester',
      },
    ]);
    const { container, searchButton, findByText } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'pidOrPin', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        lFileNo: '',
        pidOrPin: '123',
        searchBy: 'pidOrPin',
        tenantName: '',
      }),
    );

    expect(await findByText(/123 mock st/i)).toBeInTheDocument();
  });

  it('searches by L-file number', async () => {
    setupMockSearch([
      {
        id: 1,
        lFileNo: 'L-123-456',
        address: '123 mock st',
        pidOrPin: '123',
        programName: 'TRAN-IT',
        tenantName: 'Chester Tester',
      },
    ]);
    const { container, searchButton, findByText } = setup({});
    fillInput(container, 'searchBy', 'lFileNo', 'select');
    fillInput(container, 'lFileNo', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        lFileNo: '123',
        pidOrPin: '',
        searchBy: 'lFileNo',
        tenantName: '',
      }),
    );

    expect(await findByText(/L-123-456/i)).toBeInTheDocument();
  });

  it('searches tenant name', async () => {
    setupMockSearch([
      {
        id: 1,
        lFileNo: 'L-123-456',
        address: '123 mock st',
        pidOrPin: '123',
        programName: 'TRAN-IT',
        tenantName: 'Chester Tester',
      },
    ]);
    const { container, searchButton, findByText } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'tenantName', 'Chester');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        lFileNo: '',
        pidOrPin: '',
        searchBy: 'pidOrPin',
        tenantName: 'Chester',
      }),
    );

    expect(await findByText(/Chester Tester/i)).toBeInTheDocument();
  });

  it('displays an error when no matching records found', async () => {
    setupMockSearch();
    const { container, searchButton, findAllByText } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'pidOrPin', 'foo-bar-baz');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        lFileNo: '',
        pidOrPin: 'foo-bar-baz',
        searchBy: 'pidOrPin',
        tenantName: '',
      }),
    );
    const toasts = await findAllByText('Lease / License details do not exist in PIMS inventory');
    expect(toasts[0]).toBeVisible();
  });

  it('displays an error when when Search API is unreachable', async () => {
    // simulate a network error
    getLeases.mockRejectedValue(new Error('network error'));
    const { container, searchButton, findAllByText } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'pidOrPin', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        lFileNo: '',
        pidOrPin: '123',
        searchBy: 'pidOrPin',
        tenantName: '',
      }),
    );
    const toasts = await findAllByText('network error');
    expect(toasts[0]).toBeVisible();
  });
});

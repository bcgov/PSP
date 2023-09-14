import userEvent from '@testing-library/user-event';

import { Claims } from '@/constants/index';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { ILeaseSearchResult } from '@/interfaces';
import { getUserMock } from '@/mocks/user.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fillInput,
  render,
  RenderOptions,
  waitFor,
  waitForElementToBeRemoved,
} from '@/utils/test-utils';

import { ILeaseFilter } from '..';
import { LeaseListView } from './LeaseListView';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: [] },
};

jest.mock('@react-keycloak/web');
jest.mock('@/hooks/pims-api/useApiLeases');
const getLeases = jest.fn();
(useApiLeases as jest.Mock).mockReturnValue({
  getLeases,
});

jest.mock('@/hooks/repositories/useUserInfoRepository');
(useUserInfoRepository as jest.Mock).mockReturnValue({
  retrieveUserInfo: jest.fn(),
  retrieveUserInfoLoading: true,
  retrieveUserInfoResponse: getUserMock(),
});

// render component under test
const setup = (renderOptions: RenderOptions = { store: storeState }) => {
  const utils = render(<LeaseListView />, { ...renderOptions, claims: [Claims.LEASE_VIEW] });
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
        programName: 'TRAN-IT',
        tenantNames: ['Chester Tester'],
        properties: [
          {
            id: 12,
            address: '123 mock st',
            pid: '123',
          },
        ],
      },
    ]);
    const { container, searchButton, findByText, getByTitle } = setup();
    await waitForElementToBeRemoved(getByTitle('table-loading'));

    act(() => {
      fillInput(container, 'searchBy', 'pinOrPid', 'select');
      fillInput(container, 'pinOrPid', '123');
    });
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pinOrPid: '123',
        searchBy: 'pinOrPid',
        tenantName: '',
        programs: [],
        leaseStatusTypes: ['ACTIVE'],
        expiryStartDate: '',
        expiryEndDate: '',
        regionType: '',
        details: '',
      }),
    );

    expect(await findByText(/TRAN-IT/i)).toBeInTheDocument();
  });

  it('searches by L-file number', async () => {
    setupMockSearch([
      {
        id: 1,
        lFileNo: 'L-123-456',
        programName: 'TRAN-IT',
        tenantNames: ['Chester Tester'],
        properties: [{ id: 1234, address: '123 mock st', pin: '123' }],
      },
    ]);
    const { container, searchButton, findByText } = setup();
    fillInput(container, 'searchBy', 'lFileNo', 'select');
    fillInput(container, 'lFileNo', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        lFileNo: '123',
        pinOrPid: '',
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
        programName: 'TRAN-IT',
        tenantNames: ['Chester Tester'],
        properties: [{ id: 123, address: '123 mock st', pin: '123' }],
      },
    ]);
    const { container, searchButton, findByText } = setup();

    fillInput(container, 'searchBy', 'pinOrPid', 'select');
    fillInput(container, 'tenantName', 'Chester');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pinOrPid: '',
        searchBy: 'pinOrPid',
        tenantName: 'Chester',
        programs: [],
        leaseStatusTypes: ['ACTIVE'],
        expiryStartDate: '',
        expiryEndDate: '',
        regionType: '',
        details: '',
      }),
    );

    expect(await findByText(/Chester Tester/i)).toBeInTheDocument();
  });

  it('displays an error when no matching records found', async () => {
    setupMockSearch();
    const { container, searchButton, findAllByText } = setup();

    fillInput(container, 'searchBy', 'pinOrPid', 'select');
    fillInput(container, 'pinOrPid', 'foo-bar-baz');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pinOrPid: 'foo-bar-baz',
        searchBy: 'pinOrPid',
        tenantName: '',
        programs: [],
        leaseStatusTypes: ['ACTIVE'],
        expiryStartDate: '',
        expiryEndDate: '',
        regionType: '',
        details: '',
      }),
    );
    const toasts = await findAllByText('Lease / License details do not exist in PIMS inventory');
    expect(toasts[0]).toBeVisible();
  });

  it('displays an error when when Search API is unreachable', async () => {
    // simulate a network error
    getLeases.mockRejectedValue(new Error('network error'));
    const { container, searchButton, findAllByText } = setup();

    fillInput(container, 'searchBy', 'pinOrPid', 'select');
    fillInput(container, 'pinOrPid', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pinOrPid: '123',
        searchBy: 'pinOrPid',
        tenantName: '',
        programs: [],
        leaseStatusTypes: ['ACTIVE'],
        expiryStartDate: '',
        expiryEndDate: '',
        regionType: '',
        details: '',
      }),
    );
    const toasts = await findAllByText('network error');
    expect(toasts[0]).toBeVisible();
  });
});

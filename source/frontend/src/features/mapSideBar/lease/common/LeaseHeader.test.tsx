import { getMockApiLease } from '@/mocks/lease.mock';
import { prettyFormatUTCDate } from '@/utils';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { server } from '@/mocks/msw/server';
import { getUserMock } from '@/mocks/user.mock';
import { http, HttpResponse } from 'msw';
import { ILeaseHeaderProps, LeaseHeader } from './LeaseHeader';

describe('LeaseHeader component', () => {
  // render component under test
  const setup = (props: ILeaseHeaderProps, renderOptions: RenderOptions = {}) => {
    const utils = render(<LeaseHeader lease={props.lease} lastUpdatedBy={props.lastUpdatedBy} />, {
      ...renderOptions,
    });

    return { ...utils };
  };

  beforeEach(() => {
    server.use(
      http.get('/api/users/info/*', () => HttpResponse.json(getUserMock())),
      http.get('/api/properties/:id/historicalNumbers', () => HttpResponse.json([])),
    );
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected when no data is provided', async () => {
    const testLease = getMockApiLease();
    const { asFragment } = setup({ lease: testLease, lastUpdatedBy: null });
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders as expected when a lease is provided', async () => {
    const testLease = getMockApiLease();
    const { getByText, getAllByText } = setup({
      lease: testLease,
      lastUpdatedBy: {
        parentId: testLease.id || 0,
        appLastUpdateUserid: testLease.appLastUpdateUserid || '',
        appLastUpdateUserGuid: testLease.appLastUpdateUserGuid || '',
        appLastUpdateTimestamp: testLease.appLastUpdateTimestamp || '',
      },
    });
    await act(async () => {});

    expect(getByText(testLease.lFileNo!)).toBeVisible();
    expect(getByText(testLease.appCreateUserid!)).toBeVisible();
    expect(getByText(testLease.appLastUpdateUserid!)).toBeVisible();
    expect(
      getAllByText(new RegExp(prettyFormatUTCDate(testLease.appCreateTimestamp)))[0],
    ).toBeVisible();
    expect(
      getAllByText(new RegExp(prettyFormatUTCDate(testLease.appLastUpdateTimestamp)))[0],
    ).toBeVisible();
  });

  it('renders the last-update-time when provided', async () => {
    const testDate = new Date().toISOString();
    const testLease = getMockApiLease();
    const { getAllByText } = setup({
      lease: testLease,
      lastUpdatedBy: {
        parentId: testLease.id || 0,
        appLastUpdateUserid: 'Test User Id',
        appLastUpdateUserGuid: 'TEST GUID',
        appLastUpdateTimestamp: testDate,
      },
    });
    await act(async () => {});

    expect(
      getAllByText(new RegExp(prettyFormatUTCDate(testLease.appCreateTimestamp)))[0],
    ).toBeVisible();
    expect(
      getAllByText(new RegExp(prettyFormatUTCDate(testLease.appLastUpdateTimestamp)))[0],
    ).toBeVisible();
  });
});

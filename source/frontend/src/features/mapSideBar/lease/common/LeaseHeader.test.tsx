import { getMockApiLease } from '@/mocks/lease.mock';
import { prettyFormatUTCDate } from '@/utils';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { server } from '@/mocks/msw/server';
import { getUserMock } from '@/mocks/user.mock';
import { ApiGen_CodeTypes_LeaseAccountTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseAccountTypes';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
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

  beforeAll(() => {
    // Lock the current datetime as some of the tests have date-dependent fields (e.g. isLeaseExpired)
    vi.useFakeTimers();
    vi.setSystemTime(new Date('04 Dec 2023 10:15:00 GMT').getTime());
  });

  beforeEach(() => {
    server.use(
      http.get('/api/users/info/*', () => HttpResponse.json(getUserMock())),
      http.get('/api/properties/:id/historicalNumbers', () => HttpResponse.json([])),
    );
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  afterAll(() => {
    // back to reality...
    vi.useRealTimers();
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

  it('renders whether the lease is RECEIVABLE or PAYABLE', async () => {
    const testLease = getMockApiLease();
    const { getByText } = setup({
      lease: {
        ...testLease,
        paymentReceivableType: {
          id: ApiGen_CodeTypes_LeaseAccountTypes.RCVBL,
          description: 'Receivable',
          displayOrder: null,
          isDisabled: false,
        },
      },
      lastUpdatedBy: null,
    });
    await act(async () => {});

    expect(getByText(testLease.lFileNo!)).toBeVisible();
    expect(getByText('Receivable')).toBeVisible();
  });

  it('renders indicator for EXPIRED leases', async () => {
    const testLease = getMockApiLease();
    const { getByText } = setup({
      lease: {
        ...testLease,
        expiryDate: new Date('2022-05-28').toISOString(),
      },
      lastUpdatedBy: null,
    });
    await act(async () => {});

    expect(getByText(testLease.lFileNo!)).toBeVisible();
    expect(getByText('EXPIRED')).toBeVisible();
  });

  it.each([
    [
      ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED,
      'Terminated',
      new Date('2022-05-28').toISOString(),
      true,
    ],
    [
      ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE,
      'Active',
      new Date('2022-05-28').toISOString(),
      false,
    ],
  ])(
    'renders termination date only for TERMINATED leases - status: %s',
    async (
      statusId: string,
      statusDescription: string,
      testDate: string,
      shouldDisplay: boolean,
    ) => {
      const testLease = getMockApiLease();
      const { getByText, getAllByText, queryByText } = setup({
        lease: {
          ...testLease,
          fileStatusTypeCode: {
            id: statusId,
            description: statusDescription,
            displayOrder: null,
            isDisabled: false,
          },
          terminationDate: testDate,
        },
        lastUpdatedBy: null,
      });
      await act(async () => {});

      expect(getByText(testLease.lFileNo!)).toBeVisible();
      expect(getByText(statusDescription.toUpperCase())).toBeVisible();

      if (shouldDisplay) {
        expect(getAllByText(new RegExp(prettyFormatUTCDate(testDate)))[0]).toBeVisible();
      } else {
        expect(queryByText(new RegExp(prettyFormatUTCDate(testDate)))).toBeNull();
      }
    },
  );
});

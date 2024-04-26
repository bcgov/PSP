import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { getMockApiLease } from '@/mocks/lease.mock';
import { prettyFormatUTCDate } from '@/utils';
import { render, RenderOptions } from '@/utils/test-utils';

import { ILeaseHeaderProps, LeaseHeader } from './LeaseHeader';

const mockAxios = new MockAdapter(axios);

describe('LeaseHeader component', () => {
  // render component under test
  const setup = (props: ILeaseHeaderProps, renderOptions: RenderOptions = {}) => {
    const utils = render(<LeaseHeader lease={props.lease} lastUpdatedBy={props.lastUpdatedBy} />, {
      ...renderOptions,
    });

    return { ...utils };
  };

  beforeEach(() => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
  });

  afterEach(() => {
    mockAxios.reset();
    vi.clearAllMocks();
  });

  it('renders as expected when no data is provided', () => {
    const testLease = getMockApiLease();
    const { asFragment } = setup({ lease: testLease, lastUpdatedBy: null });
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

    expect(getByText(testLease.lFileNo!)).toBeVisible();
    expect(getByText(testLease.appCreateUserid!)).toBeVisible();
    expect(getByText(testLease.appLastUpdateUserid!)).toBeVisible();
    expect(getAllByText(prettyFormatUTCDate(testLease.appCreateTimestamp))[0]).toBeVisible();
    expect(getAllByText(prettyFormatUTCDate(testLease.appLastUpdateTimestamp))[0]).toBeVisible();
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

    expect(getAllByText(prettyFormatUTCDate(testLease.appCreateTimestamp))[0]).toBeVisible();
    expect(getAllByText(prettyFormatUTCDate(testLease.appLastUpdateTimestamp))[0]).toBeVisible();
  });
});

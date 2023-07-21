import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { getMockApiLease } from '@/mocks/lease.mock';
import { prettyFormatDate } from '@/utils';
import { render, RenderOptions } from '@/utils/test-utils';

import { ILeaseHeaderProps, LeaseHeader } from './LeaseHeader';

const mockAxios = new MockAdapter(axios);

describe('LeaseHeader component', () => {
  // render component under test
  const setup = (props: ILeaseHeaderProps, renderOptions: RenderOptions = {}) => {
    const utils = render(<LeaseHeader lease={props.lease} />, {
      ...renderOptions,
    });

    return { ...utils };
  };

  beforeEach(() => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
  });

  afterEach(() => {
    mockAxios.reset();
    jest.clearAllMocks();
  });

  it('renders as expected when no data is provided', () => {
    const testLease = getMockApiLease();
    const { asFragment } = setup({ lease: testLease });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders as expected when a lease is provided', async () => {
    const testLease = getMockApiLease();
    const { getByText, getAllByText } = setup({ lease: testLease });

    expect(getByText(testLease.lFileNo!)).toBeVisible();
    expect(getByText(testLease.appCreateUserid!)).toBeVisible();
    expect(getByText(testLease.appLastUpdateUserid!)).toBeVisible();
    expect(getAllByText(prettyFormatDate(testLease.appCreateTimestamp))[0]).toBeVisible();
    expect(getAllByText(prettyFormatDate(testLease.appLastUpdateTimestamp))[0]).toBeVisible();
  });
});

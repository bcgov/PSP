import { screen } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from '@/utils/test-utils';

import { IUserNameTooltipProps, UserNameTooltip } from './UserNameTooltip';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('UserNameTooltip', () => {
  const mockAxios = new MockAdapter(axios);
  const setup = (renderOptions?: RenderOptions & IUserNameTooltipProps) => {
    // render component under test
    const component = render(
      <UserNameTooltip
        userGuid={renderOptions?.userGuid ?? ''}
        userName={renderOptions?.userName ?? ''}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    mockAxios.reset();
  });

  it('renders as expected', async () => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
    const { asFragment } = setup();
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('should call the API Endpoint with given userGuid', async () => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
    setup({
      userGuid: 'D64886C0-72D7-48B9-BA4E-EF1506F93308',
      userName: 'USER',
    });
    await waitFor(() => {
      expect(mockAxios.history.get).toHaveLength(1);
      expect(mockAxios.history.get[0].url).toBe(`/users/info/D64886C0-72D7-48B9-BA4E-EF1506F93308`);
    });
  });
  it('should have the USER text in the component', async () => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
    setup({ userGuid: 'D64886C0-72D7-48B9-BA4E-EF1506F93308', userName: 'USER' });
    expect(screen.getByText(`USER`)).toBeInTheDocument();
  });
});

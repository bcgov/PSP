import { cleanup } from '@testing-library/react-hooks';
import { Formik } from 'formik';
import noop from 'lodash/noop';

import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { renderAsync, RenderOptions, waitFor } from '@/utils/test-utils';

import { SelectProps } from '../Select';
import {
  IUserRegionSelectContainerProps,
  UserRegionSelectContainer,
} from './UserRegionSelectContainer';
import { ApiGen_Concepts_RegionUser } from '@/models/api/generated/ApiGen_Concepts_RegionUser';
import { ApiGen_Concepts_User } from '@/models/api/generated/ApiGen_Concepts_User';

// mock auth library

const retrieveUserInfo = vi.fn();

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
vi.mock('@/hooks/repositories/useUserInfoRepository');
vi.mocked(useUserInfoRepository).mockReturnValue({
  retrieveUserInfo,
  retrieveUserInfoLoading: true,
  retrieveUserInfoResponse: {
    userRegions: [
      {
        id: 1,
        userId: 5,
        regionCode: 1,
      } as ApiGen_Concepts_RegionUser,
      {
        id: 2,
        userId: 5,
        regionCode: 2,
      } as ApiGen_Concepts_RegionUser,
    ],
  } as ApiGen_Concepts_User,
});

describe('User Region Select', () => {
  const setup = (
    renderOptions: RenderOptions & IUserRegionSelectContainerProps & Partial<SelectProps> = {
      field: 'region',
    },
  ) => {
    return renderAsync(
      <Formik onSubmit={noop} initialValues={{ classifiactionid: '' }}>
        <UserRegionSelectContainer {...renderOptions} />
      </Formik>,
      {
        ...renderOptions,
        store: renderOptions.store ?? storeState,
        claims: [],
      },
    );
  };
  it('renders correctly', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  afterEach(() => {
    cleanup();
  });

  it('makes a request for user data', async () => {
    await setup();
    await waitFor(() => {
      expect(retrieveUserInfo).toHaveBeenCalledWith('00000000-0000-0000-0000-000000000000');
    });
  });

  it('filters the list of displayed regions based on user regions', async () => {
    const { findByTestId } = await setup();
    const option1 = await findByTestId('select-option-1');
    const option2 = await findByTestId('select-option-2');

    expect(option1).toBeVisible();
    expect(option2).toBeVisible();
  });

  it('displays all region option if specified in props', async () => {
    const { findByTestId } = await setup({ includeAll: true, field: 'region' });
    const option1 = await findByTestId('select-option-');

    expect(option1).toBeVisible();
  });

  it('does not display all region option by default', async () => {
    const { queryByTestId } = await setup({ includeAll: false, field: 'region' });
    const option1 = await queryByTestId('select-option-');

    expect(option1).toBeNull();
  });

  it('displays a tooltip if user has no regions', async () => {
    vi.resetAllMocks();
    vi.mocked(useUserInfoRepository).mockReturnValue({
      retrieveUserInfo,
      retrieveUserInfoLoading: true,
      retrieveUserInfoResponse: {
        userRegions: [],
      } as ApiGen_Concepts_User,
    });
    const { findByTestId } = await setup();

    const tooltop = await findByTestId('tooltip-icon-region-tooltip');

    expect(tooltop).toBeVisible();
  });

  it('displays a tooltip if system has no regions', async () => {
    const { findByTestId } = await setup({
      store: {
        [lookupCodesSlice.name]: { lookupCodes: [] },
      },
      field: 'region',
    });
    const tooltip = await findByTestId('tooltip-icon-region-tooltip');

    expect(tooltip).toBeVisible();
  });
});

import { IProjectFilter } from '@/features/projects/interfaces';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { IProjectFilterProps, ProjectFilter } from './ProjectFilter';
import { ApiGen_Concepts_UserRole } from '@/models/api/generated/ApiGen_Concepts_UserRole';
import { ApiGen_Concepts_RegionUser } from '@/models/api/generated/ApiGen_Concepts_RegionUser';
import { ApiGen_Concepts_User } from '@/models/api/generated/ApiGen_Concepts_User';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const setFilter = vi.fn();

// render component under test
const setup = (
  renderOptions: RenderOptions & IProjectFilterProps = { store: storeState, setFilter },
) => {
  const { filter, setFilter: setFilterFn, ...rest } = renderOptions;
  const utils = render(<ProjectFilter filter={filter} setFilter={setFilterFn} />, {
    ...rest,
    claims: [],
  });
  const searchButton = utils.getByTestId('search');
  const resetButton = utils.getByTestId('reset-button');
  return { searchButton, resetButton, setFilter: setFilterFn, ...utils };
};

const retrieveUserInfo = vi.fn();
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

describe('Project Filter', () => {
  beforeEach(() => {
    setFilter.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('searches by project number', async () => {
    const { container, searchButton, setFilter } = setup();

    fillInput(container, 'projectNumber', '1201');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<IProjectFilter>({
        projectName: '',
        projectNumber: '1201',
        projectStatusCode: 'AC',
        projectRegionCode: '',
      }),
    );
  });

  it('searches by project name', async () => {
    const { container, searchButton, setFilter } = setup();

    fillInput(container, 'projectName', 'Hwy');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<IProjectFilter>({
        projectName: 'Hwy',
        projectNumber: '',
        projectStatusCode: 'AC',
        projectRegionCode: '',
      }),
    );
  });

  it('searches by region', async () => {
    const { container, searchButton, setFilter } = setup();

    fillInput(container, 'projectRegionCode', '2', 'select');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<IProjectFilter>({
        projectName: '',
        projectNumber: '',
        projectStatusCode: 'AC',
        projectRegionCode: '2',
      }),
    );
  });

  it('searches by status', async () => {
    const { container, searchButton, setFilter } = setup();

    fillInput(container, 'projectStatusCode', 'PL', 'select');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<IProjectFilter>({
        projectName: '',
        projectNumber: '',
        projectStatusCode: 'PL',
        projectRegionCode: '',
      }),
    );
  });
});

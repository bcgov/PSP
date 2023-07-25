import userEvent from '@testing-library/user-event';

import { Claims } from '@/constants/index';
import { IResearchFilter } from '@/features/research/interfaces';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, waitFor } from '@/utils/test-utils';

import ResearchFilter, { defaultResearchFilter } from './ResearchFilter';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

jest.mock('@react-keycloak/web');

const setFilter = jest.fn();
// render component under test
const setup = (renderOptions: RenderOptions = { store: storeState }) => {
  const utils = render(<ResearchFilter setFilter={setFilter} />, {
    ...renderOptions,
    claims: [Claims.RESEARCH_VIEW],
  });
  const searchButton = utils.getByTestId('search');
  const resetButton = utils.getByTestId('reset-button');
  return { searchButton, setFilter, resetButton, ...utils };
};

describe('Research Filter', () => {
  beforeEach(() => {
    setFilter.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup();

    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('searches for active research files by default', async () => {
    const { resetButton } = setup();
    await act(async () => userEvent.click(resetButton));

    expect(setFilter).toHaveBeenCalledWith(defaultResearchFilter);
  });

  it('searches by region', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'regionCode', 1, 'select');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith({
      appCreateUserid: '',
      appLastUpdateUserid: '',
      createOrUpdateBy: 'appLastUpdateUserid',
      createOrUpdateRange: 'updatedOnStartDate',
      createdOnEndDate: '',
      createdOnStartDate: '',
      name: '',
      regionCode: '1',
      researchFileStatusTypeCode: 'ACTIVE',
      researchSearchBy: 'name',
      rfileNumber: '',
      roadOrAlias: '',
      updatedOnEndDate: '',
      updatedOnStartDate: '',
    });
  });

  it('searches by R-file number', async () => {
    const { container, searchButton } = setup();
    fillInput(container, 'researchSearchBy', 'rfileNumber', 'select');
    fillInput(container, 'rfileNumber', '101');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        appCreateUserid: '',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        regionCode: '',
        researchFileStatusTypeCode: 'ACTIVE',
        researchSearchBy: 'rfileNumber',
        rfileNumber: '101',
        roadOrAlias: '',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
      }),
    );
  });

  it('searches by file name', async () => {
    const { container, searchButton } = setup();
    fillInput(container, 'researchSearchBy', 'name', 'select');
    fillInput(container, 'name', 'test file name 1');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        appCreateUserid: '',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: 'test file name 1',
        regionCode: '',
        researchFileStatusTypeCode: 'ACTIVE',
        researchSearchBy: 'name',
        rfileNumber: '',
        roadOrAlias: '',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
      }),
    );
  });

  it('searches by research file status', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'researchFileStatusTypeCode', 'INACTIVE', 'select');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        appCreateUserid: '',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        regionCode: '',
        researchFileStatusTypeCode: 'INACTIVE',
        researchSearchBy: 'name',
        rfileNumber: '',
        roadOrAlias: '',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
      }),
    );
  });

  it('searches by road name', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'roadOrAlias', 'a road name');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        appCreateUserid: '',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        regionCode: '',
        researchFileStatusTypeCode: 'ACTIVE',
        researchSearchBy: 'name',
        rfileNumber: '',
        roadOrAlias: 'a road name',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
      }),
    );
  });

  it('searches by create date range', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'createOrUpdateRange', 'createdOnStartDate', 'select');
    fillInput(container, 'createdOnStartDate', '2020-01-01');
    fillInput(container, 'createdOnEndDate', '2020-02-02');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        appCreateUserid: '',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'createdOnStartDate',
        createdOnEndDate: '2020-02-02',
        createdOnStartDate: '2020-01-01',
        name: '',
        regionCode: '',
        researchFileStatusTypeCode: 'ACTIVE',
        researchSearchBy: 'name',
        rfileNumber: '',
        roadOrAlias: '',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
      }),
    );
  });

  it('searches by update date range', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'createOrUpdateRange', 'updatedOnStartDate', 'select');
    fillInput(container, 'updatedOnStartDate', '2021-01-01');
    fillInput(container, 'updatedOnEndDate', '2021-02-02');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        appCreateUserid: '',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        regionCode: '',
        researchFileStatusTypeCode: 'ACTIVE',
        researchSearchBy: 'name',
        rfileNumber: '',
        roadOrAlias: '',
        updatedOnEndDate: '2021-02-02',
        updatedOnStartDate: '2021-01-01',
      }),
    );
  });

  it('searches by create user', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'createOrUpdateBy', 'appCreateUserid', 'select');
    fillInput(container, 'appCreateUserid', 'createUser');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        appCreateUserid: 'createUser',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appCreateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        regionCode: '',
        researchFileStatusTypeCode: 'ACTIVE',
        researchSearchBy: 'name',
        rfileNumber: '',
        roadOrAlias: '',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
      }),
    );
  });

  it('searches by update user', async () => {
    const { container, searchButton, setFilter } = setup();

    fillInput(container, 'createOrUpdateBy', 'appLastUpdateUserid', 'select');
    fillInput(container, 'appLastUpdateUserid', 'updateUser');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        appCreateUserid: '',
        appLastUpdateUserid: 'updateUser',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        regionCode: '',
        researchFileStatusTypeCode: 'ACTIVE',
        researchSearchBy: 'name',
        rfileNumber: '',
        roadOrAlias: '',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
      }),
    );
  });

  it('resets the filter when reset button is clicked', async () => {
    const { container, resetButton, setFilter } = setup();

    fillInput(container, 'createOrUpdateBy', 'appLastUpdateUserid', 'select');
    fillInput(container, 'appLastUpdateUserid', 'breaking');
    await act(async () => userEvent.click(resetButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<IResearchFilter>(defaultResearchFilter),
    );
  });
});

import userEvent from '@testing-library/user-event';

import { Claims } from '@/constants/index';
import { IResearchFilter } from '@/features/research/interfaces';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, waitFor } from '@/utils/test-utils';

import ResearchFilter, { defaultResearchFilter } from './ResearchFilter';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const setFilter = vi.fn();
// render component under test
const setup = (
  filter = defaultResearchFilter,
  createdByOptions: MultiSelectOption[] = [],
  renderOptions: RenderOptions = { store: storeState },
) => {
  const utils = render(
    <ResearchFilter filter={filter} setFilter={setFilter} createdByOptions={createdByOptions} />,
    {
      ...renderOptions,
      claims: [Claims.RESEARCH_VIEW],
    },
  );
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
    const { container, getByText, searchButton } = setup();

    const regionsInput = container.querySelector('#multiselect-regionCodes_input');
    await act(async () => userEvent.click(regionsInput));
    await act(async () => userEvent.click(getByText('South Coast Region')));
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith({
      pid: '',
      pin: '',
      appCreateUserid: '',
      appLastUpdateUserid: '',
      createOrUpdateBy: 'appLastUpdateUserid',
      createOrUpdateRange: 'updatedOnStartDate',
      createdOnEndDate: '',
      createdOnStartDate: '',
      name: '',
      regionCodes: [{ id: '1', text: 'South Coast Region' }],
      researchFileStatusTypeCode: '',
      researchSearchBy: 'pid',
      rfileNumber: '',
      roadOrAlias: '',
      updatedOnEndDate: '',
      updatedOnStartDate: '',
      selectedUser: [],
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
        regionCodes: [],
        researchFileStatusTypeCode: '',
        researchSearchBy: 'rfileNumber',
        rfileNumber: '101',
        roadOrAlias: '',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
        selectedUser: [],
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
        regionCodes: [],
        researchFileStatusTypeCode: '',
        researchSearchBy: 'name',
        rfileNumber: '',
        roadOrAlias: '',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
        selectedUser: [],
      }),
    );
  });

  it('searches by research file status', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'researchFileStatusTypeCode', 'INACTIVE', 'select');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        pid: '',
        pin: '',
        appCreateUserid: '',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        regionCodes: [],
        researchFileStatusTypeCode: 'INACTIVE',
        researchSearchBy: 'pid',
        rfileNumber: '',
        roadOrAlias: '',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
        selectedUser: [],
      }),
    );
  });

  it('searches by road name', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'roadOrAlias', 'a road name');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        pid: '',
        pin: '',
        appCreateUserid: '',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        regionCodes: [],
        researchFileStatusTypeCode: '',
        researchSearchBy: 'pid',
        rfileNumber: '',
        roadOrAlias: 'a road name',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
        selectedUser: [],
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
        pid: '',
        pin: '',
        appCreateUserid: '',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'createdOnStartDate',
        createdOnEndDate: '2020-02-02',
        createdOnStartDate: '2020-01-01',
        name: '',
        regionCodes: [],
        researchFileStatusTypeCode: '',
        researchSearchBy: 'pid',
        rfileNumber: '',
        roadOrAlias: '',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
        selectedUser: [],
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
        pid: '',
        pin: '',
        appCreateUserid: '',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        regionCodes: [],
        researchFileStatusTypeCode: '',
        researchSearchBy: 'pid',
        rfileNumber: '',
        roadOrAlias: '',
        updatedOnEndDate: '2021-02-02',
        updatedOnStartDate: '2021-01-01',
        selectedUser: [],
      }),
    );
  });

  it('searches by create user', async () => {
    const selectedUser = [{ id: 'DSMITH', text: 'Devin Smith (DSMITH)' }];
    const { searchButton } = setup(
      { ...defaultResearchFilter, createOrUpdateBy: 'appCreateUserid', selectedUser },
      selectedUser,
    );

    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        pid: '',
        pin: '',
        appCreateUserid: 'DSMITH',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appCreateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        regionCodes: [],
        researchFileStatusTypeCode: '',
        researchSearchBy: 'pid',
        rfileNumber: '',
        roadOrAlias: '',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
        selectedUser: [
          {
            id: 'DSMITH',
            text: 'Devin Smith (DSMITH)',
          },
        ],
      }),
    );
  });

  it('searches by update user', async () => {
    const selectedUser = [{ id: 'DSMITH', text: 'Devin Smith (DSMITH)' }];
    const { searchButton } = setup(
      { ...defaultResearchFilter, createOrUpdateBy: 'appLastUpdateUserid', selectedUser },
      selectedUser,
    );

    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        pid: '',
        pin: '',
        appCreateUserid: '',
        appLastUpdateUserid: 'DSMITH',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        regionCodes: [],
        researchFileStatusTypeCode: '',
        researchSearchBy: 'pid',
        rfileNumber: '',
        roadOrAlias: '',
        updatedOnEndDate: '',
        updatedOnStartDate: '',
        selectedUser: [
          {
            id: 'DSMITH',
            text: 'Devin Smith (DSMITH)',
          },
        ],
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

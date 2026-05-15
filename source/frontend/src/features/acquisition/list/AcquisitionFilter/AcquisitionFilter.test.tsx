import userEvent from '@testing-library/user-event';

import { Claims } from '@/constants/index';
import { getMockLookUpsByType, mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, waitFor } from '@/utils/test-utils';
import * as API from '@/constants/API';

import { AcquisitionFilterModel } from '../interfaces';
import { AcquisitionFilter } from './AcquisitionFilter';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';

const setFilter = vi.fn();
const onResetFilter = vi.fn();

const mockFilterModel = new AcquisitionFilterModel();

const acquisitionStatusTypes = getMockLookUpsByType(API.ACQUISITION_FILE_STATUS_TYPES);
const teamProfileTypes = getMockLookUpsByType(API.ACQUISITION_FILE_TEAM_PROFILE_TYPES);

const mockTeamMemberOptions: MultiSelectOption[] = [{
  id: 'P-1001',
  text: 'John Doe',
}];

// render component under test
const setup = (renderOptions: RenderOptions = {}) => {
  const utils = render(
    <AcquisitionFilter
      setFilter={setFilter}
      initialValues={mockFilterModel}
      pimsRegionsOptions={[]}
      acquisitionTeamOptions={mockTeamMemberOptions}
      acquisitionStatusOptions={acquisitionStatusTypes}
      teamProfileOptions={teamProfileTypes}
      onResetFilter={onResetFilter}
    />,
    {
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
      claims: [Claims.ACQUISITION_VIEW],
      ...renderOptions,
    },
  );

  const teamMemberInput = utils.container.querySelector(`#multiselect-acquisitionTeamMembers`) as HTMLElement;
  const searchButton = utils.getByTestId('search');
  const resetButton = utils.getByTestId('reset-button');
  const hasNOCCheckbox = utils.container.querySelector(
    `input[name="hasNoticeOfClaim"]`,
  ) as HTMLInputElement;
  return { searchButton, hasNOCCheckbox, setFilter, resetButton, teamMemberInput, ...utils };
};

describe('Acquisition Filter', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup();

    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('searches for active acquisition files by default', async () => {
    const { searchButton } = setup();

    await act(async () => userEvent.click(searchButton));
    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining(new AcquisitionFilterModel().toApi()),
    );
  });

  it('searches by acquisition file status', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'acquisitionFileStatusTypeCode', 'CANCEL', 'select');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        acquisitionFileStatusTypeCode: 'CANCEL',
        acquisitionFileNameOrNumber: '',
        projectNameOrNumber: '',
      }),
    );
  });

  it('searches by acquisition file name or number', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'acquisitionFileNameOrNumber', 'an acquisition file name');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        acquisitionFileStatusTypeCode: '',
        acquisitionFileNameOrNumber: 'an acquisition file name',
        projectNameOrNumber: '',
      }),
    );
  });

  it('searches by acquisition file Owner name', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'ownerName', 'DOE');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        ownerName: 'DOE',
      }),
    );
  });

  it('searches by Notice of Claim', async () => {
    const { searchButton, hasNOCCheckbox } = setup();

    await act(async () => userEvent.click(hasNOCCheckbox));
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        hasNoticeOfClaim: true,
      }),
    );
  });

  it('searches by ministry project name or number', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'projectNameOrNumber', 'Hwy 14 improvements');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        acquisitionFileStatusTypeCode: '',
        acquisitionFileNameOrNumber: '',
        projectNameOrNumber: 'Hwy 14 improvements',
      }),
    );
  });

  it('searches by team member role and team member', async () => {
    const { container, searchButton, teamMemberInput, getByText, queryByText } = setup();

    fillInput(container, 'acquisitionTeamMemberProfileTypeCode', 'EXPRAGENT', 'select');
    await act(async () => userEvent.click(searchButton));

    expect(getByText('Team member is required')).toBeInTheDocument();

    await act(async () => {
      userEvent.click(teamMemberInput);
    });

    await act(async () => {
      userEvent.type(teamMemberInput, 'John Doe');
    });

    await act(async () => {
      const firstOption = container.querySelector(`div.optionListContainer ul li`) as HTMLElement;
      userEvent.click(firstOption);
    });

    expect(queryByText('Team member is required')).toBeNull();

    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        acquisitionTeamMemberProfileTypeCode: 'EXPRAGENT',
        acquisitionTeamMemberPersonId: "1001"
      }),
    );
  });

  it('resets the filter when reset button is clicked', async () => {
    const { container, resetButton } = setup();

    fillInput(container, 'acquisitionFileNameOrNumber', 'breaking');
    await act(async () => userEvent.click(resetButton));

    expect(onResetFilter).toHaveBeenCalledTimes(1);
  });
});

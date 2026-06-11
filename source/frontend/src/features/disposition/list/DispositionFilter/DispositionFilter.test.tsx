import userEvent from '@testing-library/user-event';

import {
  DISPOSITION_FILE_STATUS_TYPES,
  DISPOSITION_STATUS_TYPES,
  DISPOSITION_TEAM_PROFILE_TYPES,
  DISPOSITION_TYPES,
} from '@/constants/API';
import { Claims } from '@/constants/index';
import { getMockLookUpsByType, mockLookups } from '@/mocks/lookups.mock';
import { Api_DispositionFilter } from '@/models/api/DispositionFilter';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, getByName, render, RenderOptions, screen } from '@/utils/test-utils';

import { DispositionFilterModel } from '../models';
import DispositionFilter from './DispositionFilter';
import { SelectOption } from '@/components/common/form';

const setFilter = vi.fn();
const onResetFilter = vi.fn();

const fileStatusOptions = getMockLookUpsByType(DISPOSITION_FILE_STATUS_TYPES);
const dispositionStatusOptions = getMockLookUpsByType(DISPOSITION_STATUS_TYPES);
const dispositionTypeOptions = getMockLookUpsByType(DISPOSITION_TYPES);
const teamProfileOptions = getMockLookUpsByType(DISPOSITION_TEAM_PROFILE_TYPES);

const mockFilterModel = new DispositionFilterModel();

const mockTeamMemberOptions: SelectOption[] = [
  {
    value: 'P-1001',
    label: 'John Doe',
  },
];

describe('Disposition filter', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <DispositionFilter
        initialValues={mockFilterModel}
        dispositionTeam={[]}
        fileStatusOptions={fileStatusOptions}
        dispositionStatusOptions={dispositionStatusOptions}
        dispositionTypeOptions={dispositionTypeOptions}
        pimsRegionsOptions={[]}
        dispositionTeamOptions={mockTeamMemberOptions}
        teamProfileOptions={teamProfileOptions}
        setFilter={setFilter}
        onResetFilter={onResetFilter}
      />,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        claims: [Claims.DISPOSITION_VIEW],
        ...renderOptions,
      },
    );
    return {
      ...utils,
      getTeamMemberInput: () =>
        utils.container.querySelector(`#typeahead-select-dispositionTeamMember`) as HTMLElement,
      getSearchButton: () => screen.getByTestId('search'),
      getResetButton: () => screen.getByTestId('reset-button'),
    };
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('searches for active disposition files by default', async () => {
    const { getSearchButton } = setup();

    await act(async () => userEvent.click(getSearchButton()));
    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Api_DispositionFilter>(new DispositionFilterModel().toApi()),
    );
  });

  it('searches by disposition file status', async () => {
    const { getSearchButton } = setup();

    const dropdown = getByName('dispositionFileStatusCode');
    expect(dropdown).not.toBeNull();
    await act(async () => userEvent.selectOptions(dropdown!, 'CANCELLED'));
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Partial<Api_DispositionFilter>>({
        dispositionFileStatusCode: 'CANCELLED',
      }),
    );
  });

  it('searches by disposition status', async () => {
    const { getSearchButton } = setup();

    const dropdown = getByName('dispositionStatusCode');
    expect(dropdown).not.toBeNull();
    await act(async () => userEvent.selectOptions(dropdown!, 'SOLD'));
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Partial<Api_DispositionFilter>>({
        dispositionStatusCode: 'SOLD',
      }),
    );
  });

  it('searches by disposition type', async () => {
    const { getSearchButton } = setup();

    const dropdown = getByName('dispositionTypeCode');
    expect(dropdown).not.toBeNull();
    await act(async () => userEvent.selectOptions(dropdown!, 'DIRECT'));
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Partial<Api_DispositionFilter>>({
        dispositionTypeCode: 'DIRECT',
      }),
    );
  });

  it('searches by disposition file name or number', async () => {
    const { getSearchButton } = setup();

    const input = getByName('fileNameOrNumberOrReference');
    expect(input).not.toBeNull();
    await act(async () => userEvent.paste(input!, 'test disposition'));
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Partial<Api_DispositionFilter>>({
        fileNameOrNumberOrReference: 'test disposition',
      }),
    );
  });

  it('searches by team member role and team member', async () => {
    const { container, getSearchButton, getTeamMemberInput, getByText, queryByText } = setup();

    fillInput(container, 'dispositionTeamMemberProfileTypeCode', 'MOTILEAD', 'select');
    await act(async () => userEvent.click(getSearchButton()));

    expect(getByText('Team member is required')).toBeInTheDocument();

    await act(async () => {
      userEvent.click(getTeamMemberInput());
    });

    await act(async () => {
      userEvent.type(getTeamMemberInput(), 'John Doe');
    });

    await act(async () => {
      const firstOption = container.querySelector(
        `div#typeahead-select-dispositionTeamMember a`,
      ) as HTMLElement;
      userEvent.click(firstOption);
    });

    expect(queryByText('Team member is required')).toBeNull();

    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        teamMemberProfileTypeCode: 'MOTILEAD',
        teamMemberPersonId: 1001,
      }),
    );
  });

  it('resets the filter when reset button is clicked', async () => {
    const { getResetButton } = setup();

    const input = getByName('fileNameOrNumberOrReference');
    expect(input).not.toBeNull();
    await act(async () => userEvent.paste(input!, 'test disposition'));
    await act(async () => userEvent.click(getResetButton()));

    expect(onResetFilter).toHaveBeenCalledTimes(1);
  });
});

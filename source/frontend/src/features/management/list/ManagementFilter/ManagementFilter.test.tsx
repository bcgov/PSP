import userEvent from '@testing-library/user-event';
import { http, HttpResponse } from 'msw';

import {
  MANAGEMENT_FILE_STATUS_TYPES,
  MANAGEMENT_FILE_PURPOSE_TYPES,
  MANAGEMENT_TEAM_PROFILE_TYPES,
} from '@/constants/API';
import { Claims } from '@/constants/index';
import { getMockLookUpsByType, mockLookups } from '@/mocks/lookups.mock';
import { server } from '@/mocks/msw/server';
import { getUserMock } from '@/mocks/user.mock';
import { Api_ManagementFilter } from '@/models/api/ManagementFilter';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, getByName, render, RenderOptions, screen } from '@/utils/test-utils';

import { ManagementFilterModel } from '../models';
import ManagementFilter, { IManagementFilterProps } from './ManagementFilter';
import { SelectOption } from '@/components/common/form';

const setFilter = vi.fn();
const onResetFilter = vi.fn();

const fileStatusOptions = getMockLookUpsByType(MANAGEMENT_FILE_STATUS_TYPES);
const managementFilePurposeOptions = getMockLookUpsByType(MANAGEMENT_FILE_PURPOSE_TYPES);
const teamProfileTypes = getMockLookUpsByType(MANAGEMENT_TEAM_PROFILE_TYPES);

const mockTeamMemberOptions: SelectOption[] = [
  {
    value: 'P-1001',
    label: 'John Doe',
  },
];

const mockFilterModel = new ManagementFilterModel();

describe('Management filter', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IManagementFilterProps> },
  ) => {
    const utils = render(
      <ManagementFilter
        {...renderOptions.props}
        initialValues={renderOptions.props?.initialValues ?? mockFilterModel}
        managementTeamOptions={renderOptions.props?.managementTeamOptions ?? mockTeamMemberOptions}
        fileStatusOptions={renderOptions.props?.fileStatusOptions ?? fileStatusOptions}
        managementPurposeOptions={
          renderOptions.props?.managementPurposeOptions ?? managementFilePurposeOptions
        }
        pimsRegionsOptions={renderOptions.props?.pimsRegionsOptions ?? []}
        teamProfileOptions={teamProfileTypes}
        setFilter={setFilter}
        onResetFilter={onResetFilter}
      />,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        claims: [Claims.MANAGEMENT_VIEW],
        ...renderOptions,
      },
    );

    // wait for useEffects
    await act(async () => {});

    return {
      ...utils,
      getTeamMemberInput: () =>
        utils.container.querySelector(`#typeahead-select-managementTeamMember`) as HTMLElement,
      getSearchButton: () => screen.getByTestId('search'),
      getResetButton: () => screen.getByTestId('reset-button'),
      getHasNOCCheckbox: () =>
        utils.container.querySelector(`input[name="hasNoticeOfClaim"]`) as HTMLInputElement,
    };
  };

  beforeEach(() => {
    server.use(
      http.get('/api/users/info/*', () => HttpResponse.json(getUserMock(), { status: 200 })),
    );
    setFilter.mockClear();
    onResetFilter.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('searches for active management files by default', async () => {
    const { getSearchButton } = await setup({});

    await act(async () => userEvent.click(getSearchButton()));
    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Api_ManagementFilter>(new ManagementFilterModel().toApi()),
    );
  });

  it('searches by management file status', async () => {
    const { getSearchButton } = await setup({});

    const dropdown = getByName('managementFileStatusCode');
    expect(dropdown).not.toBeNull();
    await act(async () => userEvent.selectOptions(dropdown!, 'CANCELLED'));
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Partial<Api_ManagementFilter>>({
        managementFileStatusCode: 'CANCELLED',
      }),
    );
  });

  it('searches by management purpose', async () => {
    const { getSearchButton } = await setup({});

    const dropdown = getByName('managementFilePurposeCode');
    expect(dropdown).not.toBeNull();
    await act(async () => userEvent.selectOptions(dropdown!, 'ENGINEER'));
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Partial<Api_ManagementFilter>>({
        managementFilePurposeCode: 'ENGINEER',
      }),
    );
  });

  it('searches by management project', async () => {
    const { getSearchButton } = await setup({});

    const input = getByName('projectNameOrNumber');
    expect(input).not.toBeNull();
    await act(async () => userEvent.paste(input!, 'test project'));
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Partial<Api_ManagementFilter>>({
        projectNameOrNumber: 'test project',
      }),
    );
  });

  it('searches by management file name or number', async () => {
    const { getSearchButton } = await setup({});

    const input = getByName('fileNameOrNumberOrReference');
    expect(input).not.toBeNull();
    await act(async () => userEvent.paste(input!, 'test management'));
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Partial<Api_ManagementFilter>>({
        fileNameOrNumberOrReference: 'test management',
      }),
    );
  });

  it('searches by team member role and team member', async () => {
    const { container, getSearchButton, getTeamMemberInput, getByText, queryByText } = await setup(
      {},
    );

    fillInput(container, 'managementTeamMemberProfileTypeCode', 'MINSTAFF', 'select');
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
        `div#typeahead-select-managementTeamMember a`,
      ) as HTMLElement;
      userEvent.click(firstOption);
    });

    expect(queryByText('Team member is required')).toBeNull();

    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        teamMemberProfileTypeCode: 'MINSTAFF',
        teamMemberPersonId: 1001,
      }),
    );
  });

  it('searches by file has NOC', async () => {
    const { getSearchButton, getHasNOCCheckbox } = await setup({});

    await act(async () => userEvent.click(getHasNOCCheckbox()));
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Partial<Api_ManagementFilter>>({
        hasNoticeOfClaim: true,
      }),
    );
  });

  it('resets the filter when reset button is clicked', async () => {
    const { getResetButton } = await setup({});

    const input = getByName('fileNameOrNumberOrReference');
    expect(input).not.toBeNull();
    await act(async () => userEvent.paste(input!, 'test management'));
    await act(async () => userEvent.click(getResetButton()));

    expect(onResetFilter).toHaveBeenCalledTimes(1);
  });
});

import userEvent from '@testing-library/user-event';

import { MANAGEMENT_FILE_STATUS_TYPES, MANAGEMENT_FILE_PURPOSE_TYPES } from '@/constants/API';
import { Claims } from '@/constants/index';
import { getMockLookUpsByType, mockLookups } from '@/mocks/lookups.mock';
import { Api_ManagementFilter } from '@/models/api/ManagementFilter';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, getByName, render, RenderOptions, screen } from '@/utils/test-utils';

import { ManagementFilterModel } from '../models';
import ManagementFilter from './ManagementFilter';

const setFilter = vi.fn();

const fileStatusOptions = getMockLookUpsByType(MANAGEMENT_FILE_STATUS_TYPES);
const managementFilePurposeOptions = getMockLookUpsByType(MANAGEMENT_FILE_PURPOSE_TYPES);

describe('Management filter', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <ManagementFilter
        setFilter={setFilter}
        managementTeam={[]}
        fileStatusOptions={fileStatusOptions}
        managementPurposeOptions={managementFilePurposeOptions}
      />,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        claims: [Claims.MANAGEMENT_VIEW],
        ...renderOptions,
      },
    );
    return {
      ...utils,
      getSearchButton: () => screen.getByTestId('search'),
      getResetButton: () => screen.getByTestId('reset-button'),
    };
  };

  beforeEach(() => {
    setFilter.mockClear();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('searches for active management files by default', async () => {
    const { getResetButton } = setup();
    await act(async () => userEvent.click(getResetButton()));
    expect(setFilter).toHaveBeenCalledWith(new ManagementFilterModel().toApi());
  });

  it('searches by management file status', async () => {
    const { getSearchButton } = setup();

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
    const { getSearchButton } = setup();

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
    const { getSearchButton } = setup();

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
    const { getSearchButton } = setup();

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

  it('resets the filter when reset button is clicked', async () => {
    const { getResetButton } = setup();

    const input = getByName('fileNameOrNumberOrReference');
    expect(input).not.toBeNull();
    await act(async () => userEvent.paste(input!, 'test management'));
    await act(async () => userEvent.click(getResetButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Api_ManagementFilter>(new ManagementFilterModel().toApi()),
    );
  });
});

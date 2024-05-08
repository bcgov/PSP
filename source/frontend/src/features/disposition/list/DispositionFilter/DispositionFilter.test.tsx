import userEvent from '@testing-library/user-event';

import {
  DISPOSITION_FILE_STATUS_TYPES,
  DISPOSITION_STATUS_TYPES,
  DISPOSITION_TYPES,
} from '@/constants/API';
import { Claims } from '@/constants/index';
import { getMockLookUpsByType, mockLookups } from '@/mocks/lookups.mock';
import { Api_DispositionFilter } from '@/models/api/DispositionFilter';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, getByName, render, RenderOptions, screen } from '@/utils/test-utils';

import { DispositionFilterModel } from '../models';
import DispositionFilter from './DispositionFilter';

const setFilter = vi.fn();

const fileStatusOptions = getMockLookUpsByType(DISPOSITION_FILE_STATUS_TYPES);
const dispositionStatusOptions = getMockLookUpsByType(DISPOSITION_STATUS_TYPES);
const dispositionTypeOptions = getMockLookUpsByType(DISPOSITION_TYPES);

describe('Disposition filter', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <DispositionFilter
        setFilter={setFilter}
        dispositionTeam={[]}
        fileStatusOptions={fileStatusOptions}
        dispositionStatusOptions={dispositionStatusOptions}
        dispositionTypeOptions={dispositionTypeOptions}
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

  it('searches for active disposition files by default', async () => {
    const { getResetButton } = setup();
    await act(async () => userEvent.click(getResetButton()));
    expect(setFilter).toHaveBeenCalledWith(new DispositionFilterModel().toApi());
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

  it('resets the filter when reset button is clicked', async () => {
    const { getResetButton } = setup();

    const input = getByName('fileNameOrNumberOrReference');
    expect(input).not.toBeNull();
    await act(async () => userEvent.paste(input!, 'test disposition'));
    await act(async () => userEvent.click(getResetButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Api_DispositionFilter>(new DispositionFilterModel().toApi()),
    );
  });
});

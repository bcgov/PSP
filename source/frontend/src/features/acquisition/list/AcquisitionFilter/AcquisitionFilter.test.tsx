import userEvent from '@testing-library/user-event';

import { Claims } from '@/constants/index';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, waitFor } from '@/utils/test-utils';

import { AcquisitionFilterModel, ApiGen_Concepts_AcquisitionFilter } from '../interfaces';
import { AcquisitionFilter } from './AcquisitionFilter';

const setFilter = vi.fn();

// render component under test
const setup = (renderOptions: RenderOptions = {}) => {
  const utils = render(<AcquisitionFilter setFilter={setFilter} acquisitionTeam={[]} />, {
    store: {
      [lookupCodesSlice.name]: { lookupCodes: mockLookups },
    },
    claims: [Claims.ACQUISITION_VIEW],
    ...renderOptions,
  });
  const searchButton = utils.getByTestId('search');
  const resetButton = utils.getByTestId('reset-button');
  return { searchButton, setFilter, resetButton, ...utils };
};

describe('Acquisition Filter', () => {
  beforeEach(() => {
    setFilter.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup();

    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('searches for active acquisition files by default', async () => {
    const { resetButton } = setup();
    await act(async () => userEvent.click(resetButton));

    expect(setFilter).toHaveBeenCalledWith(new AcquisitionFilterModel().toApi());
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
        acquisitionFileStatusTypeCode: 'ACTIVE',
        acquisitionFileNameOrNumber: 'an acquisition file name',
        projectNameOrNumber: '',
      }),
    );
  });

  it('searches by ministry project name or number', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'projectNameOrNumber', 'Hwy 14 improvements');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        acquisitionFileStatusTypeCode: 'ACTIVE',
        acquisitionFileNameOrNumber: '',
        projectNameOrNumber: 'Hwy 14 improvements',
      }),
    );
  });

  it('resets the filter when reset button is clicked', async () => {
    const { container, resetButton, setFilter } = setup();

    fillInput(container, 'acquisitionFileNameOrNumber', 'breaking');
    await act(async () => userEvent.click(resetButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ApiGen_Concepts_AcquisitionFilter>(
        new AcquisitionFilterModel().toApi(),
      ),
    );
  });
});

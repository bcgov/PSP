import userEvent from '@testing-library/user-event';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { act, fillInput, render, RenderOptions } from 'utils/test-utils';

import { LeaseListView } from './LeaseListView';

jest.mock('hooks/pims-api/useApiLeases');
const getLeases = jest.fn();
(useApiLeases as jest.Mock).mockReturnValue({
  getLeases,
});

const setup = (renderOptions: RenderOptions = {}) => {
  // render component under test
  const utils = render(<LeaseListView />, { ...renderOptions });
  const searchButton = utils.getByTestId('search');
  return { searchButton, ...utils };
};

describe('Lease and License List View', () => {
  beforeEach(() => {
    getLeases.mockResolvedValue({ data: { items: [] } });
  });

  it('searches by pid/pin', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'pidOrPin', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        lFileNo: '',
        pidOrPin: '123',
        searchBy: 'pidOrPin',
        tenantName: '',
      }),
    );
  });

  it('searches l file number', async () => {
    const { container, searchButton } = setup({});
    fillInput(container, 'searchBy', 'lFileNo', 'select');
    fillInput(container, 'lFileNo', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        lFileNo: '123',
        pidOrPin: '',
        searchBy: 'lFileNo',
        tenantName: '',
      }),
    );
  });

  it('searches tenant name', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'tenantName', 'tenant');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        lFileNo: '',
        pidOrPin: '',
        searchBy: 'pidOrPin',
        tenantName: 'tenant',
      }),
    );
  });

  it('displays an error when no matching PID/PIN found', async () => {
    const { container, searchButton, findAllByText } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'pidOrPin', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        lFileNo: '',
        pidOrPin: '123',
        searchBy: 'pidOrPin',
        tenantName: '',
      }),
    );
    const toasts = await findAllByText('There are no records for this PID/PIN');
    expect(toasts[0]).toBeVisible();
  });

  it('displays an error when no matching L-File # found', async () => {
    const { container, searchButton, findAllByText } = setup();

    fillInput(container, 'searchBy', 'lFileNo', 'select');
    fillInput(container, 'lFileNo', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        lFileNo: '123',
        pidOrPin: '',
        searchBy: 'lFileNo',
        tenantName: '',
      }),
    );
    const toasts = await findAllByText('There are no records for this L-File #');
    expect(toasts[0]).toBeVisible();
  });

  it('displays an error when no matching tenant found', async () => {
    const { container, searchButton, findAllByText } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'tenantName', 'tenant');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        lFileNo: '',
        pidOrPin: '',
        searchBy: 'pidOrPin',
        tenantName: 'tenant',
      }),
    );
    const toasts = await findAllByText('There are no records for this Tenant Name');
    expect(toasts[0]).toBeVisible();
  });
});

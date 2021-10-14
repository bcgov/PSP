import userEvent from '@testing-library/user-event';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { defaultTenant } from 'tenants';
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

const mockFetch = () =>
  Promise.resolve({ json: () => Promise.resolve(JSON.stringify(defaultTenant)) }) as Promise<
    Response
  >;

describe('Lease and License List View', () => {
  beforeEach(() => {
    getLeases.mockResolvedValue({ data: { items: [] } });
    global.fetch = mockFetch as any;
  });

  it('searches by pid/pin', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'pidOrPin', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith({
      lFileNo: '',
      pidOrPin: '123',
      searchBy: 'pidOrPin',
      tenantName: '',
    });
  });

  it('searches l file number', async () => {
    const { container, searchButton } = setup({});
    fillInput(container, 'searchBy', 'lFileNo', 'select');
    fillInput(container, 'lFileNo', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith({
      lFileNo: '123',
      pidOrPin: '',
      searchBy: 'lFileNo',
      tenantName: '',
    });
  });

  it('searches tenant name', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'tenantName', 'tenant');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith({
      lFileNo: '',
      pidOrPin: '',
      searchBy: 'pidOrPin',
      tenantName: 'tenant',
    });
  });

  it('displays an error when no matching PID/PIN found', async () => {
    const { container, searchButton, findByText } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'pidOrPin', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith({
      lFileNo: '',
      pidOrPin: '123',
      searchBy: 'pidOrPin',
      tenantName: '',
    });
    expect(await findByText('There is no record for this PID/ PIN')).toBeVisible();
  });

  it('displays an error when no matching L-File # found', async () => {
    const { container, searchButton, findByText } = setup();

    fillInput(container, 'searchBy', 'lFileNo', 'select');
    fillInput(container, 'lFileNo', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith({
      lFileNo: '123',
      pidOrPin: '',
      searchBy: 'lFileNo',
      tenantName: '',
    });
    expect(await findByText('There is no record for this L-File #')).toBeVisible();
  });

  it('displays an error when no matching tenant found', async () => {
    const { container, searchButton, findByText } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'tenantName', 'tenant');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith({
      lFileNo: '',
      pidOrPin: '',
      searchBy: 'pidOrPin',
      tenantName: 'tenant',
    });
    expect(await findByText('There is no record for this Tenant Name')).toBeVisible();
  });
});

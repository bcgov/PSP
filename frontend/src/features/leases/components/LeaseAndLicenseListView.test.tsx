import { act, render } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { defaultTenant } from 'tenants';
import { fillInput } from 'utils/test-utils';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import { defaultFilter } from './LeaseAndLicenseFilter';
import { LeaseAndLicenseListView } from './LeaseAndLicenseListView';

jest.mock('hooks/pims-api/useApiLeases');
const getLeases = jest.fn();
(useApiLeases as jest.Mock).mockReturnValue({
  getLeases,
});

const renderContainer = ({ store }: any) =>
  render(
    <TestCommonWrapper>
      <LeaseAndLicenseListView />
    </TestCommonWrapper>,
  );
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
    const { container, getByTestId } = renderContainer({});
    fillInput(container, 'pidOrPin', '123');
    const searchButton = getByTestId('search');
    await act(async () => {
      userEvent.click(searchButton);
      expect(getLeases).toHaveBeenCalledWith({
        ...defaultFilter,
        pidOrPin: '123',
      });
    });
  });

  it('searches l file number', async () => {
    const { container, getByTestId } = renderContainer({});
    fillInput(container, 'searchBy', 'lFileNo', 'select');
    fillInput(container, 'lFileNo', '123');
    const searchButton = getByTestId('search');
    await act(async () => {
      userEvent.click(searchButton);
      expect(getLeases).toHaveBeenCalledWith({
        ...defaultFilter,
        lFileNo: '123',
        searchBy: 'lFileNo',
      });
    });
  });

  it('searches address name', async () => {
    const { container, getByTestId } = renderContainer({});
    fillInput(container, 'address', 'address');
    const searchButton = getByTestId('search');
    await act(async () => {
      userEvent.click(searchButton);
      expect(getLeases).toHaveBeenCalledWith({
        ...defaultFilter,
        address: 'address',
      });
    });
  });

  it('searches expiry date', async () => {
    const { container, getByTestId } = renderContainer({});
    fillInput(container, 'expiryDate', '09/07/2021', 'datepicker');
    const searchButton = getByTestId('search');
    await act(async () => {
      userEvent.click(searchButton);
      expect(getLeases).toHaveBeenCalledWith({
        ...defaultFilter,
        expiryDate: '2021-09-07',
      });
    });
  });

  it('searches tenant name', async () => {
    const { container, getByTestId } = renderContainer({});
    fillInput(container, 'tenantName', 'tenant');
    const searchButton = getByTestId('search');
    await act(async () => {
      userEvent.click(searchButton);
      expect(getLeases).toHaveBeenCalledWith({
        ...defaultFilter,
        tenantName: 'tenant',
      });
    });
  });

  it('searches municipality', async () => {
    const { container, getByTestId } = renderContainer({});
    fillInput(container, 'municipality', 'municipality');
    const searchButton = getByTestId('search');
    await act(async () => {
      userEvent.click(searchButton);
      expect(getLeases).toHaveBeenCalledWith({
        ...defaultFilter,
        municipality: 'municipality',
      });
    });
  });

  it('searches multiple fields', async () => {
    const { container, getByTestId } = renderContainer({});
    fillInput(container, 'municipality', 'municipality');
    fillInput(container, 'tenantName', 'tenant');
    fillInput(container, 'expiryDate', '09/07/2021', 'datepicker');
    fillInput(container, 'address', 'address');
    fillInput(container, 'pidOrPin', '123');
    const searchButton = getByTestId('search');
    await act(async () => {
      userEvent.click(searchButton);
      expect(getLeases).toHaveBeenCalledWith({
        ...defaultFilter,
        municipality: 'municipality',
        tenantName: 'tenant',
        expiryDate: '2021-09-07',
        address: 'address',
        pidOrPin: '123',
      });
    });
  });

  it('displays on error for no results', async () => {
    const { container, getByTestId, findByText } = renderContainer({});
    fillInput(container, 'pidOrPin', '123');
    const searchButton = getByTestId('search');
    await act(async () => {
      userEvent.click(searchButton);
      expect(getLeases).toHaveBeenCalledWith({
        ...defaultFilter,
        searchBy: 'pidOrPin',
        pidOrPin: '123',
      });
      await findByText('There are no records for your search criteria.');
    });
  });
});

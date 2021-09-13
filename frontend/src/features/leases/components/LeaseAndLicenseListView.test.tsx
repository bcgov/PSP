import { act, prettyDOM, render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { fillInput } from 'utils/test-utils';
import TestCommonWrapper from 'utils/TestCommonWrapper';

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

describe('Lease and License List View', () => {
  beforeEach(() => {
    getLeases.mockResolvedValue({ data: { items: [] } });
  });
  it('searches by pid/pin', async () => {
    const { container, getByTestId } = renderContainer({});
    fillInput(container, 'pidOrPin', '123');
    const searchButton = getByTestId('search');
    await act(async () => {
      userEvent.click(searchButton);
      expect(getLeases).toHaveBeenCalledWith({
        lFileNo: '',
        pidOrPin: '123',
        searchBy: 'pidOrPin',
        tenantName: '',
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
        lFileNo: '123',
        pidOrPin: '',
        searchBy: 'lFileNo',
        tenantName: '',
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
        lFileNo: '',
        pidOrPin: '',
        searchBy: 'pidOrPin',
        tenantName: 'tenant',
      });
    });
  });

  it('displays on error for now results when searching by pid/pin', async () => {
    const { container, getByTestId, findByText } = renderContainer({});
    fillInput(container, 'pidOrPin', '123');
    const searchButton = getByTestId('search');
    await act(async () => {
      userEvent.click(searchButton);
      expect(getLeases).toHaveBeenCalledWith({
        lFileNo: '',
        pidOrPin: '123',
        searchBy: 'pidOrPin',
        tenantName: '',
      });
      await findByText('There is no record for this PID/ PIN');
    });
  });

  it('displays on error for now results when searching l file number', async () => {
    const { container, getByTestId, findByText } = renderContainer({});
    fillInput(container, 'searchBy', 'lFileNo', 'select');
    fillInput(container, 'lFileNo', '123');
    const searchButton = getByTestId('search');
    await act(async () => {
      userEvent.click(searchButton);
      expect(getLeases).toHaveBeenCalledWith({
        lFileNo: '123',
        pidOrPin: '',
        searchBy: 'lFileNo',
        tenantName: '',
      });
      await findByText('There is no record for this L-File #');
    });
  });

  it('displays on error for now results when searching tenant name', async () => {
    const { container, getByTestId, findByText } = renderContainer({});
    fillInput(container, 'tenantName', 'tenant');
    const searchButton = getByTestId('search');
    await act(async () => {
      userEvent.click(searchButton);
      expect(getLeases).toHaveBeenCalledWith({
        lFileNo: '',
        pidOrPin: '',
        searchBy: 'pidOrPin',
        tenantName: 'tenant',
      });
      await findByText('There is no record for this Tenant Name');
    });
  });
});

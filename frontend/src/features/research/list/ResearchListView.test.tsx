import userEvent from '@testing-library/user-event';
import { Claims } from 'constants/index';
import { useApiResearch } from 'hooks/pims-api/useApiResearch';
import { IResearchSearchResult } from 'interfaces/IResearchSearchResult';
import React from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, waitFor } from 'utils/test-utils';

import { IResearchFilter } from '../interfaces';
import { ResearchListView } from './ResearchListView';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: [] },
};

jest.mock('@react-keycloak/web');
jest.mock('hooks/pims-api/useApiResearch');
const getResearchFiles = jest.fn();
(useApiResearch as jest.Mock).mockReturnValue({
  getResearchFiles,
});

// render component under test
const setup = (renderOptions: RenderOptions = { store: storeState }) => {
  const utils = render(<ResearchListView />, { ...renderOptions, claims: [Claims.LEASE_VIEW] });
  const searchButton = utils.getByTestId('search');
  return { searchButton, ...utils };
};

const setupMockSearch = (searchResults?: IResearchSearchResult[]) => {
  const results = searchResults ?? [];
  const len = results.length;
  getResearchFiles.mockResolvedValue({
    data: {
      items: results,
      quantity: len,
      total: len,
      page: 1,
      pageIndex: 0,
    },
  });
};

describe('Lease and License List View', () => {
  beforeEach(() => {
    getResearchFiles.mockClear();
  });

  it('matches snapshot', async () => {
    setupMockSearch();
    const { asFragment } = setup();

    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('searches by pid/pin', async () => {
    setupMockSearch([]);
    const { container, searchButton, findByText } = setup();

    fillInput(container, 'searchBy', 'pinOrPid', 'select');
    fillInput(container, 'pinOrPid', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith();

    expect(await findByText(/TRAN-IT/i)).toBeInTheDocument();
  });

  it('searches by L-file number', async () => {
    setupMockSearch([]);
    const { container, searchButton, findByText } = setup();
    fillInput(container, 'searchBy', 'lFileNo', 'select');
    fillInput(container, 'lFileNo', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith(expect.objectContaining({}));

    expect(await findByText(/L-123-456/i)).toBeInTheDocument();
  });

  it('searches tenant name', async () => {
    setupMockSearch([]);
    const { container, searchButton, findByText } = setup();

    fillInput(container, 'searchBy', 'pinOrPid', 'select');
    fillInput(container, 'tenantName', 'Chester');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith();

    expect(await findByText(/Chester Tester/i)).toBeInTheDocument();
  });

  it('displays an error when no matching records found', async () => {
    setupMockSearch();
    const { container, searchButton, findAllByText } = setup();

    fillInput(container, 'searchBy', 'pinOrPid', 'select');
    fillInput(container, 'pinOrPid', 'foo-bar-baz');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith();
    const toasts = await findAllByText('Lease / License details do not exist in PIMS inventory');
    expect(toasts[0]).toBeVisible();
  });

  it('displays an error when when Search API is unreachable', async () => {
    // simulate a network error
    getResearchFiles.mockRejectedValue(new Error('network error'));
    const { container, searchButton, findAllByText } = setup();

    fillInput(container, 'searchBy', 'pinOrPid', 'select');
    fillInput(container, 'pinOrPid', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith();
    const toasts = await findAllByText('network error');
    expect(toasts[0]).toBeVisible();
  });
});

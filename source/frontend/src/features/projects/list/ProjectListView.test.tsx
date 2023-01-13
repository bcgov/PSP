import userEvent from '@testing-library/user-event';
import { Claims } from 'constants/index';
import { useApiProjects } from 'hooks/pims-api/useApiProjects';
import { IProjectSearchResult } from 'interfaces';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, waitFor } from 'utils/test-utils';

import { IProjectFilter } from '..';
import { ProjectListView } from './ProjectListView';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: [] },
};

jest.mock('@react-keycloak/web');
jest.mock('hooks/pims-api/useApiProjects');
const getProjects = jest.fn();
(useApiProjects as jest.Mock).mockReturnValue({
  getProjects,
});

const setupMockSearch = (searchResults?: IProjectSearchResult[]) => {
  const results = searchResults ?? [];
  const len = results.length;
  getProjects.mockResolvedValue({
    data: {
      items: results,
      quantity: len,
      total: len,
      page: 1,
      pageIndex: 0,
    },
  });
};

// render component under test
const setup = (renderOptions: RenderOptions = { store: storeState }) => {
  const utils = render(<ProjectListView />, { ...renderOptions, claims: [Claims.PROJECT_VIEW] });
  const searchButton = utils.getByTestId('search');
  return { searchButton, ...utils };
};

describe('Lease and License List View', () => {
  beforeEach(() => {
    getProjects.mockClear();
  });

  it('searches by Project Name', async () => {
    setupMockSearch([
      {
        id: 1,
        projectName: 'PROJECT-NAME',
        projectNumber: '9999',
        lastUpdatedBy: 'USER',
        lastUpdatedDate: new Date(2023, 1, 1),
      },
    ]);
    const { container, searchButton, findByText } = setup();

    fillInput(container, 'searchBy', 'pinOrPid', 'select');
    fillInput(container, 'pinOrPid', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getProjects).toHaveBeenCalledWith(
      expect.objectContaining<IProjectFilter>({
        projectStatusType: 'ACTIVE',
        regionType: '',
      }),
    );

    expect(await findByText(/TRAN-IT/i)).toBeInTheDocument();
  });
});

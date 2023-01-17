import userEvent from '@testing-library/user-event';
import { Claims } from 'constants/index';
import { useApiProjects } from 'hooks/pims-api/useApiProjects';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, waitFor } from 'utils/test-utils';

import { IProjectFilter } from '..';
import { ProjectListView } from './ProjectListView';
import { ProjectSearchResultModel } from './ProjectSearchResults/models';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: [] },
};

jest.mock('@react-keycloak/web');
jest.mock('hooks/pims-api/useApiProjects');
const searchProjects = jest.fn();
(useApiProjects as jest.Mock).mockReturnValue({
  searchProjects,
});

const setupMockSearch = (searchResults?: ProjectSearchResultModel[]) => {
  const results = searchResults ?? [];
  const len = results.length;
  searchProjects.mockResolvedValue({
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

describe('Project List View', () => {
  beforeEach(() => {
    searchProjects.mockClear();
  });

  it('matches snapshot', async () => {
    setupMockSearch();
    const { asFragment } = setup();

    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('searches by Project Name', async () => {
    setupMockSearch([
      {
        id: 1,
        description: 'PROJECT-NAME',
        code: '9999',
        region: 'NORTH',
        status: 'ACTIVE',
        lastUpdatedBy: 'USER',
        lastUpdatedDate: new Date(2023, 1, 1),
      },
    ]);
    const { container, searchButton, findByText } = setup();

    fillInput(container, 'projectName', 'NAME');
    await act(async () => userEvent.click(searchButton));

    expect(searchProjects).toHaveBeenCalledWith(
      expect.objectContaining<IProjectFilter>({
        projectName: 'NAME',
        projectNumber: '',
        projectStatusCode: 'AC',
        projectRegionCode: '',
      }),
    );

    expect(await findByText(/PROJECT-NAME/i)).toBeInTheDocument();
  });
});

import { Claims } from '@/constants/claims';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_Project } from '@/models/api/Project';
import { render, RenderOptions } from '@/utils/test-utils';

import {
  AcquisitionSearchResults,
  IAcquisitionSearchResultsProps,
} from './AcquisitionSearchResults';
import { AcquisitionSearchResultModel } from './models';

const setSort = jest.fn();
jest.mock('@react-keycloak/web');

// render component under test
const setup = (
  renderOptions: RenderOptions & Partial<IAcquisitionSearchResultsProps> = { results: [] },
) => {
  const { results, ...rest } = renderOptions;
  const utils = render(<AcquisitionSearchResults results={results ?? []} setSort={setSort} />, {
    claims: [Claims.PROJECT_VIEW, Claims.ACQUISITION_VIEW],
    ...rest,
  });
  const tableRows = utils.container.querySelectorAll('.table .tbody .tr-wrapper');
  return {
    ...utils,
    tableRows,
  };
};

const mockResults: Api_AcquisitionFile[] = [];

describe('Acquisition Search Results Table', () => {
  beforeEach(() => {
    setSort.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({
      results: mockResults.map(a => AcquisitionSearchResultModel.fromApi(a)),
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows, findAllByText } = setup({ results: [] });

    const toasts = await findAllByText(
      'No matching results can be found. Try widening your search criteria.',
    );

    expect(tableRows.length).toBe(0);
    expect(toasts[0]).toBeVisible();
  });

  it('displays historical file #', async () => {
    const { getByText, findAllByText } = setup({ results: [] });

    await findAllByText('No matching results can be found. Try widening your search criteria.');
    expect(getByText('Historical file #')).toBeVisible();
  });

  it('displays alternate project', async () => {
    const { findByText } = setup({
      results: [
        {
          compensationRequisitions: [
            {
              alternateProject: { description: 'alternate project', code: '1234' } as Api_Project,
            } as Api_CompensationRequisition,
          ],
        },
      ],
    });
    const text = await findByText('Alt Project: 1234 alternate project');

    expect(text).toBeVisible();
  });

  it('displays project before alternate project', async () => {
    const { findByText, queryByText } = setup({
      results: [
        {
          project: { description: 'project', code: '4321' } as Api_Project,
          compensationRequisitions: [
            {
              alternateProject: { description: 'alternate project', code: '1234' } as Api_Project,
            } as Api_CompensationRequisition,
          ],
        },
      ],
    });

    const text = await findByText('4321 project');
    expect(queryByText('[+1 more...]')).toBeVisible();
    expect(text).toBeVisible();
  });

  it('displays multiple alternate projects from different compensations', async () => {
    const { findByText, queryByText } = setup({
      results: [
        {
          compensationRequisitions: [
            {
              alternateProject: { description: 'alternate project 1', code: '1' } as Api_Project,
            } as Api_CompensationRequisition,
            {
              alternateProject: { description: 'alternate project 2', code: '2' } as Api_Project,
            } as Api_CompensationRequisition,
          ],
        },
      ],
    });
    const text = await findByText('Alt Project: 1 alternate project 1;');
    expect(text).toBeVisible();
    expect(queryByText('[+1 more...]')).not.toBeNull();
  });

  it('displays a duplicate alternate project only once', async () => {
    const { getByText, queryByText } = setup({
      results: [
        {
          compensationRequisitions: [
            {
              alternateProject: { description: 'alternate project', code: '1' } as Api_Project,
            } as Api_CompensationRequisition,
            {
              alternateProject: { description: 'alternate project', code: '1' } as Api_Project,
            } as Api_CompensationRequisition,
          ],
        },
      ],
    });

    expect(getByText('Alt Project: 1 alternate project')).toBeVisible();
    expect(queryByText('[+1 more...]')).toBeNull();
  });
});

import { Claims } from '@/constants/claims';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
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

const mockResults: ApiGen_Concepts_AcquisitionFile[] = [];

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
              alternateProject: {
                description: 'alternate project',
                code: '1234',
              } as ApiGen_Concepts_Project,
            } as ApiGen_Concepts_CompensationRequisition,
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
          project: { description: 'project', code: '4321' } as ApiGen_Concepts_Project,
          compensationRequisitions: [
            {
              alternateProject: {
                description: 'alternate project',
                code: '1234',
              } as ApiGen_Concepts_Project,
            } as ApiGen_Concepts_CompensationRequisition,
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
              alternateProject: {
                description: 'alternate project 1',
                code: '1',
              } as ApiGen_Concepts_Project,
            } as ApiGen_Concepts_CompensationRequisition,
            {
              alternateProject: {
                description: 'alternate project 2',
                code: '2',
              } as ApiGen_Concepts_Project,
            } as ApiGen_Concepts_CompensationRequisition,
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
              alternateProject: {
                description: 'alternate project',
                code: '1',
              } as ApiGen_Concepts_Project,
            } as ApiGen_Concepts_CompensationRequisition,
            {
              alternateProject: {
                description: 'alternate project',
                code: '1',
              } as ApiGen_Concepts_Project,
            } as ApiGen_Concepts_CompensationRequisition,
          ],
        },
      ],
    });

    expect(getByText('Alt Project: 1 alternate project')).toBeVisible();
    expect(queryByText('[+1 more...]')).toBeNull();
  });

  it('displays a team member as Organization', async () => {
    const { getByText } = setup({
      results: [
        {
          acquisitionTeam: [
            {
              id: 4,
              acquisitionFileId: 5,
              organizationId: 6,
              organization: {
                id: 6,
                isDisabled: false,
                name: 'FORTIS BC',
                alias: 'FORTIS',
                incorporationNumber: '123456789',
                organizationPersons: [],
                organizationAddresses: [],
                contactMethods: [],
                comment: '',
                rowVersion: 1,
              },
              primaryContactId: 8,
              teamProfileTypeCode: 'PROPANLYS',
              teamProfileType: {
                id: 'PROPANLYS',
                description: 'Property analyst',
                isDisabled: false,
                displayOrder: null,
              },
              rowVersion: 1,
              person: null,
              personId: null,
              primaryContact: null,
            },
          ],
        },
      ],
    });

    expect(getByText('FORTIS BC (Property analyst)')).toBeVisible();
  });
});

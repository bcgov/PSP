import { Claims } from '@/constants/claims';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiProperty } from '@/mocks/properties.mock';
import { Api_DispositionFile } from '@/models/api/DispositionFile';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, screen } from '@/utils/test-utils';

import { DispositionSearchResultModel } from '../models';
import {
  DispositionSearchResults,
  IDispositionSearchResultsProps,
} from './DispositionSearchResults';

jest.mock('@react-keycloak/web');

const setSort = jest.fn();
const mockResults: Api_DispositionFile[] = [mockDispositionFileResponse(1, 'test disposition')];

describe('Disposition search results table', () => {
  const getTableRows = () => document.querySelectorAll('.table .tbody .tr-wrapper');

  const setup = (
    renderOptions: RenderOptions & Partial<IDispositionSearchResultsProps> = { results: [] },
  ) => {
    const { results, ...rest } = renderOptions;
    const utils = render(<DispositionSearchResults results={results ?? []} setSort={setSort} />, {
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
      claims: [Claims.DISPOSITION_VIEW],
      ...rest,
    });

    return { ...utils };
  };

  beforeEach(() => {
    setSort.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({
      results: mockResults.map(a => DispositionSearchResultModel.fromApi(a)),
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    setup({ results: [] });
    const message = await screen.findByText(
      'No matching results can be found. Try widening your search criteria.',
    );
    expect(message).toBeVisible();
    expect(getTableRows().length).toBe(0);
  });

  it('displays disposition file name', async () => {
    setup({ results: mockResults.map(a => DispositionSearchResultModel.fromApi(a)) });
    const text = await screen.findByText(mockResults[0].fileName!);
    expect(text).toBeVisible();
  });

  it('displays disposition file number', async () => {
    setup({ results: mockResults.map(a => DispositionSearchResultModel.fromApi(a)) });
    const text = await screen.findByText(`D-${mockResults[0].fileNumber!}`);
    expect(text).toBeVisible();
  });

  it('displays disposition legacy file reference', async () => {
    setup({ results: mockResults.map(a => DispositionSearchResultModel.fromApi(a)) });
    const text = await screen.findByText(mockResults[0].fileReference!);
    expect(text).toBeVisible();
  });

  it('displays multiple file properties', () => {
    setup({
      results: [
        DispositionSearchResultModel.fromApi({
          ...mockDispositionFileResponse(),
          fileProperties: [
            {
              id: 100,
              fileId: 1,
              propertyId: 1,
              property: { ...getMockApiProperty(), id: 1 },
            },
            {
              id: 200,
              fileId: 1,
              propertyId: 2,
              property: { ...getMockApiProperty(), id: 2 },
            },
            {
              id: 300,
              fileId: 1,
              propertyId: 3,
              property: { ...getMockApiProperty(), id: 3 },
            },
          ],
        }),
      ],
    });

    const addresses = screen.getAllByText('1234 mock Street', { exact: false });
    expect(addresses).toHaveLength(2);
    expect(screen.getByText('[+1 more...]')).toBeVisible();
  });

  it('displays a team member organization', () => {
    setup({
      results: [
        DispositionSearchResultModel.fromApi({
          ...mockDispositionFileResponse(),
          dispositionTeam: [
            {
              id: 1,
              dispositionFileId: 1,
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
              teamProfileType: {
                id: 'MAJORPRJ',
                description: 'Major Projects',
                isDisabled: false,
              },
            },
          ],
        }),
      ],
    });

    expect(screen.getByText('FORTIS BC (Major Projects)')).toBeVisible();
  });

  it('displays a team member person', () => {
    setup({
      results: [
        DispositionSearchResultModel.fromApi({
          ...mockDispositionFileResponse(),
          dispositionTeam: [
            {
              id: 1,
              dispositionFileId: 1,
              personId: 6,
              person: {
                id: 6,
                isDisabled: false,
                rowVersion: 1,
                firstName: 'chester',
                surname: 'tester',
              },
              teamProfileType: {
                id: 'MoTIReg',
                description: 'MoTI Region',
                isDisabled: false,
              },
            },
          ],
        }),
      ],
    });

    expect(screen.getByText('chester tester (MoTI Region)')).toBeVisible();
  });

  it('displays multiple team members', () => {
    setup({
      results: [
        DispositionSearchResultModel.fromApi({
          ...mockDispositionFileResponse(),
          dispositionTeam: [
            {
              id: 1,
              dispositionFileId: 1,
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
              teamProfileType: {
                id: 'MAJORPRJ',
                description: 'Major Projects',
                isDisabled: false,
              },
            },
            {
              id: 2,
              dispositionFileId: 1,
              personId: 6,
              person: {
                id: 6,
                isDisabled: false,
                rowVersion: 1,
                firstName: 'chester',
                surname: 'tester',
              },
              teamProfileType: {
                id: 'MoTIReg',
                description: 'MoTI Region',
                isDisabled: false,
              },
            },
            {
              id: 3,
              dispositionFileId: 1,
              personId: 7,
              person: {
                id: 7,
                isDisabled: false,
                rowVersion: 1,
                firstName: 'john',
                surname: 'doe',
              },
              teamProfileType: {
                id: 'CAPPROG',
                description: 'Capital Program',
                isDisabled: false,
              },
            },
          ],
        }),
      ],
    });

    expect(screen.getByText('chester tester (MoTI Region),')).toBeVisible();
    expect(screen.getByText('john doe (Capital Program),')).toBeVisible();
    expect(screen.getByText('[+1 more...]')).toBeVisible();
  });
});

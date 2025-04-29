import { Claims } from '@/constants/claims';
import { getEmptyPerson } from '@/mocks/contacts.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { getMockApiProperty } from '@/mocks/properties.mock';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, screen } from '@/utils/test-utils';
import { ManagementSearchResultModel } from '../models';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';
import { IManagementSearchResultsProps, ManagementSearchResults } from './ManagementSearchResults';

const setSort = vi.fn();
const mockResults: ApiGen_Concepts_ManagementFile[] = [
  mockManagementFileResponse(1, 'test management'),
];

describe('Management search results table', () => {
  const getTableRows = () => document.querySelectorAll('.table .tbody .tr-wrapper');

  const setup = (
    renderOptions: RenderOptions & Partial<IManagementSearchResultsProps> = { results: [] },
  ) => {
    const { results, ...rest } = renderOptions;
    const utils = render(<ManagementSearchResults results={results ?? []} setSort={setSort} />, {
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
      results: mockResults.map(a => ManagementSearchResultModel.fromApi(a)),
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

  it('displays management file name', async () => {
    setup({ results: mockResults.map(a => ManagementSearchResultModel.fromApi(a)) });
    const text = await screen.findByText(mockResults[0].fileName);
    expect(text).toBeVisible();
  });

  it('displays management file number', async () => {
    setup({ results: mockResults.map(a => ManagementSearchResultModel.fromApi(a)) });
    const text = await screen.findByText(`${mockResults[0].id}`);
    expect(text).toBeVisible();
  });

  it('displays management legacy file reference', async () => {
    setup({ results: mockResults.map(a => ManagementSearchResultModel.fromApi(a)) });
    const text = await screen.findByText(mockResults[0].legacyFileNum);
    expect(text).toBeVisible();
  });

  it('displays multiple file properties', () => {
    setup({
      results: [
        ManagementSearchResultModel.fromApi({
          ...mockManagementFileResponse(),
          fileProperties: [
            {
              id: 100,
              fileId: 1,
              propertyId: 1,
              property: { ...getMockApiProperty(), id: 1 },
              displayOrder: null,
              file: null,
              propertyName: null,
              location: null,
              rowVersion: null,
            },
            {
              id: 200,
              fileId: 1,
              propertyId: 2,
              property: { ...getMockApiProperty(), id: 2 },
              displayOrder: null,
              file: null,
              propertyName: null,
              location: null,
              rowVersion: null,
            },
            {
              id: 300,
              fileId: 1,
              propertyId: 3,
              property: { ...getMockApiProperty(), id: 3 },
              displayOrder: null,
              file: null,
              propertyName: null,
              location: null,
              rowVersion: null,
            },
          ],
        }),
      ],
    });

    const addresses = screen.getAllByText('1234 mock Street', { exact: false });
    expect(addresses).toHaveLength(2);
    expect(screen.getAllByText('[+1 more...]')).toHaveLength(1);
  });

  it('displays a team member organization', () => {
    setup({
      results: [
        ManagementSearchResultModel.fromApi({
          ...mockManagementFileResponse(),
          managementTeam: [
            {
              id: 1,
              managementFileId: 1,
              organizationId: 6,
              organization: {
                ...getEmptyOrganization(),
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
                displayOrder: null,
              },
              primaryContact: null,
              primaryContactId: null,
              rowVersion: null,
              teamProfileTypeCode: null,
              person: null,
              personId: null,
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
        ManagementSearchResultModel.fromApi({
          ...mockManagementFileResponse(),
          managementTeam: [
            {
              id: 1,
              managementFileId: 1,
              personId: 6,
              person: {
                ...getEmptyPerson(),
                id: 6,
                isDisabled: false,
                rowVersion: 1,
                firstName: 'chester',
                surname: 'tester',
                comment: null,
                contactMethods: null,
                middleNames: null,
                personAddresses: null,
                personOrganizations: null,
                preferredName: null,
              },
              teamProfileType: {
                id: 'MoTIReg',
                description: 'MoTI Region',
                isDisabled: false,
                displayOrder: null,
              },
              primaryContact: null,
              primaryContactId: null,
              rowVersion: null,
              teamProfileTypeCode: null,
              organization: null,
              organizationId: null,
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
        ManagementSearchResultModel.fromApi({
          ...mockManagementFileResponse(),
          managementTeam: [
            {
              id: 1,
              managementFileId: 1,
              organizationId: 6,
              organization: {
                ...getEmptyOrganization(),
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
                displayOrder: null,
              },
              person: null,
              personId: null,
              primaryContact: null,
              primaryContactId: null,
              rowVersion: null,
              teamProfileTypeCode: null,
            },
            {
              id: 2,
              managementFileId: 1,
              personId: 6,
              person: {
                ...getEmptyPerson(),
                id: 6,
                isDisabled: false,
                rowVersion: 1,
                firstName: 'chester',
                surname: 'tester',
                comment: null,
                contactMethods: null,
                middleNames: null,
                personAddresses: null,
                personOrganizations: null,
                preferredName: null,
              },
              teamProfileType: {
                id: 'MoTIReg',
                description: 'MoTI Region',
                isDisabled: false,
                displayOrder: null,
              },
              primaryContact: null,
              primaryContactId: null,
              rowVersion: null,
              teamProfileTypeCode: null,
              organization: null,
              organizationId: null,
            },
            {
              id: 3,
              managementFileId: 1,
              personId: 7,
              person: {
                ...getEmptyPerson(),
                id: 7,
                isDisabled: false,
                rowVersion: 1,
                firstName: 'john',
                surname: 'doe',
                comment: null,
                contactMethods: null,
                middleNames: null,
                personAddresses: null,
                personOrganizations: null,
                preferredName: null,
              },
              teamProfileType: {
                id: 'CAPPROG',
                description: 'Capital Program',
                isDisabled: false,
                displayOrder: null,
              },
              primaryContact: null,
              primaryContactId: null,
              rowVersion: null,
              teamProfileTypeCode: null,
              organization: null,
              organizationId: null,
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

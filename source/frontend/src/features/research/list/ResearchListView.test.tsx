import userEvent from '@testing-library/user-event';

import { Claims } from '@/constants/index';
import { useApiResearchFile } from '@/hooks/pims-api/useApiResearchFile';
import { IResearchSearchResult } from '@/interfaces/IResearchSearchResult';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiProperty } from '@/mocks/properties.mock';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fillInput,
  render,
  RenderOptions,
  waitFor,
  waitForElementToBeRemoved,
} from '@/utils/test-utils';

import { ResearchListView } from './ResearchListView';
import { useKeycloak } from '@react-keycloak/web';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

vi.mock('@/hooks/pims-api/useApiResearchFile');
const getResearchFiles = vi.fn();
vi.mocked(useApiResearchFile).mockReturnValue({
  getResearchFiles,
} as unknown as ReturnType<typeof useApiResearchFile>);

// render component under test
const setup = (renderOptions: RenderOptions = { store: storeState }) => {
  const utils = render(<ResearchListView />, { ...renderOptions, claims: [Claims.LEASE_VIEW] });
  const searchButton = utils.getByTestId('search');
  return { searchButton, ...utils };
};

const setupMockSearch = (searchResults?: IResearchSearchResult[]) => {
  const results = searchResults ?? mockResearchListViewResponse;
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

describe('Research List View', () => {
  beforeEach(() => {
    getResearchFiles.mockClear();
  });

  it('matches snapshot', async () => {
    setupMockSearch();
    const { asFragment, getByTitle } = setup();

    const fragment = await waitFor(() => asFragment());
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    expect(fragment).toMatchSnapshot();
  });

  it('searches by region', async () => {
    setupMockSearch([]);
    const { container, searchButton, findByText, getByTitle } = setup();
    await waitForElementToBeRemoved(getByTitle('table-loading'));

    fillInput(container, 'regionCode', 'South Coast Region', 'select');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith({
      appCreateUserid: '',
      appLastUpdateUserid: '',
      createOrUpdateBy: 'appLastUpdateUserid',
      createOrUpdateRange: 'updatedOnStartDate',
      createdOnEndDate: '',
      createdOnStartDate: '',
      name: '',
      page: 1,
      quantity: 10,
      regionCode: '',
      researchFileStatusTypeCode: 'ACTIVE',
      researchSearchBy: 'name',
      rfileNumber: '',
      roadOrAlias: '',
      sort: undefined,
      updatedOnEndDate: '',
      updatedOnStartDate: '',
    });

    expect(await findByText(/South Coast Region/i)).toBeInTheDocument();
  });

  it('regions are not duplicated', async () => {
    setupMockSearch([
      {
        id: 1,
        fileStatusTypeCode: {
          id: 'ACTIVE',
          description: 'Active',
          isDisabled: false,
          displayOrder: null,
        },
        fileName: 'name',
        fileNumber: 'R100-100-100',
        appLastUpdateUserid: '',
        appCreateUserid: '',
        appCreateTimestamp: '2020-01-01',
        appLastUpdateTimestamp: '2021-01-01',
        fileProperties: [
          {
            id: 1,
            isDisabled: false,
            property: {
              id: 1,
              region: {
                id: 1,
                description: 'Southern Interior Region',
                isDisabled: false,
              },
              dataSourceEffectiveDate: '2021-08-31T00:00:00',
              isSensitive: false,
              pid: '007-723-385',
              pin: 90069930,
              landArea: 1,
              rowVersion: 2,
            },
            rowVersion: 0,
          } as any,
          {
            id: 2,
            isDisabled: false,
            property: {
              id: 2,
              region: {
                id: 1,
                description: 'Southern Interior Region',
                isDisabled: false,
              },
              dataSourceEffectiveDate: '2021-08-31T00:00:00',
              isSensitive: false,
              pid: '011-041-404',
              pin: 90072652,
              landArea: 1,
              rowVersion: 2,
            } as any,
            rowVersion: 0,
          },
        ],
      },
    ]);
    const { container, searchButton, findAllByText } = setup();

    fillInput(container, 'regionCode', 'South Coast Region', 'select');
    await act(async () => userEvent.click(searchButton));

    expect(await findAllByText(/Southern Interior Region/i)).toHaveLength(2);
  });

  it('all unique regions are listed', async () => {
    setupMockSearch([
      {
        id: 1,
        fileStatusTypeCode: {
          id: 'ACTIVE',
          description: 'Active',
          isDisabled: false,
          displayOrder: null,
        },
        fileName: 'name',
        fileNumber: 'R100-100-100',
        appLastUpdateUserid: '',
        appCreateUserid: '',
        appCreateTimestamp: '2020-01-01',
        appLastUpdateTimestamp: '2021-01-01',
        fileProperties: [
          {
            id: 1,
            isDisabled: false,
            property: {
              id: 1,
              region: {
                id: 1,
                description: 'Southern Interior Region',
                isDisabled: false,
              },
              dataSourceEffectiveDate: '2021-08-31T00:00:00',
              isSensitive: false,
              pid: '007-723-385',
              pin: 90069930,
              landArea: 1,
              rowVersion: 2,
            },
            rowVersion: 0,
          } as any,
          {
            id: 2,
            isDisabled: false,
            property: {
              id: 2,
              region: {
                id: 2,
                description: 'South Coast Region',
                isDisabled: false,
              },
              dataSourceEffectiveDate: '2021-08-31T00:00:00',
              isSensitive: false,
              pid: '011-041-404',
              pin: 90072652,
              landArea: 1,
              rowVersion: 2,
            } as any,
            rowVersion: 0,
          },
        ],
      },
    ]);
    const { container, searchButton, findByText } = setup();

    fillInput(container, 'regionCode', 'South Coast Region', 'select');
    await act(async () => userEvent.click(searchButton));

    expect(await findByText(/Southern Interior Region, South Coast Region/i)).toBeInTheDocument();
  });

  it('searches by R-file number', async () => {
    setupMockSearch([]);
    const { container, searchButton } = setup();
    fillInput(container, 'researchSearchBy', 'rFileNumber', 'select');
    fillInput(container, 'rFileNumber', '101');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith({
      appCreateUserid: '',
      appLastUpdateUserid: '',
      createOrUpdateBy: 'appLastUpdateUserid',
      createOrUpdateRange: 'updatedOnStartDate',
      createdOnEndDate: '',
      createdOnStartDate: '',
      name: '',
      page: 1,
      quantity: 10,
      regionCode: '',
      researchFileStatusTypeCode: 'ACTIVE',
      researchSearchBy: '',
      rfileNumber: '',
      roadOrAlias: '',
      sort: undefined,
      updatedOnEndDate: '',
      updatedOnStartDate: '',
    });
  });

  it('searches by file name', async () => {
    setupMockSearch([]);
    const { container, searchButton } = setup();
    fillInput(container, 'researchSearchBy', 'name', 'select');
    fillInput(container, 'name', 'test file name 1');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith({
      appCreateUserid: '',
      appLastUpdateUserid: '',
      createOrUpdateBy: 'appLastUpdateUserid',
      createOrUpdateRange: 'updatedOnStartDate',
      createdOnEndDate: '',
      createdOnStartDate: '',
      name: 'test file name 1',
      page: 1,
      quantity: 10,
      regionCode: '',
      researchFileStatusTypeCode: 'ACTIVE',
      researchSearchBy: 'name',
      rfileNumber: '',
      roadOrAlias: '',
      sort: undefined,
      updatedOnEndDate: '',
      updatedOnStartDate: '',
    });
  });

  it('searches by research file status', async () => {
    setupMockSearch([]);
    const { container, searchButton } = setup();

    fillInput(container, 'researchFileStatusTypeCode', 'INACTIVE', 'select');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith({
      appCreateUserid: '',
      appLastUpdateUserid: '',
      createOrUpdateBy: 'appLastUpdateUserid',
      createOrUpdateRange: 'updatedOnStartDate',
      createdOnEndDate: '',
      createdOnStartDate: '',
      name: '',
      page: 1,
      quantity: 10,
      regionCode: '',
      researchFileStatusTypeCode: 'INACTIVE',
      researchSearchBy: 'name',
      rfileNumber: '',
      roadOrAlias: '',
      sort: undefined,
      updatedOnEndDate: '',
      updatedOnStartDate: '',
    });
  });

  it('searches by road name', async () => {
    setupMockSearch([]);
    const { container, searchButton } = setup();

    fillInput(container, 'roadOrAlias', 'a road name');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith({
      appCreateUserid: '',
      appLastUpdateUserid: '',
      createOrUpdateBy: 'appLastUpdateUserid',
      createOrUpdateRange: 'updatedOnStartDate',
      createdOnEndDate: '',
      createdOnStartDate: '',
      name: '',
      page: 1,
      quantity: 10,
      regionCode: '',
      researchFileStatusTypeCode: 'ACTIVE',
      researchSearchBy: 'name',
      rfileNumber: '',
      roadOrAlias: 'a road name',
      sort: undefined,
      updatedOnEndDate: '',
      updatedOnStartDate: '',
    });
  });

  it('searches by create date range', async () => {
    setupMockSearch([]);
    const { container, searchButton } = setup();

    fillInput(container, 'createOrUpdateRange', 'createdOnStartDate', 'select');
    fillInput(container, 'createdOnStartDate', '2020-01-01');
    fillInput(container, 'createdOnEndDate', '2020-02-02');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith(
      expect.objectContaining({
        appCreateUserid: '',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'createdOnStartDate',
        createdOnEndDate: '2020-02-02',
        createdOnStartDate: '2020-01-01',
        name: '',
        page: 1,
        quantity: 10,
        regionCode: '',
        researchFileStatusTypeCode: 'ACTIVE',
        researchSearchBy: 'name',
        rfileNumber: '',
        roadOrAlias: '',
        sort: undefined,
        updatedOnEndDate: '',
        updatedOnStartDate: '',
      }),
    );
  });

  it('searches by update date range', async () => {
    setupMockSearch([]);
    const { container, searchButton } = setup();

    fillInput(container, 'createOrUpdateRange', 'updatedOnStartDate', 'select');
    fillInput(container, 'updatedOnStartDate', '2021-01-01');
    fillInput(container, 'updatedOnEndDate', '2021-02-02');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith(
      expect.objectContaining({
        appCreateUserid: '',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        page: 1,
        quantity: 10,
        regionCode: '',
        researchFileStatusTypeCode: 'ACTIVE',
        researchSearchBy: 'name',
        rfileNumber: '',
        roadOrAlias: '',
        sort: undefined,
        updatedOnEndDate: '2021-02-02',
        updatedOnStartDate: '2021-01-01',
      }),
    );
  });

  it('searches by create user', async () => {
    setupMockSearch([]);
    const { container, searchButton } = setup();

    fillInput(container, 'createOrUpdateBy', 'appCreateUserid', 'select');
    fillInput(container, 'appCreateUserid', 'createUser');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith(
      expect.objectContaining({
        appCreateUserid: 'createUser',
        appLastUpdateUserid: '',
        createOrUpdateBy: 'appCreateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        page: 1,
        quantity: 10,
        regionCode: '',
        researchFileStatusTypeCode: 'ACTIVE',
        researchSearchBy: 'name',
        rfileNumber: '',
        roadOrAlias: '',
        sort: undefined,
        updatedOnEndDate: '',
        updatedOnStartDate: '',
      }),
    );
  });

  it('searches by update user', async () => {
    setupMockSearch([]);
    const { container, searchButton } = setup();

    fillInput(container, 'createOrUpdateBy', 'appLastUpdateUserid', 'select');
    fillInput(container, 'appLastUpdateUserid', 'updateUser');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith(
      expect.objectContaining({
        appCreateUserid: '',
        appLastUpdateUserid: 'updateUser',
        createOrUpdateBy: 'appLastUpdateUserid',
        createOrUpdateRange: 'updatedOnStartDate',
        createdOnEndDate: '',
        createdOnStartDate: '',
        name: '',
        page: 1,
        quantity: 10,
        regionCode: '',
        researchFileStatusTypeCode: 'ACTIVE',
        researchSearchBy: 'name',
        rfileNumber: '',
        roadOrAlias: '',
        sort: undefined,
        updatedOnEndDate: '',
        updatedOnStartDate: '',
      }),
    );
  });

  it('displays an error when no matching records found', async () => {
    setupMockSearch([]);
    const { container, searchButton, findByText } = setup();

    fillInput(container, 'createOrUpdateBy', 'appLastUpdateUserid', 'select');
    fillInput(container, 'appLastUpdateUserid', 'no match');
    await act(async () => userEvent.click(searchButton));

    const text = await findByText('No matching Research Files found');
    expect(text).toBeVisible();
  });

  it('displays an error when when Search API is unreachable', async () => {
    // simulate a network error
    getResearchFiles.mockRejectedValue(new Error('network error'));
    const { container, searchButton, findAllByText } = setup();

    fillInput(container, 'createOrUpdateBy', 'appLastUpdateUserid', 'select');
    fillInput(container, 'appLastUpdateUserid', 'breaking');
    await act(async () => userEvent.click(searchButton));

    expect(getResearchFiles).toHaveBeenCalledWith({
      appCreateUserid: '',
      appLastUpdateUserid: '',
      createOrUpdateBy: 'appLastUpdateUserid',
      createOrUpdateRange: 'updatedOnStartDate',
      createdOnEndDate: '',
      createdOnStartDate: '',
      name: '',
      page: 1,
      quantity: 10,
      regionCode: '',
      researchFileStatusTypeCode: 'ACTIVE',
      researchSearchBy: 'name',
      rfileNumber: '',
      roadOrAlias: '',
      sort: undefined,
      updatedOnEndDate: '',
      updatedOnStartDate: '',
    });
    const toasts = await findAllByText('network error');
    expect(toasts[0]).toBeVisible();
  });
});

const emptyResearchFile: ApiGen_Concepts_ResearchFile = {
  roadName: null,
  roadAlias: null,
  fileProperties: null,
  requestDate: null,
  requestDescription: null,
  requestSourceDescription: null,
  researchResult: null,
  researchCompletionDate: null,
  isExpropriation: null,
  expropriationNotes: null,
  requestSourceType: null,
  requestorPerson: null,
  requestorOrganization: null,
  researchFilePurposes: null,
  researchFileProjects: null,
  id: 0,
  fileName: null,
  fileNumber: null,
  fileStatusTypeCode: null,
  ...getEmptyBaseAudit(),
};

const mockResearchListViewResponse: ApiGen_Concepts_ResearchFile[] = [
  {
    ...emptyResearchFile,
    id: 1,
    fileName: 'name',
    fileNumber: 'R100-100-100',
    fileProperties: [
      {
        id: 1,
        propertyId: 1,
        property: {
          ...getMockApiProperty(),
          id: 1,
          region: {
            id: 2,
            description: 'Southern Interior Region',
            isDisabled: false,
            displayOrder: null,
          },
          dataSourceEffectiveDateOnly: '2021-08-31T00:00:00',
          isSensitive: false,
          pid: 723385,
          pin: 90069930,
          landArea: 1,
          isVolumetricParcel: false,
          volumetricMeasurement: 0,
          rowVersion: 2,
        },
        rowVersion: 0,
        displayOrder: null,
        documentReference: null,
        fileId: 1,
        isLegalOpinionObtained: null,
        isLegalOpinionRequired: null,
        propertyName: null,
        purposeTypes: null,
        researchSummary: null,
        file: null,
      },
      {
        id: 2,
        propertyId: 2,
        property: {
          ...getMockApiProperty(),
          id: 2,
          region: {
            id: 1,
            description: 'South Coast Region',
            isDisabled: false,
            displayOrder: null,
          },
          dataSourceEffectiveDateOnly: '2021-08-31T00:00:00',
          isSensitive: false,
          pid: 11041404,
          pin: 90072652,
          landArea: 1,
          isVolumetricParcel: false,
          volumetricMeasurement: 0,
          rowVersion: 2,
        },
        rowVersion: 0,
        displayOrder: null,
        documentReference: null,
        fileId: 1,
        isLegalOpinionObtained: null,
        isLegalOpinionRequired: null,
        propertyName: null,
        purposeTypes: null,
        researchSummary: null,
        file: null,
      },
    ],
    appCreateTimestamp: '2020-01-01T00:00:00',
    appLastUpdateTimestamp: '2021-01-01T00:00:00',
    appLastUpdateUserid: 'dbo',
    appCreateUserid: 'dbo',
    rowVersion: 1,
  },
  {
    ...emptyResearchFile,
    id: 2,
    fileName: 'name',
    roadName: 'a road name',
    fileNumber: 'R100-100-101',
    fileProperties: [],
    appCreateTimestamp: '2020-02-02T00:00:00',
    appLastUpdateTimestamp: '2021-02-02T00:00:00',
    appLastUpdateUserid: 'dbo',
    appCreateUserid: 'dbo',
    rowVersion: 1,
  },
  {
    ...emptyResearchFile,
    id: 3,
    fileName: 'test file name 1',
    fileNumber: 'R100-100-102',
    fileProperties: [],
    appCreateTimestamp: '2020-03-03T00:00:00',
    appLastUpdateTimestamp: '2020-04-04T00:00:00',
    appLastUpdateUserid: 'dbo',
    appCreateUserid: 'dbo',
    rowVersion: 1,
  },
  {
    ...emptyResearchFile,
    id: 4,
    fileName: 'name',
    fileNumber: 'R100-100-103',
    fileProperties: [],
    appCreateTimestamp: '2020-05-05T00:00:00',
    appLastUpdateTimestamp: '2020-06-06T00:00:00',
    appLastUpdateUserid: 'dbo',
    appCreateUserid: 'createUser',
    rowVersion: 1,
  },
  {
    ...emptyResearchFile,
    id: 5,
    fileName: 'name',
    fileNumber: 'R100-100-104',
    fileProperties: [],
    appCreateTimestamp: '2020-05-05T00:00:00',
    appLastUpdateTimestamp: '2020-06-06T00:00:00',
    appLastUpdateUserid: 'updateUser',
    appCreateUserid: 'a user',
    rowVersion: 1,
  },
];

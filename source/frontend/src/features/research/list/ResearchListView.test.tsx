import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/index';
import { useApiResearchFile } from '@/hooks/pims-api/useApiResearchFile';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiProperty } from '@/mocks/properties.mock';
import { getEmptyResearchFileProperty } from '@/mocks/researchFile.mock';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { getEmptyResearchFile } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fillInput,
  render,
  RenderOptions,
  screen,
  userEvent,
  waitForElementToBeRemoved,
} from '@/utils/test-utils';

import { ResearchListView } from './ResearchListView';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { defaultResearchFilter } from './ResearchFilter/ResearchFilter';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const history = createMemoryHistory();

vi.mock('@/hooks/pims-api/useApiResearchFile');
const getResearchFiles = vi.fn();
vi.mocked(useApiResearchFile, { partial: true }).mockReturnValue({
  getResearchFiles,
});

vi.mock('@/hooks/repositories/useUserInfoRepository');
const retrieveUserLookup = vi.fn();
vi.mocked(useUserInfoRepository, { partial: true }).mockReturnValue({
  retrieveUserLookup,
});

// render component under test
const setup = (
  renderOptions: RenderOptions = { store: storeState },
  filter = defaultResearchFilter,
  createdByOptions = [],
  ) => {
  const utils = render(<ResearchListView />, {
    ...renderOptions,
    history,
    claims: renderOptions?.claims ?? [Claims.RESEARCH_VIEW],
  });
  const searchButton = utils.getByTestId('search');
  const userLookup = utils.container.querySelector('#multiselect-selectedUser_input');
  return { searchButton, userLookup, ...utils };
};

const setupMockSearch = (searchResults?: ApiGen_Concepts_ResearchFile[]) => {
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
    retrieveUserLookup.mockResolvedValue({
  data: {
    items: [
      {
        id: 1,
        businessIdentifierValue: 'DSMITH',
        person: {
          firstName: 'Devin',
          surname: 'Smith',
        },
      },
    ],
    page: 1,
    quantity: 1000,
    total: 1,
  },
});
  });

  it('matches snapshot', async () => {
    setupMockSearch();
    const { asFragment, getByTitle } = setup();

    await waitForElementToBeRemoved(getByTitle('table-loading'));
    expect(asFragment()).toMatchSnapshot();
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
      pid: '',
      pin: '',
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
      researchFileStatusTypeCode: '',
      researchSearchBy: 'pid',
      rfileNumber: '',
      roadOrAlias: '',
      sort: undefined,
      updatedOnEndDate: '',
      updatedOnStartDate: '',
      selectedUser: [],
    });
    const toasts = await findAllByText('network error');
    expect(toasts[0]).toBeVisible();
  });

  it(`renders the 'Create a Research File' button when user has permissions`, async () => {
    setupMockSearch([]);
    setup({ claims: [Claims.RESEARCH_VIEW, Claims.RESEARCH_ADD] });

    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    const button = await screen.findByText(/Create a Research File/i);
    expect(button).toBeVisible();
  });

  it(`hides the 'Create a Research File' button when user has no permissions`, async () => {
    setupMockSearch([]);
    setup({ claims: [Claims.RESEARCH_VIEW] });

    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    const button = await screen.queryByText(/Create a Research File/i);
    expect(button).toBeNull();
  });

  it('navigates to create research route when user clicks the create button', async () => {
    setupMockSearch([]);
    const selectedUser = [{ id: 'DSMITH', text: 'David Smith (DSMITH)' }];
    setup({ claims: [Claims.RESEARCH_VIEW, Claims.RESEARCH_ADD] }, defaultResearchFilter, selectedUser);

    await waitForElementToBeRemoved(screen.getByTitle('table-loading'));
    const button = await screen.findByText(/Create a Research File/i);
    expect(button).toBeVisible();
    await act(async () => userEvent.click(button));
    expect(history.location.pathname).toBe('/mapview/sidebar/research/new');
  });
});

const mockResearchListViewResponse: ApiGen_Concepts_ResearchFile[] = [
  {
    ...getEmptyResearchFile(),
    id: 1,
    fileName: 'name',
    fileNumber: 'R100-100-100',
    fileProperties: [
      {
        id: 1,
        isActive: null,
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
        propertyResearchPurposeTypes: null,
        researchSummary: null,
        location: null,
        boundary: null,
        file: null,
      },
      {
        id: 2,
        isActive: null,
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
        propertyResearchPurposeTypes: null,
        researchSummary: null,
        location: null,
        boundary: null,
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
    ...getEmptyResearchFile(),
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
    ...getEmptyResearchFile(),
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
    ...getEmptyResearchFile(),
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
    ...getEmptyResearchFile(),
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

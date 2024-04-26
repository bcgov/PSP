import { getEmptyOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { getEmptyResearchFile } from '@/models/defaultInitializers';
import { prettyFormatUTCDate } from '@/utils';
import { render, RenderOptions } from '@/utils/test-utils';

import ResearchHeader, { IResearchHeaderProps } from './ResearchHeader';
import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import { vi } from 'vitest';

const testResearchFile: ApiGen_Concepts_ResearchFile = {
  ...getEmptyResearchFile(),
  id: 5,
  fileName: 'New research file',
  roadName: 'Test road name',
  roadAlias: 'Test road alias',
  fileNumber: 'RFile-0000000018',
  fileStatusTypeCode: {
    id: 'ACTIVE',
    description: 'Active',
    isDisabled: false,
    displayOrder: null,
  },
  fileProperties: [],
  requestDate: '2022-04-14T00:00:00',
  requestDescription: 'a request description',
  researchResult: 'A research result',
  researchCompletionDate: '2022-03-30T00:00:00',
  isExpropriation: false,
  expropriationNotes: 'An expropriation note',
  requestSourceType: {
    id: 'HQ',
    description: 'Headquarters (HQ)',
    isDisabled: false,
    displayOrder: null,
  },
  requestorOrganization: {
    ...getEmptyOrganization(),
    id: 3,
    isDisabled: false,
    name: 'Dairy Queen Forever! Property Management',
    organizationPersons: [],
    organizationAddresses: [],
    contactMethods: [],
    rowVersion: 1,
    alias: null,
    comment: null,
    incorporationNumber: null,
  },
  researchFilePurposes: [],
  appCreateTimestamp: '2022-04-22T19:36:45.65',
  appLastUpdateTimestamp: '2022-04-25T21:03:02.347',
  appLastUpdateUserid: 'admin',
  appCreateUserid: 'admin',
  rowVersion: 9,
};

vi.mock('@/hooks/pims-api/useApiUsers');

vi.mocked(useApiUsers).mockReturnValue({
  getUserInfo: vi.fn().mockResolvedValue({}),
} as any);

describe('ResearchHeader component', () => {
  const setup = (renderOptions: RenderOptions & IResearchHeaderProps) => {
    // render component under test
    const component = render(
      <ResearchHeader
        researchFile={renderOptions.researchFile}
        lastUpdatedBy={renderOptions.lastUpdatedBy}
      />,
      {
        ...renderOptions,
      },
    );

    return {
      component,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected when provided no research file', () => {
    const { component } = setup({ lastUpdatedBy: null });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders as expected when provided a list of properties', async () => {
    const {
      component: { getByText },
    } = await setup({
      researchFile: testResearchFile,
      lastUpdatedBy: {
        parentId: testResearchFile.id || 0,
        appLastUpdateTimestamp: testResearchFile.appLastUpdateTimestamp || '',
        appLastUpdateUserGuid: testResearchFile.appLastUpdateUserGuid || '',
        appLastUpdateUserid: testResearchFile.appLastUpdateUserid || '',
      },
    });

    expect(getByText(testResearchFile.fileNumber as string)).toBeVisible();
    expect(getByText(testResearchFile.fileName as string)).toBeVisible();

    expect(getByText(prettyFormatUTCDate(testResearchFile.appCreateTimestamp))).toBeVisible();
    expect(getByText(prettyFormatUTCDate(testResearchFile.appLastUpdateTimestamp))).toBeVisible();
  });

  it('renders as expected when provided a different last-updated information', async () => {
    const lastUpdateTimeStamp = new Date().toISOString();
    const {
      component: { getByText },
    } = await setup({
      researchFile: testResearchFile,
      lastUpdatedBy: {
        parentId: testResearchFile.id || 0,
        appLastUpdateTimestamp: lastUpdateTimeStamp,
        appLastUpdateUserGuid: 'Test GUID',
        appLastUpdateUserid: testResearchFile.appLastUpdateUserid || '',
      },
    });

    expect(getByText(testResearchFile.fileNumber as string)).toBeVisible();
    expect(getByText(testResearchFile.fileName as string)).toBeVisible();

    expect(getByText(prettyFormatUTCDate(testResearchFile.appCreateTimestamp))).toBeVisible();
    expect(getByText(prettyFormatUTCDate(lastUpdateTimeStamp))).toBeVisible();
  });
});

import { Api_ResearchFile } from 'models/api/ResearchFile';
import { prettyFormatDate } from 'utils';
import { render, RenderOptions } from 'utils/test-utils';

import ResearchHeader, { IResearchHeaderProps } from './ResearchHeader';

const testResearchFile: Api_ResearchFile = {
  id: 5,
  fileName: 'New research file',
  roadName: 'Test road name',
  roadAlias: 'Test road alias',
  fileNumber: 'RFile-0000000018',
  fileStatusTypeCode: {
    id: 'ACTIVE',
    description: 'Active',
    isDisabled: false,
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
  },
  requestorOrganization: {
    id: 3,
    isDisabled: false,
    name: 'Dairy Queen Forever! Property Management',
    organizationPersons: [],
    organizationAddresses: [],
    contactMethods: [],
    rowVersion: 1,
  },
  researchFilePurposes: [],
  appCreateTimestamp: '2022-04-22T19:36:45.65',
  appLastUpdateTimestamp: '2022-04-25T21:03:02.347',
  appLastUpdateUserid: 'admin',
  appCreateUserid: 'admin',
  rowVersion: 9,
};

describe('ResearchHeader component', () => {
  const setup = (renderOptions: RenderOptions & IResearchHeaderProps) => {
    // render component under test
    const component = render(<ResearchHeader researchFile={renderOptions.researchFile} />, {
      ...renderOptions,
    });

    return {
      component,
    };
  };

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('renders as expected when provided no research file', () => {
    const { component } = setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders as expected when provided a list of properties', async () => {
    const {
      component: { getByText },
    } = await setup({ researchFile: testResearchFile });

    expect(getByText(testResearchFile.fileNumber as string)).toBeVisible();
    expect(getByText(testResearchFile.fileName as string)).toBeVisible();

    expect(getByText(prettyFormatDate(testResearchFile.appCreateTimestamp))).toBeVisible();
    expect(getByText(prettyFormatDate(testResearchFile.appLastUpdateTimestamp))).toBeVisible();
  });
});

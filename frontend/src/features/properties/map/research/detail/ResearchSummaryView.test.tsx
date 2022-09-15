import Claims from 'constants/claims';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import { render, RenderOptions } from 'utils/test-utils';

import ResearchSummaryView, { IResearchSummaryViewProps } from './ResearchSummaryView';

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
  researchProperties: [],
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

jest.mock('@react-keycloak/web');

const setEditMode = jest.fn();

describe('ResearchSummaryView component', () => {
  const setup = (renderOptions: RenderOptions & IResearchSummaryViewProps) => {
    // render component under test
    const component = render(
      <ResearchSummaryView
        researchFile={renderOptions.researchFile}
        setEditMode={renderOptions.setEditMode}
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
      },
    );

    return {
      component,
    };
  };

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('renders as expected when provided no research file', () => {
    const { component } = setup({ researchFile: testResearchFile, setEditMode });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders the edit button if the user has research edit permissions', () => {
    const {
      component: { getByTitle },
    } = setup({
      researchFile: testResearchFile,
      setEditMode,
      claims: [Claims.RESEARCH_EDIT],
    });
    const editResearchFile = getByTitle('Edit research file');
    expect(editResearchFile).toBeVisible();
  });

  it('does not render the edit button if the user does not have research edit permissions', () => {
    const {
      component: { queryByTitle },
    } = setup({
      researchFile: testResearchFile,
      setEditMode,
      claims: [],
    });
    const editResearchFile = queryByTitle('Edit research file');
    expect(editResearchFile).toBeNull();
  });
});

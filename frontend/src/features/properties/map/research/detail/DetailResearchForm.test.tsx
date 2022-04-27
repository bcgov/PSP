import { Api_ResearchFile } from 'models/api/ResearchFile';
import { render, RenderOptions } from 'utils/test-utils';

import { IDetailResearchFormProps } from './DetailResearchForm';
import DetailResearchForm from './DetailResearchForm';

const testResearchFile: Api_ResearchFile = {
  id: 5,
  name: 'New research file',
  roadName: 'Test road name',
  roadAlias: 'Test road alias',
  rfileNumber: 'RFile-0000000018',
  researchFileStatusTypeCode: {
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

describe('DetailResearchForm component', () => {
  const setup = (renderOptions: RenderOptions & IDetailResearchFormProps) => {
    // render component under test
    const component = render(<DetailResearchForm researchFile={renderOptions.researchFile} />, {
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
    const { component } = setup({ researchFile: testResearchFile });
    expect(component.asFragment()).toMatchSnapshot();
  });
});

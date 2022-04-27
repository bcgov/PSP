import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions } from 'utils/test-utils';

import { UpdateResearchFormModel } from './models';
import UpdateResearchForm from './UpdateResearchForm';

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

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('UpdateResearchForm component', () => {
  const setup = (renderOptions: RenderOptions & { initialValues: UpdateResearchFormModel }) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.initialValues}>
        {formikProps => <UpdateResearchForm formikProps={formikProps} />}
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        history,
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
    var initialValues = UpdateResearchFormModel.fromApi(testResearchFile);
    const { component } = setup({ initialValues });
    expect(component.asFragment()).toMatchSnapshot();
  });
});

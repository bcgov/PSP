import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { mockLookups } from '@/mocks/lookups.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { getEmptyResearchFile } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, screen } from '@/utils/test-utils';
import { fakeText, fillInput } from '@/utils/test-utils';

import { UpdateResearchSummaryFormModel } from './models';
import { UpdateResearchFileYupSchema } from './UpdateResearchFileYupSchema';
import UpdateSummaryForm from './UpdateSummaryForm';

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

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('UpdateResearchForm component', () => {
  const setup = (
    renderOptions: RenderOptions & { initialValues: UpdateResearchSummaryFormModel },
  ) => {
    // render component under test
    const component = render(
      <Formik
        onSubmit={noop}
        initialValues={renderOptions.initialValues}
        validationSchema={UpdateResearchFileYupSchema}
      >
        {formikProps => <UpdateSummaryForm formikProps={formikProps} />}
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return { ...component };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected when provided no research file', () => {
    const initialValues = UpdateResearchSummaryFormModel.fromApi(testResearchFile);
    const { asFragment } = setup({ initialValues });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the status field', () => {
    const initialValues = UpdateResearchSummaryFormModel.fromApi(testResearchFile);
    const { getByTestId } = setup({ initialValues });
    expect(getByTestId('researchFileStatus')).toBeInTheDocument();
  });

  it('renders the file name field', () => {
    const initialValues = UpdateResearchSummaryFormModel.fromApi(testResearchFile);
    const { container } = setup({ initialValues });
    const fileName = container.querySelector(`#input-name`);

    expect(fileName).toBeInTheDocument();
  });

  it('should have the Help with choosing a name text in the component', async () => {
    const initialValues = UpdateResearchSummaryFormModel.fromApi(testResearchFile);
    setup({ initialValues });
    expect(screen.getByText(`Help with choosing a name`)).toBeInTheDocument();
  });

  it('should validate R-File name', async () => {
    const initialValues = UpdateResearchSummaryFormModel.fromApi(testResearchFile);
    const { container, findByText } = setup({ initialValues });

    // name is required
    await fillInput(container, 'name', '');
    expect(await findByText(/Research File name is required/i)).toBeVisible();

    // name cannot exceed 250 characters
    await fillInput(container, 'name', fakeText(300));
    expect(await findByText(/Research File name must be less than 250 characters/i)).toBeVisible();
  });
});

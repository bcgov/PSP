import { Formik } from 'formik';
import { noop } from 'lodash';
import { mockLookups } from 'mocks';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from 'utils/test-utils';

import { AddAcquisitionTeamForm } from './AddAcquisitionTeamForm';
import { AcquisitionForm } from './models';

describe('AcquisitionTeam component', () => {
  // render component under test
  const setup = (props: { initialForm: AcquisitionForm }, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <Formik initialValues={props.initialForm} onSubmit={noop}>
        {formikProps => <AddAcquisitionTeamForm formikProps={formikProps} />}
      </Formik>,
      {
        ...renderOptions,
        store: { [lookupCodesSlice.name]: { lookupCodes: mockLookups } },
      },
    );

    return { ...utils };
  };

  let testForm: AcquisitionForm;

  beforeEach(() => {
    testForm = new AcquisitionForm();
    testForm.fileName = 'Test name';
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ initialForm: testForm });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders add new team member link', async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    expect(getByTestId('add-team-member')).toBeVisible();
  });

  it('renders remove new team member link', async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    const addTeamMember = getByTestId('add-team-member');
    await act(async () => {
      userEvent.click(addTeamMember);
    });

    expect(getByTestId('remove-team-member')).toBeVisible();
  });

  it('renders person profile select', async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    const addTeamMember = getByTestId('add-team-member');
    await act(async () => {
      userEvent.click(addTeamMember);
    });
    expect(getByTestId('select-profile')).toBeVisible();
  });
});

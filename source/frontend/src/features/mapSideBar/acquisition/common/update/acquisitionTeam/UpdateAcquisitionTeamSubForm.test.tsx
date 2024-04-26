import { Formik, FormikProps, getIn } from 'formik';

import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  getByName,
  render,
  RenderOptions,
  selectOptions,
  userEvent,
} from '@/utils/test-utils';

import { WithAcquisitionTeam } from '../../models';
import { UpdateAcquisitionTeamSubForm } from './UpdateAcquisitionTeamSubForm';
import { createRef } from 'react';

describe('AcquisitionTeamSubForm component', () => {
  // render component under test
  const setup = (
    props: { initialForm: WithAcquisitionTeam },
    renderOptions: RenderOptions = {},
  ) => {
    const ref = createRef<FormikProps<WithAcquisitionTeam>>();
    const utils = render(
      <Formik innerRef={ref} initialValues={props.initialForm} onSubmit={vi.fn()}>
        {formikProps => <UpdateAcquisitionTeamSubForm />}
      </Formik>,
      {
        ...renderOptions,
        store: { [lookupCodesSlice.name]: { lookupCodes: mockLookups } },
      },
    );

    return {
      ...utils,
      getFormikRef: () => ref,
    };
  };

  let testForm: WithAcquisitionTeam;

  beforeEach(() => {
    testForm = { team: [] };
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ initialForm: testForm });
    expect(asFragment()).toMatchSnapshot();
  });

  it(`renders 'Add team member' link`, async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    expect(getByTestId('add-team-member')).toBeVisible();
  });

  it(`renders 'Remove team member' link`, async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    const addRow = getByTestId('add-team-member');
    await act(async () => userEvent.click(addRow));
    expect(getByTestId('team.0.remove-button')).toBeVisible();
  });

  it(`renders 'Profile type' dropdown`, async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    const addRow = getByTestId('add-team-member');
    await act(async () => userEvent.click(addRow));
    expect(getByTestId('select-profile')).toBeVisible();
  });

  it(`displays a confirmation popup before team member is removed`, async () => {
    const { getByTestId, getByText } = setup({ initialForm: testForm });
    const addRow = getByTestId('add-team-member');
    await act(async () => userEvent.click(addRow));
    await act(async () => userEvent.click(getByTestId('team.0.remove-button')));

    expect(getByText(/Are you sure you want to remove this row/i)).toBeVisible();
  });

  it(`removes the team member upon user confirmation`, async () => {
    const { getByTestId, getByText, getByTitle } = setup({
      initialForm: testForm,
    });
    const addRow = getByTestId('add-team-member');
    await act(async () => userEvent.click(addRow));
    await act(async () => userEvent.click(getByTestId('team.0.remove-button')));

    expect(getByText(/Are you sure you want to remove this row/i)).toBeVisible();

    await act(async () => userEvent.click(getByTitle('ok-modal')));
    expect(getByName('team.0.contactTypeCode')).toBeNull();
  });

  it(`does not remove the owner when confirmation popup is cancelled`, async () => {
    const { getByTestId, getByText, getByTitle } = setup({
      initialForm: testForm,
    });
    const addRow = getByTestId('add-team-member');
    await act(async () => userEvent.click(addRow));
    await act(async () => userEvent.click(getByTestId('team.0.remove-button')));

    expect(getByText(/Are you sure you want to remove this row/i)).toBeVisible();

    await act(async () => userEvent.click(getByTitle('cancel-modal')));
    expect(getByName('team.0.contactTypeCode')).toBeVisible();
  });

  it(`sets the contact manager field as 'touched' when team profile type is changed`, async () => {
    const { getByTestId, getFormikRef } = setup({
      initialForm: testForm,
    });
    const addRow = getByTestId('add-team-member');
    await act(async () => userEvent.click(addRow));
    await act(async () => selectOptions('team.0.contactTypeCode', 'MOTILAWYER'));
    expect(getIn(getFormikRef().current?.touched, 'team.0.contact')).toBe(true);
  });
});

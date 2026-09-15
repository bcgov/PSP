import { Formik, FormikProps, getIn } from 'formik';
import { RestrictContactType } from '@/constants/contacts';
import { ApiGen_CodeTypes_LeaseTeamProfileTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseTeamProfileTypes';
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
import { createRef } from 'react';
import { WithLeaseTeam } from '../models';
import { AddLeaseTeamSubForm } from './AddLeaseTeamSubform';
import { AddLeaseTeamYupSchema } from './AddLeaseTeamYupSchema';
import { IContactSearchResult } from '@/interfaces';

const contactInputMock = vi.fn();

vi.mock('@/components/common/form/ContactInput/ContactInputContainer', () => ({
  ContactInputContainer: (props: any) => {
    contactInputMock(props);
    return <div data-testid="contact-input-container" />;
  },
}));

describe('AddLeaseTeamSubForm component', () => {
  // render component under test
  const setup = (props: { initialForm: WithLeaseTeam }, renderOptions: RenderOptions = {}) => {
    const ref = createRef<FormikProps<WithLeaseTeam>>();
    const utils = render(
      <Formik
        innerRef={ref}
        initialValues={props.initialForm}
        validationSchema={AddLeaseTeamYupSchema}
        onSubmit={vi.fn()}
      >
        {formikProps => <AddLeaseTeamSubForm />}
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

  const selectedPerson: IContactSearchResult = {
    id: '1',
    summary: 'summary',
    mailingAddress: '123 mock st',
    surname: 'last',
    firstName: 'first',
    email: 'email',
    municipalityName: 'city',
    provinceState: 'province',
    isDisabled: false,
    personId: 1,
    person: null,
    middleNames: null,
    organizationName: null,
  };

  let testForm: WithLeaseTeam;

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

    expect(getByText(/Do you wish to remove this team member/i)).toBeVisible();
  });

  it(`removes the team member upon user confirmation`, async () => {
    const { getByTestId, getByText, getByTitle } = setup({
      initialForm: testForm,
    });
    const addRow = getByTestId('add-team-member');
    await act(async () => userEvent.click(addRow));
    await act(async () => userEvent.click(getByTestId('team.0.remove-button')));

    expect(getByText(/Do you wish to remove this team member/i)).toBeVisible();

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

    expect(getByText(/Do you wish to remove this team member/i)).toBeVisible();

    await act(async () => userEvent.click(getByTitle('cancel-modal')));
    expect(getByName('team.0.contactTypeCode')).toBeVisible();
  });

  it(`sets the contact manager field as 'touched' when team profile type is changed`, async () => {
    const { getByTestId, getFormikRef } = setup({
      initialForm: testForm,
    });
    const addRow = getByTestId('add-team-member');
    await act(async () => userEvent.click(addRow));
    await act(async () => selectOptions('team.0.contactTypeCode', 'MOTTLAWYER'));
    expect(getIn(getFormikRef().current?.touched, 'team.0.contact')).toBe(true);
  });

  it('restricts contact selection to PIMS users for PROPANALYST profile', async () => {
    const { getByTestId } = setup({
      initialForm: testForm,
    });

    await act(async () => userEvent.click(getByTestId('add-team-member')));

    await act(async () =>
      selectOptions('team.0.contactTypeCode', ApiGen_CodeTypes_LeaseTeamProfileTypes.PROPANALYST),
    );

    expect(contactInputMock).toHaveBeenLastCalledWith(
      expect.objectContaining({
        restrictContactType: [RestrictContactType.ONLY_PIMSUSERS],
      }),
    );
  });

  it('allows all contact types for unrestricted team profiles', async () => {
    const { getByTestId } = setup({
      initialForm: testForm,
    });

    await act(async () => userEvent.click(getByTestId('add-team-member')));

    await act(async () =>
      selectOptions('team.0.contactTypeCode', ApiGen_CodeTypes_LeaseTeamProfileTypes.MOTTLAWYER),
    );

    expect(contactInputMock).toHaveBeenLastCalledWith(
      expect.objectContaining({
        restrictContactType: [
          RestrictContactType.ONLY_PIMSUSERS,
          RestrictContactType.ONLY_INDIVIDUALS,
          RestrictContactType.ONLY_ORGANIZATIONS,
        ],
      }),
    );
  });

  it('restricts key contact selection to PIMS users', async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    await act(async () => userEvent.click(getByTestId('add-team-member')));
    await act(async () =>
      selectOptions('team.0.contactTypeCode', ApiGen_CodeTypes_LeaseTeamProfileTypes.KEYCNTCT),
    );

    expect(contactInputMock).toHaveBeenLastCalledWith(
      expect.objectContaining({
        restrictContactType: [RestrictContactType.ONLY_PIMSUSERS],
      }),
    );
  });

  it('displays an error when the same contact and role are selected twice', async () => {
    const { getByTestId, getFormikRef, getByText } = setup({
      initialForm: testForm,
    });

    // First team member
    await act(async () => {
      await userEvent.click(getByTestId('add-team-member'));
    });

    await act(async () => {
      await selectOptions(
        'team.0.contactTypeCode',
        ApiGen_CodeTypes_LeaseTeamProfileTypes.PROPANALYST,
      );
    });

    await act(async () => {
      await getFormikRef().current?.setFieldValue('team.0.contact', selectedPerson);
    });

    // Second team member
    await act(async () => {
      await userEvent.click(getByTestId('add-team-member'));
    });

    await act(async () => {
      await selectOptions(
        'team.1.contactTypeCode',
        ApiGen_CodeTypes_LeaseTeamProfileTypes.PROPANALYST,
      );
    });

    // Select SAME person
    await act(async () => {
      await getFormikRef().current?.setFieldValue('team.1.contact', selectedPerson);
    });

    await act(async () => {
      await getFormikRef().current?.validateForm();
    });

    expect(getFormikRef().current?.errors.team).toBe(
      'You have selected a team member that already has the selected role.',
    );
  });
});

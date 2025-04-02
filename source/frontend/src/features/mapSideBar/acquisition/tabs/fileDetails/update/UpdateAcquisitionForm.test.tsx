import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FormikProps } from 'formik';
import { createRef } from 'react';

import { InterestHolderType } from '@/constants/interestHolderTypes';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { IAutocompletePrediction } from '@/interfaces';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mockNotesResponse } from '@/mocks/noteResponses.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { toTypeCodeNullable } from '@/utils/formUtils';
import {
  act,
  fakeText,
  fireEvent,
  render,
  RenderOptions,
  screen,
  userEvent,
  waitFor,
  waitForEffects,
} from '@/utils/test-utils';

import { InterestHolderForm } from '../../stakeholders/update/models';
import { UpdateAcquisitionSummaryFormModel } from './models';
import { UpdateAcquisitionFileYupSchema } from './UpdateAcquisitionFileYupSchema';
import UpdateAcquisitionForm, { IUpdateAcquisitionFormProps } from './UpdateAcquisitionForm';

const mockAxios = new MockAdapter(axios);

vi.mock('@/hooks/repositories/useProjectProvider');
vi.mocked(useProjectProvider, { partial: true }).mockReturnValue({
  retrieveProjectProducts: vi.fn(),
});

vi.mock('@/hooks/repositories/useUserInfoRepository');
vi.mocked(useUserInfoRepository, { partial: true }).mockReturnValue({
  retrieveUserInfo: vi.fn(),
});

const onSubmit = vi.fn();
const validationSchema = vi.fn().mockReturnValue(UpdateAcquisitionFileYupSchema);
type TestProps = Pick<IUpdateAcquisitionFormProps, 'initialValues'>;

describe('UpdateAcquisitionForm component', () => {
  // render component under test
  const setup = async (props: TestProps, renderOptions: RenderOptions = {}) => {
    const ref = createRef<FormikProps<UpdateAcquisitionSummaryFormModel>>();
    const utils = render(
      <UpdateAcquisitionForm
        formikRef={ref}
        initialValues={props.initialValues}
        validationSchema={validationSchema}
        onSubmit={onSubmit}
      />,
      {
        ...renderOptions,
        claims: [],
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
      },
    );

    // wait for effects
    await act(async () => {});

    return {
      ...utils,
      getFormikRef: () => ref,
      getIsOrganizationRadioButtonValue: (index = 0) => {
        const radio = utils.container.querySelector(
          `input[name="owners[${index}].isOrganization"]:checked`,
        ) as HTMLInputElement;

        return radio.value;
      },
      getGivenNameTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="owners[${index}].givenName"]`,
        ) as HTMLInputElement,
      getLastNameCorpNameTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="owners[${index}].lastNameAndCorpName"]`,
        ) as HTMLInputElement,
      getOtherNameTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="owners[${index}].otherName"]`,
        ) as HTMLInputElement,
      getIncorporationTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="owners[${index}].incorporationNumber"]`,
        ) as HTMLInputElement,
      getRegistrationTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="owners[${index}].registrationNumber"]`,
        ) as HTMLInputElement,
      getEmailTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="owners[${index}].contactEmailAddress"]`,
        ) as HTMLInputElement,
      getPhoneTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="owners[${index}].contactPhoneNumber"]`,
        ) as HTMLInputElement,
      getCloseButton: () => utils.getByTitle('close'),
      getFileStatusDropdown: () =>
        utils.container.querySelector(`select[name="fileStatusTypeCode"]`) as HTMLSelectElement,
      getEstimatedCompletionDatePicker: () =>
        utils.container.querySelector(`input[name="estimatedCompletionDate"]`) as HTMLInputElement,
      getAssignedDatePicker: () =>
        utils.container.querySelector(`input[name="assignedDate"]`) as HTMLInputElement,
      getPossessionDatePicker: () =>
        utils.container.querySelector(`input[name="possessionDate"]`) as HTMLInputElement,
      getTeamMemberProfileDropDownList: (index = 0) =>
        utils.container.querySelector(
          `select[name="team.${index}.contactTypeCode"]`,
        ) as HTMLSelectElement,
      getRemoveProjectButton: () =>
        utils.container.querySelector(
          `div[data-testid="typeahead-project"] button`,
        ) as HTMLSelectElement,
      getSubfileInterestTypeDropdown: () =>
        utils.container.querySelector(
          `select[name="subfileInterestTypeCode"]`,
        ) as HTMLSelectElement,
      getOtherSubfileInterestTypeTextbox: () =>
        utils.container.querySelector(`input[name="otherSubfileInterestType"]`) as HTMLInputElement,
      getProgressAppraisalStatusTypeDropdown: () =>
        utils.container.querySelector(`#input-appraisalStatusType`) as HTMLSelectElement,
    };
  };

  let initialValues: UpdateAcquisitionSummaryFormModel;

  beforeEach(() => {
    initialValues = UpdateAcquisitionSummaryFormModel.fromApi(mockAcquisitionFileResponse());

    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
    mockAxios
      .onGet(new RegExp('acquisitionfiles/1/properties'))
      .reply(200, mockAcquisitionFileResponse().fileProperties);
    mockAxios.onGet(new RegExp('acquisitionfiles/*')).reply(200, mockAcquisitionFileResponse());
    mockAxios.onGet(new RegExp('notes/*')).reply(200, mockNotesResponse());
  });

  afterEach(() => {
    mockAxios.resetHistory();
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({ initialValues });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays legacy file number', async () => {
    const { getByDisplayValue } = await setup({ initialValues });

    expect(getByDisplayValue('legacy file number')).toBeVisible();
  });

  it('displays progress statuses', async () => {
    const { getProgressAppraisalStatusTypeDropdown: getProgessAppraisalStatusTypeDropdown } =
      await setup({ initialValues });

    expect(getProgessAppraisalStatusTypeDropdown()).toHaveValue('RECEIVED');
  });

  it('displays owner solicitor and owner representative', async () => {
    const { getByText } = await setup({ initialValues });

    expect(getByText('Millennium Inc')).toBeVisible();
    expect(getByText('Han Solo')).toBeVisible();
    expect(getByText('test representative comment')).toBeVisible();
  });

  it('displays estimated completion, assigned and possession dates', async () => {
    const { getEstimatedCompletionDatePicker, getPossessionDatePicker, getAssignedDatePicker } =
      await setup({ initialValues });

    expect(getEstimatedCompletionDatePicker()).toHaveValue('Jul 10, 2024');
    expect(getPossessionDatePicker()).toHaveValue('Jul 10, 2025');
    expect(getAssignedDatePicker()).toHaveValue('Dec 18, 2024');
  });

  it('displays Individual type Owner with data', async () => {
    const {
      getIsOrganizationRadioButtonValue,
      getGivenNameTextbox,
      getLastNameCorpNameTextbox,
      getOtherNameTextbox,
      getIncorporationTextbox,
      getRegistrationTextbox,
      getEmailTextbox,
      getPhoneTextbox,
    } = await setup({ initialValues });

    expect(getIsOrganizationRadioButtonValue()).toEqual('false');

    expect(getGivenNameTextbox(0).value).toEqual('John');
    expect(getLastNameCorpNameTextbox(0).value).toEqual('Doe');
    expect(getOtherNameTextbox(0).value).toEqual('Jr.');

    expect(getIncorporationTextbox(0)).toEqual(null);
    expect(getRegistrationTextbox(0)).toEqual(null);

    expect(getEmailTextbox(0).value).toEqual('jonh.doe@gmail.com');
    expect(getPhoneTextbox(0).value).toEqual('775-111-1111');
  });

  it('displays Corporation type Owner with data', async () => {
    const {
      getIsOrganizationRadioButtonValue,
      getGivenNameTextbox,
      getLastNameCorpNameTextbox,
      getOtherNameTextbox,
      getIncorporationTextbox,
      getRegistrationTextbox,
      getEmailTextbox,
      getPhoneTextbox,
    } = await setup({ initialValues });

    expect(getIsOrganizationRadioButtonValue(1)).toEqual('true');

    expect(getGivenNameTextbox(1)).toEqual(null);

    expect(getLastNameCorpNameTextbox(1).value).toEqual('FORTIS BC');
    expect(getOtherNameTextbox(1).value).toEqual('LTD');

    expect(getIncorporationTextbox(1).value).toEqual('9999');
    expect(getRegistrationTextbox(1).value).toEqual('12345');

    expect(getEmailTextbox(1).value).toEqual('fake@email.ca');
    expect(getPhoneTextbox(1).value).toEqual('775-111-1111');
  });

  it('it validates that only profile is not repeated on another team member', async () => {
    const { getTeamMemberProfileDropDownList, getByTestId, queryByTestId } = await setup({
      initialValues,
    });

    // Set duplicate should fail
    await act(async () => {
      userEvent.selectOptions(getTeamMemberProfileDropDownList(1), 'NEGOTAGENT');
    });

    expect(validationSchema).toBeCalled();
    expect(getByTestId('team-profile-dup-error')).toBeVisible();

    // Set unique should pass
    await act(async () => {
      userEvent.selectOptions(getTeamMemberProfileDropDownList(1), 'EXPRAGENT');
    });

    expect(validationSchema).toBeCalled();
    expect(queryByTestId(/team-profile-dup-error/i)).toBeNull();
  });

  it('it clears the product field when a project is removed', async () => {
    const { getRemoveProjectButton, getFormikRef } = await setup({
      initialValues,
    });

    await act(async () => userEvent.click(getRemoveProjectButton()));
    await waitFor(() => getFormikRef().current?.submitForm());

    initialValues.product = '';
    initialValues.project = '' as unknown as IAutocompletePrediction;

    expect(validationSchema).toBeCalled();
    expect(onSubmit).toHaveBeenLastCalledWith(initialValues, expect.anything());
  });

  describe('Sub-interest files', () => {
    let parentId: number;
    beforeEach(() => {
      parentId = 99;
      initialValues.parentAcquisitionFileId = parentId;
      initialValues.formattedProject = '1111 - Test Project';
    });

    it('should display sub file interest type SELECT', async () => {
      const { getSubfileInterestTypeDropdown } = await setup({
        initialValues,
      });
      expect(getSubfileInterestTypeDropdown()).toBeInTheDocument();
    });

    it('should display OTHER sub file interest type', async () => {
      const { getSubfileInterestTypeDropdown, getOtherSubfileInterestTypeTextbox, getByTestId } =
        await setup({
          initialValues,
        });
      const subfileInterestTypeDropdown = getSubfileInterestTypeDropdown();

      expect(subfileInterestTypeDropdown).toBeInTheDocument();
      await act(async () => {
        userEvent.click(subfileInterestTypeDropdown);
        userEvent.selectOptions(screen.getByTestId('subfileInterestTypeCode'), ['OTHER']);
      });
      await waitForEffects();

      const otherSubfileInterestTextbox = getOtherSubfileInterestTypeTextbox();
      expect(otherSubfileInterestTextbox).toBeInTheDocument();
    });

    it('should validate OTHER sub file interest type max length', async () => {
      const { findByText, getSubfileInterestTypeDropdown, getOtherSubfileInterestTypeTextbox } =
        await setup({ initialValues });
      const subfileInterestTypeDropdown = getSubfileInterestTypeDropdown();

      expect(subfileInterestTypeDropdown).toBeInTheDocument();
      await act(async () => {
        userEvent.click(subfileInterestTypeDropdown);
        userEvent.selectOptions(screen.getByTestId('subfileInterestTypeCode'), ['OTHER']);
      });
      await waitForEffects();

      const otherSubfileInterestTextbox = getOtherSubfileInterestTypeTextbox();
      expect(otherSubfileInterestTextbox).toBeInTheDocument();

      await act(async () => {
        userEvent.paste(otherSubfileInterestTextbox, fakeText(201));
        fireEvent.blur(otherSubfileInterestTextbox);
      });
      await waitForEffects();

      expect(
        await findByText(/Other Subfile interest description must be at most 200 characters/i),
      ).toBeVisible();
    });

    it('renders sub-interest information section', async () => {
      const { getByText } = await setup({ initialValues });
      await waitForEffects();

      expect(
        getByText(
          'Each property in this sub-file should be impacted by the sub-interest(s) in this section',
        ),
      ).toBeVisible();
      expect(getByText('+ Add Sub-interest')).toBeVisible();
      expect(getByText(/Sub-interest solicitor/i)).toBeVisible();
      expect(getByText(/Sub-interest representative/i)).toBeVisible();
    });

    it('renders multiple solicitors if present', async () => {
      const { getByTitle, getAllByText } = await setup({
        initialValues: {
          ...initialValues,
          toApi: vi.fn(),
          ownerSolicitors: [
            InterestHolderForm.fromApi(
              {
                interestHolderId: 1,
                interestHolderType: toTypeCodeNullable(InterestHolderType.OWNER_SOLICITOR),

                acquisitionFileId: 1,
                personId: null,
                person: null,
                organizationId: 1,
                organization: {
                  ...getEmptyOrganization(),
                  id: 1,
                  name: 'Millennium Inc',
                  alias: 'M Inc',
                  incorporationNumber: '1234',
                  comment: '',
                  contactMethods: null,
                  isDisabled: false,
                  organizationAddresses: null,
                  organizationPersons: null,
                  rowVersion: null,
                },
                interestHolderProperties: [],
                primaryContactId: 1,
                primaryContact: null,
                comment: null,
                isDisabled: false,
                ...getEmptyBaseAudit(),
              },
              InterestHolderType.OWNER_SOLICITOR,
            ),
            InterestHolderForm.fromApi(
              {
                interestHolderId: 2,
                interestHolderType: toTypeCodeNullable(InterestHolderType.OWNER_SOLICITOR),

                acquisitionFileId: 1,
                personId: null,
                person: null,
                organizationId: 2,
                organization: {
                  ...getEmptyOrganization(),
                  id: 2,
                  name: 'Test Org',
                  alias: 'M Inc',
                  incorporationNumber: '12345',
                  comment: '',
                  contactMethods: null,
                  isDisabled: false,
                  organizationAddresses: null,
                  organizationPersons: null,
                  rowVersion: null,
                },
                interestHolderProperties: [],
                primaryContactId: 1,
                primaryContact: null,
                comment: null,
                isDisabled: false,
                ...getEmptyBaseAudit(),
              },
              InterestHolderType.OWNER_SOLICITOR,
            ),
          ],
        },
      });
      await waitForEffects();

      expect(getByTitle(/O1/i)).toBeVisible();
      expect(getByTitle(/O2/i)).toBeVisible();
      expect(getAllByText('Owner solicitor:')).toHaveLength(2);
    });
  });
});

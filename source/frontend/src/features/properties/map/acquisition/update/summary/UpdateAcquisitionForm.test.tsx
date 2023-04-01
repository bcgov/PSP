import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FormikProps } from 'formik';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { mockLookups } from 'mocks/mockLookups';
import { mockNotesResponse } from 'mocks/mockNoteResponses';
import { createRef } from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import { UpdateAcquisitionSummaryFormModel } from './models';
import { UpdateAcquisitionFileYupSchema } from './UpdateAcquisitionFileYupSchema';
import UpdateAcquisitionForm, { IUpdateAcquisitionFormProps } from './UpdateAcquisitionForm';

const mockAxios = new MockAdapter(axios);

// mock auth library
jest.mock('@react-keycloak/web');

const onSubmit = jest.fn();
const validationSchema = jest.fn().mockReturnValue(UpdateAcquisitionFileYupSchema);
type TestProps = Pick<IUpdateAcquisitionFormProps, 'initialValues'>;

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

describe('UpdateAcquisitionForm component', () => {
  // render component under test
  const setup = (props: TestProps, renderOptions: RenderOptions = {}) => {
    const ref = createRef<FormikProps<UpdateAcquisitionSummaryFormModel>>();
    const utils = render(
      <UpdateAcquisitionForm
        ref={ref}
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
      getFileCompletionDatePicker: () =>
        utils.container.querySelector(`input[name="completionDate"]`) as HTMLInputElement,
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
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup({ initialValues });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays legacy file number', async () => {
    const { getByDisplayValue } = setup({ initialValues });
    expect(getByDisplayValue('legacy file number')).toBeVisible();
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
    } = setup({ initialValues });

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
    } = setup({ initialValues });

    expect(getIsOrganizationRadioButtonValue(1)).toEqual('true');

    expect(getGivenNameTextbox(1)).toEqual(null);

    expect(getLastNameCorpNameTextbox(1).value).toEqual('FORTIS BC');
    expect(getOtherNameTextbox(1).value).toEqual('LTD');

    expect(getIncorporationTextbox(1).value).toEqual('9999');
    expect(getRegistrationTextbox(1).value).toEqual('12345');

    expect(getEmailTextbox(1).value).toEqual('fake@email.ca');
    expect(getPhoneTextbox(1).value).toEqual('');
  });

  it('should disable file completion date until the user marks the file as COMPLETED', async () => {
    const { getFormikRef, getFileStatusDropdown, getFileCompletionDatePicker } = setup({
      initialValues,
    });

    await act(() => userEvent.selectOptions(getFileStatusDropdown(), 'DRAFT'));
    expect(getFileCompletionDatePicker()).toBeDisabled();

    // submit form to trigger validation check
    await waitFor(() => getFormikRef().current?.submitForm());

    expect(validationSchema).toBeCalled();
    expect(onSubmit).toBeCalled();
  });

  it('should require a file completion date when status is set to COMPLETED', async () => {
    const { getFormikRef, getFileStatusDropdown, findByText } = setup({ initialValues });

    await act(() => userEvent.selectOptions(getFileStatusDropdown(), 'COMPLT'));

    // submit form to trigger validation check
    await waitFor(() => getFormikRef().current?.submitForm());

    expect(validationSchema).toBeCalled();
    expect(
      await findByText(
        /Acquisition completed date is required when file status is set to "Complete"/i,
      ),
    ).toBeVisible();
    expect(onSubmit).not.toBeCalled();
  });
});

import { Formik } from 'formik';
import { noop } from 'lodash';
import { mockLookups } from 'mocks';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from 'utils/test-utils';

import { WithAcquisitionOwners } from '../../models';
import UpdateAcquisitionOwnersSubForm from './UpdateAcquisitionOwnersSubForm';

describe('UpdateAcquisitionOwnersSubForm component', () => {
  // render component under test
  const setup = (
    props: { initialForm: WithAcquisitionOwners },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <Formik initialValues={props.initialForm} onSubmit={noop}>
        {formikProps => <UpdateAcquisitionOwnersSubForm />}
      </Formik>,
      {
        ...renderOptions,
        store: { [lookupCodesSlice.name]: { lookupCodes: mockLookups } },
      },
    );

    return {
      ...utils,
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
      getIsPrymaryContactRadioButtonValue: (index = 0) => {
        const radio = utils.container.querySelector(
          `input[name="owners[${index}].isPrimaryContact"]:checked`,
        ) as HTMLInputElement;

        return radio.value;
      },
      getIsOrganizationRadioButtonValue: (index = 0) => {
        const radio = utils.container.querySelector(
          `input[name="owners[${index}].isOrganization"]:checked`,
        ) as HTMLInputElement;

        return radio.value;
      },
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
    };
  };

  let testForm: WithAcquisitionOwners;

  beforeEach(() => {
    testForm = { owners: [] };
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ initialForm: testForm });
    expect(asFragment()).toMatchSnapshot();
  });

  it(`renders 'Add owner' link`, async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    expect(getByTestId('add-file-owner')).toBeVisible();
  });

  it(`renders 'Remove owner' link`, async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    const addRow = getByTestId('add-file-owner');
    await act(async () => {
      userEvent.click(addRow);
    });
    expect(getByTestId('remove-button')).toBeVisible();
  });

  it(`renders owner row fields when 'Add owner' link is clicked`, async () => {
    const { getByTestId, getGivenNameTextbox, getIsPrymaryContactRadioButtonValue } = setup({
      initialForm: testForm,
    });
    const addRow = getByTestId('add-file-owner');
    await act(async () => {
      userEvent.click(addRow);
    });
    expect(getGivenNameTextbox()).toBeVisible();
    expect(getIsPrymaryContactRadioButtonValue()).toEqual('true');
  });

  it(`Only Renders the Owner as Individual fields by Default`, async () => {
    const {
      getByTestId,
      getIsPrymaryContactRadioButtonValue,
      getGivenNameTextbox,
      getLastNameCorpNameTextbox,
      getOtherNameTextbox,
      getIsOrganizationRadioButtonValue,
      getIncorporationTextbox,
      getRegistrationTextbox,
      getEmailTextbox,
      getPhoneTextbox,
    } = setup({ initialForm: testForm });
    const addRow = getByTestId('add-file-owner');
    await act(async () => {
      userEvent.click(addRow);
    });

    expect(getIsPrymaryContactRadioButtonValue()).toEqual('true');
    expect(getIsOrganizationRadioButtonValue()).toEqual('false');

    expect(getGivenNameTextbox()).toBeVisible();
    expect(getLastNameCorpNameTextbox()).toBeVisible();
    expect(getOtherNameTextbox()).toBeVisible();

    expect(getIncorporationTextbox()).toEqual(null);
    expect(getRegistrationTextbox()).toEqual(null);

    expect(getEmailTextbox()).toBeVisible();
    expect(getPhoneTextbox()).toBeVisible();
  });

  it(`Only Renders the Owner as Corporation fields`, async () => {
    const {
      container,
      getByTestId,
      getGivenNameTextbox,
      getLastNameCorpNameTextbox,
      getOtherNameTextbox,
      getIncorporationTextbox,
      getRegistrationTextbox,
      getIsOrganizationRadioButtonValue,
      getEmailTextbox,
      getPhoneTextbox,
    } = setup({ initialForm: testForm });
    const addRow = getByTestId('add-file-owner');

    await act(async () => userEvent.click(addRow));
    const organizationsButton = container.querySelector(`#input-true`);
    await act(() => organizationsButton && userEvent.click(organizationsButton));

    expect(getGivenNameTextbox()).toEqual(null);
    expect(getLastNameCorpNameTextbox()).toBeVisible();
    expect(getOtherNameTextbox()).toBeVisible();

    expect(getIsOrganizationRadioButtonValue()).toEqual('true');

    expect(getIncorporationTextbox()).toBeVisible();
    expect(getRegistrationTextbox()).toBeVisible();

    expect(getEmailTextbox()).toBeVisible();
    expect(getPhoneTextbox()).toBeVisible();
  });

  it(`displays a confirmation popup before owner is removed`, async () => {
    const { getByTestId, getByText } = setup({ initialForm: testForm });
    const addRow = getByTestId('add-file-owner');
    await act(async () => userEvent.click(addRow));
    await act(async () => userEvent.click(getByTestId('remove-button')));

    expect(getByText(/Are you sure you want to remove this Owner/i)).toBeVisible();
  });

  it(`removes the owner upon user confirmation`, async () => {
    const { getByTestId, getByText, getByTitle, getGivenNameTextbox } = setup({
      initialForm: testForm,
    });
    const addRow = getByTestId('add-file-owner');
    await act(async () => userEvent.click(addRow));
    await act(async () => userEvent.click(getByTestId('remove-button')));

    expect(getByText(/Are you sure you want to remove this Owner/i)).toBeVisible();

    await act(async () => userEvent.click(getByTitle('ok-modal')));
    expect(getGivenNameTextbox()).toBeNull();
  });

  it(`does not remove the owner when confirmation popup is cancelled`, async () => {
    const { getByTestId, getByText, getByTitle, getGivenNameTextbox } = setup({
      initialForm: testForm,
    });
    const addRow = getByTestId('add-file-owner');
    await act(async () => userEvent.click(addRow));
    await act(async () => userEvent.click(getByTestId('remove-button')));

    expect(getByText(/Are you sure you want to remove this Owner/i)).toBeVisible();

    await act(async () => userEvent.click(getByTitle('cancel-modal')));
    expect(getGivenNameTextbox()).toBeVisible();
  });
});

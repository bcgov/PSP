import { Formik } from 'formik';
import noop from 'lodash/noop';

import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

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
      getIsPrimaryContactRadioButtonValue: (index = 0) => {
        const radio = utils.container.querySelector(
          `input[name="owners[${index}].isPrimaryContact"]:checked`,
        ) as HTMLInputElement;

        return radio.value;
      },
      getIsPrimaryContactRadioButton: (index = 0) => {
        const radio = utils.container.querySelector(
          `input[name="owners[${index}].isPrimaryContact"]`,
        ) as HTMLInputElement;

        return radio;
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
      getCountryNameTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="owners[${index}].address.countryOther"]`,
        ) as HTMLInputElement,
    };
  };

  let testForm: WithAcquisitionOwners;

  beforeEach(() => {
    testForm = { owners: [] };
  });

  afterEach(() => {
    vi.clearAllMocks();
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
    expect(getByTestId('owners[0]-remove-button')).toBeVisible();
  });

  it(`renders 'country name' text`, async () => {
    const { getCountryNameTextbox } = setup({
      initialForm: {
        ...testForm,
        owners: [
          {
            isPrimaryContact: '',
            isOrganization: '',
            lastNameAndCorpName: '',
            otherName: '',
            givenName: '',
            incorporationNumber: '',
            registrationNumber: '',
            contactEmailAddress: '',
            contactPhoneNumber: '',
            isEmpty: noop as any,
            toApi: noop as any,
            address: { countryId: 4, countryOther: 'fake country' },
          },
        ],
      },
    });
    expect(getCountryNameTextbox()).toHaveDisplayValue('fake country');
  });

  it(`renders owner row fields when 'Add owner' link is clicked`, async () => {
    const { getByTestId, getGivenNameTextbox, getIsPrimaryContactRadioButtonValue } = setup({
      initialForm: testForm,
    });
    const addRow = getByTestId('add-file-owner');
    await act(async () => {
      userEvent.click(addRow);
    });
    expect(getGivenNameTextbox()).toBeVisible();
    expect(getIsPrimaryContactRadioButtonValue()).toEqual('true');
  });

  it(`Only Renders the Owner as Individual fields by Default`, async () => {
    const {
      getByTestId,
      getIsPrimaryContactRadioButtonValue,
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

    expect(getIsPrimaryContactRadioButtonValue()).toEqual('true');
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
      getIsPrimaryContactRadioButtonValue,
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
    await act(async () => {
      organizationsButton && userEvent.click(organizationsButton);
    });

    expect(getIsPrimaryContactRadioButtonValue()).toEqual('true');

    expect(getGivenNameTextbox()).toEqual(null);
    expect(getLastNameCorpNameTextbox()).toBeVisible();
    expect(getOtherNameTextbox()).toBeVisible();

    expect(getIsOrganizationRadioButtonValue()).toEqual('true');

    expect(getIncorporationTextbox()).toBeVisible();
    expect(getRegistrationTextbox()).toBeVisible();

    expect(getEmailTextbox()).toBeVisible();
    expect(getPhoneTextbox()).toBeVisible();
  });

  it(`It removes the Primary contact flag when a second owner is marked as primary and update when primary is removed`, async () => {
    const { container, getByTestId, getIsPrimaryContactRadioButton, getByTitle } = setup({
      initialForm: testForm,
    });
    const addRow = getByTestId('add-file-owner');

    await act(async () => userEvent.click(addRow));
    await act(async () => userEvent.click(addRow));

    const organizationsButton = container.querySelector(`#input-true`);
    await act(async () => {
      organizationsButton && userEvent.click(organizationsButton);
    });

    let firstOwnerPrimaryFlag = getIsPrimaryContactRadioButton();
    let secondOwnerPrimaryFlag = getIsPrimaryContactRadioButton(1);

    expect(firstOwnerPrimaryFlag).toHaveProperty('checked', true);
    expect(secondOwnerPrimaryFlag).toHaveProperty('checked', false);

    // Mark second owner as Primary
    await act(async () => userEvent.click(secondOwnerPrimaryFlag));

    expect(firstOwnerPrimaryFlag).toHaveProperty('checked', false);
    expect(secondOwnerPrimaryFlag).toHaveProperty('checked', true);

    // Mark first Owner as Primary
    await act(async () => userEvent.click(firstOwnerPrimaryFlag));
    expect(firstOwnerPrimaryFlag).toHaveProperty('checked', true);
    expect(secondOwnerPrimaryFlag).toHaveProperty('checked', false);

    // remove the first owner which should automatically update the reamining owner as primary.
    await act(async () => userEvent.click(getByTestId('owners[0]-remove-button')));
    await act(async () => userEvent.click(getByTitle('ok-modal')));

    firstOwnerPrimaryFlag = getIsPrimaryContactRadioButton();
    secondOwnerPrimaryFlag = getIsPrimaryContactRadioButton(1);

    //Previously second owner.
    expect(firstOwnerPrimaryFlag).toHaveProperty('checked', true);
    // no second owner.
    expect(secondOwnerPrimaryFlag).toBe(null);
  });

  it(`displays a confirmation popup before owner is removed`, async () => {
    const { getByTestId, getByText } = setup({ initialForm: testForm });
    const addRow = getByTestId('add-file-owner');
    await act(async () => userEvent.click(addRow));
    await act(async () => userEvent.click(getByTestId('owners[0]-remove-button')));

    expect(getByText(/Are you sure you want to remove this Owner/i)).toBeVisible();
  });

  it(`removes the owner upon user confirmation`, async () => {
    const { getByTestId, getByText, getByTitle, getGivenNameTextbox } = setup({
      initialForm: testForm,
    });
    const addRow = getByTestId('add-file-owner');
    await act(async () => userEvent.click(addRow));
    await act(async () => userEvent.click(getByTestId('owners[0]-remove-button')));

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
    await act(async () => userEvent.click(getByTestId('owners[0]-remove-button')));

    expect(getByText(/Are you sure you want to remove this Owner/i)).toBeVisible();

    await act(async () => userEvent.click(getByTitle('cancel-modal')));
    expect(getGivenNameTextbox()).toBeVisible();
  });
});

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
    const { getByTestId, getGivenNameTextbox } = setup({ initialForm: testForm });
    const addRow = getByTestId('add-file-owner');
    await act(async () => {
      userEvent.click(addRow);
    });
    expect(getGivenNameTextbox()).toBeVisible();
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

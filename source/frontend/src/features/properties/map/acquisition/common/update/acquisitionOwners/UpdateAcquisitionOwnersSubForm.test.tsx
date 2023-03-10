import { Formik } from 'formik';
import { noop } from 'lodash';
import { mockLookups } from 'mocks';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from 'utils/test-utils';

import { WithAcquisitionOwners } from '../../models';
import UpdateAcquisitionOwnersSubForm from './UpdateAcquisitionOwnersSubForm';

describe('AcquisitionTeam component', () => {
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
      getGivenNameTextbox: (index: string = '0') =>
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

  it('renders add new Owner link', async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    expect(getByTestId('add-file-owner')).toBeVisible();
  });

  it('renders remove new team member link', async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    const addRow = getByTestId('add-file-owner');
    await act(async () => {
      userEvent.click(addRow);
    });

    expect(getByTestId('remove-button')).toBeVisible();
  });

  it('renders owner row fields', async () => {
    const { getByTestId, getGivenNameTextbox } = setup({ initialForm: testForm });
    const addRow = getByTestId('add-file-owner');
    await act(async () => {
      userEvent.click(addRow);
    });
    expect(getGivenNameTextbox()).toBeVisible();
  });
});

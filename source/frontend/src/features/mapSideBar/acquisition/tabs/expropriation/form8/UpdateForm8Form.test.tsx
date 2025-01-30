import { FormikProps } from 'formik';
import { createRef } from 'react';

import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fillInput,
  render,
  RenderOptions,
  userEvent,
  waitForEffects,
} from '@/utils/test-utils';

import { Form8FormModel } from './models/Form8FormModel';
import UpdateForm8Form, { IForm8FormProps } from './UpdateForm8Form';
import { mockGetExpropriationPaymentApi } from '@/mocks/ExpropriationPayment.mock';
import { PayeeOption } from '../../../models/PayeeOptionModel';

const currentGstPercent = 0.05;
const onSave = vi.fn();
const onCancel = vi.fn();
const onSucces = vi.fn();

const acquisitionFileMock = mockAcquisitionFileResponse();
const defatulForm8Model = new Form8FormModel(null, acquisitionFileMock.id!);

describe('Form 8 UpdateForm component', () => {
  const setup = async (renderOptions: RenderOptions & { props?: Partial<IForm8FormProps> }) => {
    const formikRef = createRef<FormikProps<Form8FormModel>>();
    const utils = render(
      <UpdateForm8Form
        {...renderOptions.props}
        onSave={onSave}
        onCancel={onCancel}
        onSuccess={onSucces}
        payeeOptions={renderOptions.props?.payeeOptions ?? []}
        initialValues={renderOptions.props?.initialValues ?? defatulForm8Model}
        gstConstant={currentGstPercent}
      />,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
      },
    );

    return {
      ...utils,
      formikRef,
      getPayeeOptionSelect: () =>
        utils.container.querySelector('select[name="payeeKey"]') as HTMLInputElement,
      getExpropriationAuthoritySelect: () =>
        utils.container.querySelector(
          'input[name="expropriationAuthority.contact.id"]',
        ) as HTMLInputElement,
      getDescriptionTextbox: () =>
        utils.container.querySelector('textarea[name="description"]') as HTMLInputElement,
      getAdvancedPaymentServedDate: () => {
        return utils.container.querySelector(
          `input[name="advancedPaymentServedDate"]`,
        ) as HTMLInputElement;
      },
      getSaveButton: () => utils.getByText(/Save/i),
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it(`renders new Form 8 with default values`, async () => {
    const {
      queryByTestId,
      getPayeeOptionSelect,
      getExpropriationAuthoritySelect,
      getDescriptionTextbox,
      getAdvancedPaymentServedDate,
    } = await setup({});

    expect(getPayeeOptionSelect()).toHaveValue('');
    expect(getExpropriationAuthoritySelect()).toHaveValue('');
    expect(getDescriptionTextbox()).toHaveValue('');
    expect(getAdvancedPaymentServedDate()).toHaveValue('');

    expect(queryByTestId(`paymentItems[0]`)).not.toBeInTheDocument();
  });

  it('displays the Form8 values from api', async () => {
    const mockExpropiationPaymentApi = mockGetExpropriationPaymentApi(1, 1);
    const ownerMockOption = PayeeOption.createOwner(mockExpropiationPaymentApi.acquisitionOwner);

    const { getAdvancedPaymentServedDate, getDescriptionTextbox, getPayeeOptionSelect } =
      await setup({
        props: {
          initialValues: Form8FormModel.fromApi(mockExpropiationPaymentApi),
          payeeOptions: [ownerMockOption],
        },
      });

    await waitForEffects();
    expect(getAdvancedPaymentServedDate()).toHaveValue('Jan 02, 2025');
    expect(getDescriptionTextbox()).toHaveValue('MY DESCRIPTION');
    expect(getPayeeOptionSelect()).toHaveDisplayValue('John Doe Jr. (Owner)');
  });

  it('validates that only one payment item per type is added', async () => {
    const { container, getByTestId, findByText, getSaveButton } = await setup({});

    await act(async () => userEvent.click(getByTestId('add-payment-item')));
    await act(async () => {
      fillInput(container, 'paymentItems[0].paymentItemTypeCode', 'MARKETVALUE', 'select');
    });

    await act(async () => userEvent.click(getByTestId('add-payment-item')));
    await act(async () => {
      fillInput(container, 'paymentItems[1].paymentItemTypeCode', 'MARKETVALUE', 'select');
    });

    await act(async () => userEvent.click(getSaveButton()));

    const error = await findByText(
      'Each payment type can only be added once. Select a different payment type.',
    );
    expect(error).toBeVisible();
  });
});

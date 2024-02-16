import { FormikProps } from 'formik';
import { createRef } from 'react';

import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { Form8FormModel } from './models/Form8FormModel';
import UpdateForm8Form, { IForm8FormProps } from './UpdateForm8Form';

const currentGstPercent = 0.05;
const onSave = jest.fn();
const onCancel = jest.fn();
const onSucces = jest.fn();

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
        payeeOptions={[]}
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
      getSaveButton: () => utils.getByText(/Save/i),
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it(`renders new Form 8 with default values`, async () => {
    const {
      queryByTestId,
      getPayeeOptionSelect,
      getExpropriationAuthoritySelect,
      getDescriptionTextbox,
    } = await setup({});

    expect(getPayeeOptionSelect()).toHaveValue('');
    expect(getExpropriationAuthoritySelect()).toHaveValue('');
    expect(getDescriptionTextbox()).toHaveValue('');

    expect(queryByTestId(`paymentItems[0]`)).not.toBeInTheDocument();
  });

  it('validates that only one payment item per type is added', async () => {
    const { container, getByTestId, findByText, getSaveButton } = await setup({});

    await act(async () => userEvent.click(getByTestId('add-payment-item')));
    await act(() => {
      fillInput(container, 'paymentItems[0].paymentItemTypeCode', 'MARKETVALUE', 'select');
    });

    await act(async () => userEvent.click(getByTestId('add-payment-item')));
    await act(() => {
      fillInput(container, 'paymentItems[1].paymentItemTypeCode', 'MARKETVALUE', 'select');
    });

    await act(async () => userEvent.click(getSaveButton()));

    const error = await findByText(
      'Each payment type can only be added once. Select a different payment type.',
    );
    expect(error).toBeVisible();
  });
});

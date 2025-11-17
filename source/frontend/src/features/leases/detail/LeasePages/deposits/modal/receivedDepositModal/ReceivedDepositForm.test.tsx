import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import React from 'react';

import { MAX_SQL_MONEY_SIZE } from '@/constants/API';
import { IContactSearchResult } from '@/interfaces';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_CodeTypes_LeaseSecurityDepositTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseSecurityDepositTypes';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fakeText, fillInput, render, RenderOptions } from '@/utils/test-utils';

import { FormLeaseDeposit } from '../../models/FormLeaseDeposit';
import ReceivedDepositForm from './ReceivedDepositForm';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

const onSave = vi.fn();

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('ReceivedDepositForm component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { initialValues: FormLeaseDeposit }) => {
    const formikRef = React.createRef<FormikProps<FormLeaseDeposit>>();
    const utils = render(
      <ReceivedDepositForm
        onSave={onSave}
        formikRef={formikRef}
        initialValues={renderOptions.initialValues}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return { ...utils, formikRef };
  };

  let initialValues: FormLeaseDeposit;

  beforeEach(() => {
    mockAxios.resetHistory();
    vi.clearAllMocks();
    initialValues = FormLeaseDeposit.createEmpty(1);
  });

  it('renders as expected', async () => {
    const { asFragment } = setup({ initialValues });
    expect(asFragment()).toMatchSnapshot();
  });

  it('validates that the deposit date is not required', async () => {
    const { container, findByDisplayValue } = setup({ initialValues });

    let input: HTMLInputElement;
    await act(async () => {
      const result = await fillInput(container, 'depositDate', '2020-01-02', 'datepicker');
      input = result.input as HTMLInputElement;
    });

    await findByDisplayValue('Jan 02, 2020');
    expect(input).toHaveProperty('required', false);
  });

  it('should validate required fields', async () => {
    const { container, findByText } = setup({ initialValues });

    await act(async () => {
      await fillInput(container, 'depositTypeCode', '', 'select');
    });
    expect(await findByText(/Deposit Type is required/i)).toBeVisible();

    await act(async () => {
      await fillInput(container, 'contactHolder', '');
    });
    expect(await findByText(/Deposit Holder is required/i)).toBeVisible();
  });

  it('should require description if type is Other', async () => {
    const { container, findByText } = setup({ initialValues });

    await act(async () => {
      await fillInput(
        container,
        'depositTypeCode',
        ApiGen_CodeTypes_LeaseSecurityDepositTypes.OTHER,
        'select',
      );
    });

    await act(async () => {
      await fillInput(container, 'otherTypeDescription', fakeText(20));
    });

    let descriptionInput: HTMLTextAreaElement;
    await act(async () => {
      const result = await fillInput(container, 'description', '', 'textarea');
      descriptionInput = result.input as HTMLTextAreaElement;
    });
    expect(
      await findByText(/Description required when Deposit type "Other" is selected/i),
    ).toBeVisible();

    expect(descriptionInput).toHaveProperty('required', true);
  });

  it('should validate character limits', async () => {
    const { container, findByText } = setup({ initialValues });

    await act(async () => {
      await fillInput(
        container,
        'depositTypeCode',
        ApiGen_CodeTypes_LeaseSecurityDepositTypes.OTHER,
        'select',
      );
    });
    expect(await findByText(/Describe other/i)).toBeVisible();

    await act(async () => {
      await fillInput(container, 'otherTypeDescription', fakeText(201));
    });
    expect(
      await findByText(/Other type description must be at most 200 characters/i),
    ).toBeVisible();

    await act(async () => {
      await fillInput(container, 'description', fakeText(2001), 'textarea');
    });
    expect(await findByText(/Description must be at most 2000 characters/i)).toBeVisible();

    await act(async () => {
      await fillInput(container, 'amountPaid', MAX_SQL_MONEY_SIZE + 10);
    });
    expect(await findByText(`Amount paid must be less than ${MAX_SQL_MONEY_SIZE}`)).toBeVisible();
  });

  it('displays other type text if Other is selected', async () => {
    const { container, findByText } = setup({ initialValues });

    let otherField = container.querySelector(`input[name="otherTypeDescription"]`);
    expect(otherField).toBeNull();

    await act(async () => {
      await fillInput(
        container,
        'depositTypeCode',
        ApiGen_CodeTypes_LeaseSecurityDepositTypes.OTHER,
        'select',
      );
    });
    const otherText = await findByText('Describe other:');
    expect(otherText).toBeVisible();

    otherField = container.querySelector(`input[name="otherTypeDescription"]`);
    expect(otherField).toBeVisible();
  });

  it('calls onSave when form is submitted', async () => {
    const mockDeposit = FormLeaseDeposit.createEmpty(1);
    mockDeposit.depositTypeCode = ApiGen_CodeTypes_LeaseSecurityDepositTypes.OTHER;
    mockDeposit.otherTypeDescription = 'Some other type';
    mockDeposit.description = 'This is a description';
    mockDeposit.amountPaid = 500;
    mockDeposit.contactHolder = {} as IContactSearchResult;

    const { container, findByText, formikRef } = setup({ initialValues: mockDeposit });

    const otherText = await findByText('Describe other:');
    expect(otherText).toBeVisible();

    let otherField = container.querySelector(`input[name="otherTypeDescription"]`);
    expect(otherField).toBeVisible();

    await act(async () => {
      formikRef.current?.submitForm();
    });

    expect(onSave).toHaveBeenCalledWith(mockDeposit);
  });
});

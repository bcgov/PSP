import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { MAX_SQL_MONEY_SIZE } from '@/constants/API';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { fakeText, fillInput, render, RenderOptions } from '@/utils/test-utils';

import { FormLeaseDeposit } from '../../models/FormLeaseDeposit';
import ReceivedDepositForm from './ReceivedDepositForm';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

const onSave = jest.fn();
const submitForm = jest.fn();

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('ReceivedDepositForm component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { initialValues: FormLeaseDeposit }) => {
    const utils = render(
      <ReceivedDepositForm
        onSave={onSave}
        formikRef={{ current: { submitForm } } as any}
        initialValues={renderOptions.initialValues}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return { ...utils };
  };

  let initialValues: FormLeaseDeposit;

  beforeEach(() => {
    mockAxios.resetHistory();
    jest.clearAllMocks();
    initialValues = FormLeaseDeposit.createEmpty(1);
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ initialValues });
    expect(asFragment()).toMatchSnapshot();
  });
  it('renders with data as expected', () => {
    const { asFragment } = setup({ initialValues });
    expect(asFragment()).toMatchSnapshot();
  });

  it('validates that the deposit date is required', async () => {
    const { container, findByDisplayValue } = setup({ initialValues });

    const { input } = await fillInput(container, 'depositDate', '2020-01-02', 'datepicker');
    await findByDisplayValue('Jan 02, 2020');
    expect(input).toHaveProperty('required');
  });

  it('should validate required fields', async () => {
    const { container, findByText } = setup({ initialValues });

    await fillInput(container, 'depositTypeCode', '', 'select');
    expect(await findByText(/Deposit Type is required/i)).toBeVisible();

    await fillInput(container, 'description', '', 'textarea');
    expect(await findByText(/Description is required/i)).toBeVisible();

    await fillInput(container, 'amountPaid', '');
    expect(await findByText(/Deposit amount is required/i)).toBeVisible();

    await fillInput(container, 'depositDate', '');
    expect(await findByText(/Deposit Date is required/i)).toBeVisible();
  });

  it('should validate character limits', async () => {
    const { container, findByText } = setup({ initialValues });

    await fillInput(container, 'depositTypeCode', 'OTHER', 'select');
    expect(await findByText(/Describe other/i)).toBeVisible();

    await fillInput(container, 'otherTypeDescription', fakeText(201));
    expect(
      await findByText(/Other type description must be at most 200 characters/i),
    ).toBeVisible();

    await fillInput(container, 'description', fakeText(2001), 'textarea');
    expect(await findByText(/Description must be at most 2000 characters/i)).toBeVisible();

    await fillInput(container, 'amountPaid', MAX_SQL_MONEY_SIZE + 10);
    expect(await findByText(`Amount paid must be less than ${MAX_SQL_MONEY_SIZE}`)).toBeVisible();
  });
});

import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import Claims from 'constants/claims';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import {
  defaultFormLease,
  defaultFormLeaseTerm,
  IContactSearchResult,
  ILeasePayment,
} from 'interfaces';
import { noop } from 'lodash';
import { getAllByRole as getAllByRoleBase, renderAsync, RenderOptions } from 'utils/test-utils';

import { ITermsFormProps, TermsForm } from './TermsForm';

jest.mock('@react-keycloak/web');
const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const defaultTestFormLeaseTerm = {
  ...defaultFormLeaseTerm,
  isTermExercised: false,
  startDate: '2020-01-01',
  expiryDate: '2020-12-15',
  paymentAmount: 1000,
};

describe('TermsForm component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<ITermsFormProps> & {
        initialValues?: any;
        selectedTenants?: IContactSearchResult[];
        onCancel?: () => void;
        setSelectedTenants?: (tenants: IContactSearchResult[]) => void;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <TermsForm
          onEdit={noop}
          onDelete={noop}
          onEditPayment={noop}
          onDeletePayment={noop}
          onSavePayment={noop}
        />
      </Formik>,
      {
        ...renderOptions,
        claims: [Claims.LEASE_EDIT],
        history,
      },
    );

    return {
      findFirstRow: () => {
        const rows = component.getAllByRole('row');
        return rows && rows.length > 1 ? rows[1] : null;
      },
      findCell: (row: HTMLElement, index: number) => {
        const columns = getAllByRoleBase(row, 'cell');
        return columns && columns.length > index ? columns[index] : null;
      },
      component,
    };
  };

  const setupWithRange = async (frequency: string) => {
    return await setup({
      store: { systemConstant: { systemConstants: [{ name: 'GST', value: '5.0' }] } },
      initialValues: {
        ...defaultFormLease,
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            leasePmtFreqTypeCode: { id: frequency, description: frequency },
            isGstEligible: true,
            gstAmount: 50,
          },
        ],
      },
    });
  };

  beforeEach(() => {
    mockAxios.resetHistory();
  });
  it('renders as expected', async () => {
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders with data as expected', async () => {
    const { component } = await setup({
      initialValues: { ...defaultFormLease, terms: [defaultFormLeaseTerm] },
    });

    expect(component.asFragment()).toMatchSnapshot();
  });
  test.each([
    ['ANNUAL', '$1,000', '$1,050'],
    ['SEMIANN', '$1,000', '$2,100'],
    ['QUARTER', '$1,000', '$4,200'],
    ['BIMONTH', '$1,000', '$6,300'],
    ['MONTHLY', '$1,000', '$12,600'],
    ['BIWEEK', '$1,000', '$26,250'],
    ['WEEKLY', '$1,000', '$52,500'],
    ['DAILY', '$1,000', '$366,450'],
    ['PREPAID', '$1,000', '$1,050'],
    ['NOMINAL', '$1,000', '$1,050'],
  ])(
    'performs %s calculation correctly',
    async (frequency: string, amount: string, total: string) => {
      const { findFirstRow, findCell } = await setupWithRange(frequency);

      const row = findFirstRow() as HTMLElement;
      expect(row).not.toBeNull();
      expect(findCell(row, 1)?.textContent).toBe('Jan 1, 2020 - Dec 15, 2020');
      expect(findCell(row, 2)?.textContent).toBe(frequency);
      expect(findCell(row, 4)?.textContent).toBe(amount);
      expect(findCell(row, 8)?.textContent).toBe(total);
    },
  );
  it('Does not display certain fields if the end date is not supplied', async () => {
    const { findFirstRow, findCell } = await setup({
      store: undefined,
      initialValues: {
        ...defaultFormLease,
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            expiryDate: undefined as any,
            leasePmtFreqTypeCode: { id: 'MONTHLY', description: 'MONTHLY' },
          },
        ],
      },
    });

    const row = findFirstRow() as HTMLElement;
    expect(row).not.toBeNull();
    expect(findCell(row, 8)?.textContent).toBe('-');
  });

  it('Does not display certain fields if the gst constant is not supplied', async () => {
    const { findFirstRow, findCell } = await setup({
      store: undefined,
      initialValues: {
        ...defaultFormLease,
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            leasePmtFreqTypeCode: { id: 'MONTHLY', description: 'MONTHLY' },
          },
        ],
      },
    });

    const row = findFirstRow() as HTMLElement;
    expect(row).not.toBeNull();
    expect(findCell(row, 6)?.textContent).toBe('-');
    expect(findCell(row, 7)?.textContent).toBe('$1,000');
    expect(findCell(row, 8)?.textContent).toBe('$12,000');
  });

  it('Does not display certain fields if the amount is not supplied', async () => {
    const { findFirstRow, findCell } = await setup({
      store: { systemConstant: { systemConstants: [{ name: 'GST', value: '5.0' }] } },
      initialValues: {
        ...defaultFormLease,
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            leasePmtFreqTypeCode: { id: 'MONTHLY', description: 'MONTHLY' },
            paymentAmount: undefined as any,
          },
        ],
      },
    });

    const row = findFirstRow() as HTMLElement;
    expect(row).not.toBeNull();
    expect(findCell(row, 4)?.textContent).toBe('$0');
    expect(findCell(row, 7)?.textContent).toBe('-');
    expect(findCell(row, 8)?.textContent).toBe('-');
  });

  it('Does not calculate gst if gst flag is off', async () => {
    const { findFirstRow, findCell } = await setup({
      store: { systemConstant: { systemConstants: [{ name: 'GST', value: '5.0' }] } },
      initialValues: {
        ...defaultFormLease,
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            leasePmtFreqTypeCode: { id: 'MONTHLY', description: 'MONTHLY' },
            isGstEligible: false,
          },
        ],
      },
    });

    const row = findFirstRow() as HTMLElement;
    expect(row).not.toBeNull();
    expect(findCell(row, 5)?.textContent).toBe('N');
    expect(findCell(row, 6)?.textContent).toBe('-');
    expect(findCell(row, 7)?.textContent).toBe('$1,000');
    expect(findCell(row, 8)?.textContent).toBe('$12,000');
  });

  it('Does not calculate payments if term is not exercised', async () => {
    const { findFirstRow, findCell } = await setup({
      store: { systemConstant: { systemConstants: [{ name: 'GST', value: '5.0' }] } },
      initialValues: {
        ...defaultFormLease,
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            leasePmtFreqTypeCode: { id: 'MONTHLY', description: 'MONTHLY' },
            isTermExercised: false,
          },
        ],
      },
    });

    const row = findFirstRow() as HTMLElement;
    expect(row).not.toBeNull();
    expect(findCell(row, 9)?.textContent).toBe('-');
  });

  it('sums payment fields if exercised', async () => {
    const { findFirstRow, findCell } = await setup({
      store: { systemConstant: { systemConstants: [{ name: 'GST', value: '5.0' }] } },
      initialValues: {
        ...defaultFormLease,
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            isTermExercised: true,
            payments: [{ amountTotal: 1 }, { amountTotal: 1 }] as ILeasePayment[],
          },
        ],
      },
    });

    const row = findFirstRow() as HTMLElement;
    expect(row).not.toBeNull();
    expect(findCell(row, 9)?.textContent).toBe('$2');
  });

  it('displays the first column correctly', async () => {
    const { component, findCell } = await setup({
      initialValues: {
        ...defaultFormLease,
        terms: [
          {
            ...defaultTestFormLeaseTerm,
          },
          {
            ...defaultTestFormLeaseTerm,
          },
        ],
      },
    });
    const rows = component.getAllByRole('row');
    expect(rows).toHaveLength(3);
    expect(findCell(rows[1], 0)?.textContent).toBe('Initial term');
    expect(findCell(rows[2], 0)?.textContent).toBe('Renewal 1');
  });

  it('displays the last payment date correctly', async () => {
    const {
      component: { findByText },
    } = await setup({
      initialValues: {
        ...defaultFormLease,
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            isTermExercised: true,
            payments: [
              { amountTotal: 1, receivedDate: '2020-01-01' },
              { amountTotal: 1, receivedDate: '2021-01-01' },
            ] as ILeasePayment[],
          },
        ],
      },
    });
    expect(await findByText('last payment received: Jan 1, 2021'));
  });
});

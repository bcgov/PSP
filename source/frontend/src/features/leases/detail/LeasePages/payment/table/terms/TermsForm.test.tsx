import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { Claims } from '@/constants';
import { LeaseTermStatusTypes } from '@/constants/leaseStatusTypes';
import { LeaseFormModel } from '@/features/leases/models';
import { IContactSearchResult } from '@/interfaces';
import {
  act,
  getAllByRole as getAllByRoleBase,
  renderAsync,
  RenderOptions,
  userEvent,
} from '@/utils/test-utils';

import { defaultFormLeaseTerm, FormLeasePayment } from '../../models';
import TermsForm, { ITermsFormProps } from './TermsForm';
import { createRef } from 'react';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const defaultTestFormLeaseTerm = {
  ...defaultFormLeaseTerm,
  id: 1,
  isTermExercised: false,
  startDate: '2020-01-01T18:00',
  expiryDate: '2020-12-15T18:00',
  paymentAmount: 1000,
};
const onGenerate = vi.fn();

describe('TermsForm component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<ITermsFormProps> & {
        initialValues?: Partial<LeaseFormModel>;
        selectedTenants?: IContactSearchResult[];
        onCancel?: () => void;
        setSelectedTenants?: (tenants: IContactSearchResult[]) => void;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <TermsForm
        onEdit={noop}
        onDelete={noop}
        onEditPayment={noop}
        onDeletePayment={noop}
        onSavePayment={noop}
        onGenerate={onGenerate}
        formikRef={createRef()}
        lease={renderOptions.initialValues ?? ({} as any)}
      />,
      {
        ...renderOptions,
        claims: [Claims.LEASE_EDIT, Claims.LEASE_DELETE, Claims.LEASE_VIEW],
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
        ...new LeaseFormModel(),
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            leasePmtFreqTypeCode: {
              id: frequency,
              description: frequency,
              displayOrder: null,
              isDisabled: false,
            },
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
      initialValues: { ...new LeaseFormModel(), terms: [defaultFormLeaseTerm] },
    });

    expect(component.asFragment()).toMatchSnapshot();
  });
  test.each([
    ['ANNUAL', '$1,000.00', '$1,050.00'],
    ['SEMIANN', '$1,000.00', '$2,100.00'],
    ['QUARTER', '$1,000.00', '$4,200.00'],
    ['BIMONTH', '$1,000.00', '$6,300.00'],
    ['MONTHLY', '$1,000.00', '$12,600.00'],
    ['BIWEEK', '$1,000.00', '$26,250.00'],
    ['WEEKLY', '$1,000.00', '$52,500.00'],
    ['DAILY', '$1,000.00', '$366,450.00'],
    ['PREPAID', '$1,000.00', '$1,050.00'],
    ['NOMINAL', '$1,000.00', '$1,050.00'],
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
        ...new LeaseFormModel(),
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            expiryDate: undefined as any,
            leasePmtFreqTypeCode: {
              id: 'MONTHLY',
              description: 'MONTHLY',
              displayOrder: null,
              isDisabled: false,
            },
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
        ...new LeaseFormModel(),
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            leasePmtFreqTypeCode: {
              id: 'MONTHLY',
              description: 'MONTHLY',
              displayOrder: null,
              isDisabled: false,
            },
          },
        ],
      },
    });

    const row = findFirstRow() as HTMLElement;
    expect(row).not.toBeNull();
    expect(findCell(row, 6)?.textContent).toBe('-');
    expect(findCell(row, 7)?.textContent).toBe('$1,000.00');
    expect(findCell(row, 8)?.textContent).toBe('$12,000.00');
  });

  it('Does not display certain fields if the amount is not supplied', async () => {
    const { findFirstRow, findCell } = await setup({
      store: { systemConstant: { systemConstants: [{ name: 'GST', value: '5.0' }] } },
      initialValues: {
        ...new LeaseFormModel(),
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            leasePmtFreqTypeCode: {
              id: 'MONTHLY',
              description: 'MONTHLY',
              displayOrder: null,
              isDisabled: false,
            },
            paymentAmount: undefined as any,
          },
        ],
      },
    });

    const row = findFirstRow() as HTMLElement;
    expect(row).not.toBeNull();
    expect(findCell(row, 4)?.textContent).toBe('$0.00');
    expect(findCell(row, 7)?.textContent).toBe('-');
    expect(findCell(row, 8)?.textContent).toBe('-');
  });

  it('Does not calculate gst if gst flag is off', async () => {
    const { findFirstRow, findCell } = await setup({
      store: { systemConstant: { systemConstants: [{ name: 'GST', value: '5.0' }] } },
      initialValues: {
        ...new LeaseFormModel(),
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            leasePmtFreqTypeCode: {
              id: 'MONTHLY',
              description: 'MONTHLY',
              displayOrder: null,
              isDisabled: false,
            },
            isGstEligible: false,
          },
        ],
      },
    });

    const row = findFirstRow() as HTMLElement;
    expect(row).not.toBeNull();
    expect(findCell(row, 5)?.textContent).toBe('N');
    expect(findCell(row, 6)?.textContent).toBe('-');
    expect(findCell(row, 7)?.textContent).toBe('$1,000.00');
    expect(findCell(row, 8)?.textContent).toBe('$12,000.00');
  });

  it('Does not calculate payments if term is not exercised', async () => {
    const { findFirstRow, findCell } = await setup({
      store: { systemConstant: { systemConstants: [{ name: 'GST', value: '5.0' }] } },
      initialValues: {
        ...new LeaseFormModel(),
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            leasePmtFreqTypeCode: {
              id: 'MONTHLY',
              description: 'MONTHLY',
              displayOrder: null,
              isDisabled: false,
            },
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
        ...new LeaseFormModel(),
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            isTermExercised: true,
            payments: [{ amountTotal: 1 }, { amountTotal: 1 }] as FormLeasePayment[],
          },
        ],
      },
    });

    const row = findFirstRow() as HTMLElement;
    expect(row).not.toBeNull();
    expect(findCell(row, 9)?.textContent).toBe('$2.00');
  });

  it('displays the first column correctly', async () => {
    const { component, findCell } = await setup({
      initialValues: {
        ...new LeaseFormModel(),
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
        ...new LeaseFormModel(),
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            isTermExercised: true,
            payments: [
              { amountTotal: 1, receivedDate: '2020-01-01T18:00' },
              { amountTotal: 1, receivedDate: '2021-01-01T18:00' },
            ] as FormLeasePayment[],
          },
        ],
      },
    });
    expect(await findByText('last payment received: Jan 1, 2021'));
  });

  it('renders a delete icon when term has no payments and is not exercised', async () => {
    const {
      component: { getAllByTitle, getAllByRole },
    } = await setup({
      initialValues: {
        ...new LeaseFormModel(),
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            isTermExercised: false,
            payments: [] as FormLeasePayment[],
          },
        ],
      },
    });

    const rows = getAllByRole('row');
    expect(rows).toHaveLength(2);

    expect(getAllByTitle('delete term')[0]).toBeVisible();
  });

  it('renders a tooltip instead of a delete icon when term is exercised', async () => {
    const {
      component: { getByTestId, queryByTitle },
    } = await setup({
      initialValues: {
        ...new LeaseFormModel(),
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            isTermExercised: true,
            statusTypeCode: {
              id: LeaseTermStatusTypes.EXERCISED,
              description: null,
              displayOrder: null,
              isDisabled: false,
            },
            payments: [] as FormLeasePayment[],
          },
        ],
      },
    });

    const tooltip = getByTestId('tooltip-icon-no-delete-tooltip-term-1');
    expect(queryByTitle('delete term')).toBeNull();
    expect(tooltip).toBeVisible();
    expect(tooltip.id).toBe('no-delete-tooltip-term-1');
  });

  it('renders a tooltip instead of a delete icon when term has one or more payments', async () => {
    const {
      component: { getByTestId, queryByTitle },
    } = await setup({
      initialValues: {
        ...new LeaseFormModel(),
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            isTermExercised: false,
            payments: [{ amountTotal: 1, receivedDate: '2020-01-01T18:00' }] as FormLeasePayment[],
          },
        ],
      },
    });

    const tooltip = getByTestId('tooltip-icon-no-delete-tooltip-term-1');
    expect(queryByTitle('delete term')).toBeNull();
    expect(tooltip).toBeVisible();
    expect(tooltip.id).toBe('no-delete-tooltip-term-1');
  });

  it('does not render generation button if missing permissions', async () => {
    const {
      component: { queryByTitle },
    } = await setup({
      initialValues: {
        ...new LeaseFormModel(),
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            isTermExercised: false,
            payments: [{ amountTotal: 1, receivedDate: '2020-01-01T18:00' }] as FormLeasePayment[],
          },
        ],
      },
      claims: [],
    });

    expect(queryByTitle('Generate H1005(a)')).toBeNull();
  });

  it('does not render generation button if lease is not of expected type', async () => {
    const {
      component: { queryByTitle },
    } = await setup({
      initialValues: {
        ...new LeaseFormModel(),
        leaseTypeCode: 'OTHER',
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            isTermExercised: false,
            payments: [{ amountTotal: 1, receivedDate: '2020-01-01T18:00' }] as FormLeasePayment[],
          },
        ],
      },
    });

    expect(queryByTitle('Generate H1005(a)')).toBeNull();
  });

  it('only renders generation button on first term', async () => {
    const { findFirstRow, findCell } = await setup({
      initialValues: {
        ...new LeaseFormModel(),
        leaseTypeCode: 'LIOCCUTIL',
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            isTermExercised: false,
          },
          {
            ...defaultTestFormLeaseTerm,
            isTermExercised: false,
          },
        ],
      },
    });

    const row = findFirstRow() as HTMLElement;
    expect(row).not.toBeNull();
    expect(findCell(row, 11)?.textContent).toContain('Generate H1005(a)');
  });

  it('calls onGenerate when generation button is clicked', async () => {
    const {
      component: { queryAllByTitle },
    } = await setup({
      initialValues: {
        ...new LeaseFormModel(),
        leaseTypeCode: 'LIOCCUTIL',
        terms: [
          {
            ...defaultTestFormLeaseTerm,
            isTermExercised: false,
            payments: [{ amountTotal: 1, receivedDate: '2020-01-01T18:00' }] as FormLeasePayment[],
          },
        ],
      },
    });

    const generateButton = queryAllByTitle('Generate H1005(a)')[0];
    expect(generateButton).toBeVisible();

    await act(async () => userEvent.click(generateButton));
    expect(onGenerate).toHaveBeenCalled();
  });
});
